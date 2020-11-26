using System;
using System.Collections.ObjectModel;
using System.Linq;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    internal class RdeScraper : AbstractSeleniumScraper
    {
        protected override void NavigateToNextPage(ChromeDriver driver)
        {
            var beforeUrl = driver.Url;
            var currentUrl = driver.Url.Split('/');
            var url = "https://rde.lt/search/lt/word/" + currentUrl[6] + "/page/" +
                      (Convert.ToInt32(currentUrl.Last()) + 1);
            driver.Navigate().GoToUrl(url);
            if (driver.FindElements(By.CssSelector("div.search_page_header")).Count == 1)
                if (driver.FindElement(By.CssSelector("div.search_page_header")).Text
                    .Equals("Deja, nieko nepavyko rasti."))
                    driver.Navigate().GoToUrl(beforeUrl);
        }

        protected override bool AnyElements(ChromeDriver driver)
        {
            if (driver.FindElements(By.CssSelector("div.search_page_header")).Count == 1)
                if (driver.FindElement(By.CssSelector("div.search_page_header")).Text
                    .Equals("Deja, nieko nepavyko rasti."))
                    return false;

            return true;
        }

        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string photoUrl)
        {
            var list = driver.FindElement(By.CssSelector("div.big_box_header")).FindElements(By.CssSelector("span"));
            if (list.Count > 1) return (list[1].Text, photoUrl);

            return ("None", photoUrl);
        }

        protected override bool ShouldStopScraping(ChromeDriver nextPage, string urlBefor)
        {
            return nextPage.Url.Equals(urlBefor);
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            var list = driver.FindElements(By.CssSelector("div.product_box_div"));
            return list;
        }

        protected override bool ShouldScrapeIf(IWebElement product)
        {
            return true; //true, nes nededa itemu kurie isparduoti, kiek radau
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            var price = product.FindElement(By.CssSelector("div.product_price_wo_discount_listing")).Text.Split(':')
                .Last();
            var name = product.FindElement(By.ClassName("product_name")).Text;
            var productUrl = product.FindElement(By.ClassName("product_name")).FindElement(By.CssSelector("a"))
                .GetAttribute("href");
            var photoUrl = product.FindElement(By.CssSelector("img.product_photo_grid")).GetAttribute("src");
            ;

            return (price, name, productUrl, photoUrl);
        }
    }
}