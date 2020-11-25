using System.Collections.ObjectModel;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    internal class TopocentrasScraper : AbstractSeleniumScraper
    {
        protected override void NavigateToNextPage(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("a.Pager-nextButton-3UR")).Count == 1)
            {
                var nuoroda = driver.FindElement(By.CssSelector("a.Pager-nextButton-3UR")).GetAttribute("href");

                driver.Navigate().GoToUrl(nuoroda);
            }
        }

        protected override bool AnyElements(ChromeDriver driver)
        {
            if (driver.FindElements(By.ClassName("ProductNotFoundPage-errorHeader-2Xo")).Count == 0) return true;

            return false;
        }

        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string productUrl)
        {
            var group = driver.FindElements(By.ClassName("breadcrumbs-breadcrumbLink-2NB"));
            foreach (var productgroup in group)
                if (!productgroup.Text.Equals("Topocentras"))
                {
                    var img = driver.FindElement(By.ClassName("carousel-mainImage-2gm")).GetAttribute("src");
                    return (productgroup.Text, img);
                }

            return ("None", "");
        }

        protected override bool ShouldStopScraping(ChromeDriver nextPage, string urlBefore)
        {
            return nextPage.Url == urlBefore;
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            var list = driver.FindElements(By.ClassName("ProductGrid-productWrapper-1hm"));
            return list;
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            return true;
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            var price = product.FindElement(By.ClassName("Price-price-27p")).Text;
            var name = product.FindElement(By.ClassName("ProductGrid-productName-1JN")).Text;
            var productUrl = product.FindElement(By.ClassName("ProductGrid-link-3Q6")).GetAttribute("href");
            var photoUrl = "";

            return (price, name, productUrl, photoUrl);
        }
    }
}