using System;
using System.Collections.ObjectModel;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    internal class AutoaibeScraper : AbstractSeleniumScraper
    {
        protected override void NavigateToNextPage(ChromeDriver driver)
        {
            if (driver.FindElements(By.ClassName("pagination")).Count > 0)
            {
                var page = driver.FindElement(By.CssSelector("span.page-numbers")).Text;
                var numbers = page.Split('/');
                if (numbers[0] != numbers[1])
                {
                    page = Convert.ToString(Convert.ToInt32(numbers[0]) + 1);
                    var searchstring = driver.Url.Split('=', '&');
                    var search = searchstring[1];
                    driver.Navigate().GoToUrl("https://autoaibe.lt/search/?q=" + search + "&page=" + page);
                }
            }
        }

        protected override bool AnyElements(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("li.product-item-wrapper.clearfix")).Count == 0) return false;

            return true;
        }

        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string productUrl)
        {
            var group = driver.FindElement(By.CssSelector("div.breadcrumb.last")).Text;
            return (group, productUrl);
        }

        protected override bool ShouldStopScraping(ChromeDriver nextPage, string urlBefor)
        {
            if (urlBefor.Equals(nextPage.Url)) return true;

            return false;
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            var list = driver.FindElements(By.CssSelector("li.product-item-wrapper.clearfix"));
            return list;
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            if (product.FindElements(By.CssSelector("span.product-quantity.empty")).Count > 0) return false;

            return true;
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            var price = product.FindElement(By.ClassName("user-price")).Text;
            var name = product.FindElement(By.ClassName("name")).Text;
            var productUrl = product.FindElement(By.ClassName("name")).GetAttribute("href");
            var photoUrl = product.FindElement(By.TagName("img")).GetAttribute("src");
            if (photoUrl.Contains("svg"))
                photoUrl = "https://upload.wikimedia.org/wikipedia/commons/0/0a/No-image-available.png";
            return (price, name, productUrl, photoUrl);
        }
    }
}