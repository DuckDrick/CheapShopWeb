using System.Collections.ObjectModel;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    internal class SkytechScraper : AbstractSeleniumScraper
    {
        private string GetProductImage(ChromeDriver driver)
        {
            return driver.FindElement(By.Id("main-product-image")).GetAttribute("src");
        }

        protected override void NavigateToNextPage(ChromeDriver driver)
        {
        }

        protected override bool AnyElements(ChromeDriver driver)
        {
            if (driver.FindElements(By.ClassName("productListing-info")).Count == 0) return true;

            return false;
        }

        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string productUrl)
        {
            var list = driver.FindElement(By.ClassName("navbar-breadcrumb"));
            var group = list.FindElements(By.CssSelector("a"));
            foreach (var productgroup in group)
                if (!productgroup.Text.Equals("Pradžia"))
                    return (productgroup.Text, GetProductImage(driver));

            return ("None", "");
        }

        protected override bool ShouldStopScraping(ChromeDriver nextPage, string urlBefor)
        {
            return true;
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            var list = driver.FindElements(By.CssSelector("tr.productListing"));
            return list;
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            return true;
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            var price = product.FindElement(By.CssSelector("strong")).Text;
            var name = product.FindElement(By.ClassName("name")).Text;
            var productUrl = product.FindElement(By.CssSelector("a")).GetAttribute("href");
            var photoUrl = "";

            return (price, name, productUrl, photoUrl);
        }
    }
}