using System.Collections.ObjectModel;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    internal class ErmitazasScraper : AbstractSeleniumScraper
    {
        protected override void NavigateToNextPage(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("a.next.ml-2.padding-4-8-2")).Count > 0)
            {
                var newurl = driver.FindElement(By.CssSelector("a.next.ml-2.padding-4-8-2")).GetAttribute("href");
                driver.Navigate().GoToUrl(newurl);
            }
        }

        protected override bool AnyElements(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("div.alert.alert-warning.text-center")).Count == 0) return false;

            return true;
        }

        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string imageUrl)
        {
            var group = driver.FindElement(By.CssSelector("li.active.end")).Text.Split('(');
            //imageUrl = driver.FindElement(By.TagName("a")).GetAttribute("href");
            return (group[0], imageUrl);
        }

        protected override bool ShouldStopScraping(ChromeDriver nextPage, string urlBefor)
        {
            if (urlBefor.Equals(nextPage.Url)) return true;
            return false;
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            var list = driver.FindElements(By.ClassName("media-item"));
            return list;
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            return true; // scrapinti jeigu buvo rasta itemai kurie neispirkti
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            string price;
            if (product.FindElements(By.CssSelector("span.price.orange")).Count == 0)
                price = product.FindElement(By.CssSelector("span.price.grey")).Text;
            else
                price = product.FindElement(By.CssSelector("span.price.orange")).Text;
            var name = product.FindElement(By.CssSelector("span.media-title.lh17.inline-block.hover-underline")).Text;
            var productUrl = product.FindElement(By.CssSelector("a")).GetAttribute("href");
            var photoUrl = product.FindElement(By.CssSelector("img.minh300.maxh255.center-img.lazy"))
                .GetAttribute("src");
            return (price, name, productUrl, photoUrl);
        }
    }
}