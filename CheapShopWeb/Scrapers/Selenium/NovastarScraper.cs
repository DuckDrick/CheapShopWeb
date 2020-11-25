using System.Collections.ObjectModel;
using System.Threading;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace CheapShopWeb.Selenium
{
    internal class NovastarScraper : AbstractSeleniumScraper
    {
        protected override void NavigateToNextPage(ChromeDriver driver)
        {
            var m = driver.FindElementsByClassName("list-pagination");
            if (m.Count != 0)
            {
                var pages = m[0].FindElements(By.TagName("a"));
                var n = pages[pages.Count - 1];

                ((IJavaScriptExecutor) driver)
                    .ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                var action = new Actions(driver);
                action.MoveToElement(n);
                var ad = driver.FindElementsByXPath("//*[@id=\"soundest-forms-container\"]");
                if (ad.Count > 0)
                    ((IJavaScriptExecutor) driver)
                        .ExecuteScript("document.getElementById(\"soundest-forms-container\").remove();");

                action.Click().Build().Perform();
                Thread.Sleep(3000);
            }
        }

        protected override bool AnyElements(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("div.novanaut.novanaut--head.face-less")).Count == 1) return false;
            return true;
        }

        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string productUrl)
        {
            Thread.Sleep(1500);
            var group = driver.FindElement(By.ClassName("breadcrumbs"));
            var g = group.FindElements(By.TagName("span"));
            var kk = g[1].Text;
            return (kk, productUrl);
        }

        protected override bool ShouldStopScraping(ChromeDriver chromeDriver, string urlBefore)
        {
            Thread.Sleep(1000);
            if (chromeDriver.Url.Equals(urlBefore)) return true;
            return false;
        }


        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            ReadOnlyCollection<IWebElement> list;
            Thread.Sleep(3000);
            try
            {
                list = driver.FindElements(By.ClassName("product__item"));
            }
            catch
            {
                return null;
            }

            if (list != null)
                return list;
            return driver.FindElements(By.ClassName("product__item-mobile"));
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            return true; //Vėl true? - true, nes jie nededa itemu kuriu nera, bent kiek radau
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            string price;
            if (product.FindElements(By.CssSelector("span.price__value.price__value--discounted")).Count == 1)
                price = product.FindElement(By.CssSelector("span.price__value.price__value--discounted")).Text;
            else if (product.FindElements(By.ClassName("price__standard")).Count == 1)
                price = product.FindElement(By.ClassName("price__standard")).Text;
            else
                price = product.FindElement(By.ClassName("price__value")).Text;
            var name = product.FindElement(By.ClassName("link--dark")).Text;
            var productUrl = product.FindElement(By.CssSelector("a.link--dark")).GetAttribute("href");
            var photoUrl = product.FindElement(By.TagName("img"))
                .GetAttribute("src");
            return (price, name, productUrl, photoUrl);
        }
    }
}