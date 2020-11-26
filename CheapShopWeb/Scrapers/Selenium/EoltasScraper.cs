using System;
using System.Collections.ObjectModel;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    internal class EoltasScraper : AbstractSeleniumScraper
    {
        protected override void NavigateToNextPage(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("a.btn.btn-pager")).Count > 0)
            {
                var page = Convert.ToInt32(driver.FindElement(By.CssSelector("a.btn.btn-pager.active")).Text);
                var search = driver.Url.Split('/');
                var link = "https://eoltas.lt/lt_LT/search/" + search[4] + "/2?page=" + (page + 1);
                driver.Navigate().GoToUrl(link);
            }
        }

        protected override bool AnyElements(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("p.h3.clr-muted")).Count == 0) return true;

            return false;
        }

        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string photoUrl)
        {
            return ("Automobiliu_prekes", photoUrl);
        }

        protected override bool ShouldStopScraping(ChromeDriver nextPage, string urlBefor)
        {
            if (urlBefor.Equals(nextPage.Url)) return true;

            return false;
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            var list = driver.FindElements(By.ClassName("cont-product"));
            return list;
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            if (product.FindElements(By.CssSelector("p.attr-el__content")).Count > 0) return true;

            return false;
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            var price = product.FindElement(By.CssSelector("div.attr-el.state-price"))
                .FindElement(By.ClassName("attr-el__content")).Text.Split('/');
            var list = product.FindElements(By.ClassName("attr-el__content"));
            var name = product.FindElement(By.CssSelector("a.cont-product__cell.cell-title")).Text;
            var pav = name.Split('\'');
            name = pav[0];
            var productUrl = product.FindElement(By.CssSelector("a.cont-product__cell.cell-title"))
                .GetAttribute("href");
            var photoUrl = product.FindElement(By.CssSelector("img.cont-product__cell.cell-img"))
                .GetAttribute("src");
            return (price[0].Replace(',', '.'), name, productUrl, photoUrl);
        }
    }
}