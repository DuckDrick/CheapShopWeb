using System.Collections.ObjectModel;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    internal class VarleScraper : AbstractSeleniumScraper
    {
        protected override void NavigateToNextPage(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("a.next")).Count > 0)
            {
                var link = driver.FindElement(By.CssSelector("a.next")).GetAttribute("href");
                driver.Navigate().GoToUrl(link);
            }
        }

        protected override bool AnyElements(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("form#not-found_form.center-errors.errors-margin-bottom")).Count ==
                0) return true;

            return false;
        }

        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string photoUrl)
        {
            if (driver.FindElements(By.CssSelector("li.first")).Count > 0)
            {
                var group = driver.FindElement(By.CssSelector("li.first")).Text;
                photoUrl = driver.FindElement(By.CssSelector("img.main-image")).GetAttribute("src");
                return (group, photoUrl);
            }

            return ("None", "https://upload.wikimedia.org/wikipedia/commons/0/0a/No-image-available.png");
        }

        protected override bool ShouldStopScraping(ChromeDriver nextPage, string urlBefor)
        {
            if (urlBefor.Equals(nextPage.Url)) return true;

            return false;
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            var list = driver.FindElements(By.CssSelector("div.grid-item.product"));
            return list;
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            if (product.FindElements(By.CssSelector("span.price")).Count > 0) return true;
            return false;
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            var price = product.FindElement(By.CssSelector("span.price")).Text;
            var name = product.FindElement(By.CssSelector("a.title")).Text;
            var productUrl = product.FindElement(By.CssSelector("a.title")).GetAttribute("href");
            var photoUrl = " ";
            return (price, name, productUrl, photoUrl);
        }
    }
}