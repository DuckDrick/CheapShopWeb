using System.Collections.ObjectModel;
using System.Linq;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    internal class PiguScraper : AbstractSeleniumScraper
    {
        protected override void NavigateToNextPage(ChromeDriver driver)
        {
            driver.Navigate()
                .GoToUrl(driver.FindElementByXPath("//*[@id=\"pagination\"]/div[1]/a[2]").GetAttribute("href"));
        }


        protected override bool AnyElements(ChromeDriver driver)
        {
            return driver.FindElementsByXPath("//*[@id=\"productListLoader\"]").Count > 0;
        }


        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string imageUrl)
        {
            var pagehtml = driver.FindElementByTagName("body").GetProperty("innerHTML");
            var groups = driver.FindElementByXPath("//*[@id=\"breadCrumbs\"]");
            var list = groups.FindElements(By.TagName("li"));
            return (list[1].Text.Trim(), imageUrl);
        }

        protected override bool ShouldStopScraping(ChromeDriver chromeDriver, string urlBefor)
        {
            var splitLink = chromeDriver.Url.Split('/');
            return splitLink[splitLink.Length - 1].Equals("#");
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            var products = driver.FindElementByXPath("//*[@id=\"productListLoader\"]")
                .FindElements(By.XPath("//div[contains(@class, \"product-list-item\")]"))
                .Where(product => product.GetAttribute("widget-old") != null).ToList();
            ;
            return new ReadOnlyCollection<IWebElement>(products);
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            var spanList = product.FindElement(By.ClassName("product-price")).FindElements(By.TagName("span"));
            return spanList.Count > 1;
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            var price = product.FindElement(By.XPath("div/div/div[2]/span[2]")).Text.Replace(" ", "").Replace("€", "") +
                        "€";
            var name = product.FindElement(By.XPath("div/div/a[2]/img")).GetAttribute("alt");
            name = name.Substring(0, name.IndexOf("kaina ir informacija")).Trim();
            var productUrl = product.FindElement(By.XPath("div/div/a[2]")).GetAttribute("href");
            var photoUrl = product.FindElement(By.XPath("div/div/a[2]/img")).GetAttribute("src");

            return (price, name, productUrl, photoUrl);
        }
    }
}