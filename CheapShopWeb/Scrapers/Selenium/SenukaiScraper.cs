using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CheapShopWeb.Scrapers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Selenium
{
    class SenukaiScraper : AbstractSeleniumScraper
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
            System.Threading.Thread.Sleep(1000);
            if (driver.FindElements(By.CssSelector(".sn-docs.js-product-container.product-grid-row")).Count != 0 || driver.FindElements(By.CssSelector(".sn-docs.js-product-container.product-list-row")).Count != 0) //nezinau ar antra puse reikalinga
            {
                return true;
            }
            return false;
        }

        protected override (string, string, string, string) GetInfo(IWebElement product)
        {
            Regex r = new Regex("\\d*,\\d*");

            var price = r.Match(product.FindElement(By.CssSelector("span.item-price")).Text).Value.Replace(" ", "");
            var name = product.FindElement(By.CssSelector("a.new-product-name")).Text;
            var productUrl = product.FindElement(By.CssSelector("a.new-product-name")).GetAttribute("href"); //click on name to open the page
            var photoUrl = product.FindElement(By.CssSelector("a.new-product-image")).GetAttribute("src");
            return (price, name, productUrl, photoUrl);
        }
        protected override (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string photoUrl)
        {
            if (driver.FindElements(By.CssSelector("span.page-path-v2__item ")).Count > 0)
            {
                var group = driver.FindElement(By.CssSelector("span.page-path-v2__item ")).Text; //fix needed
                photoUrl = driver.FindElement(By.CssSelector("img.product-gallery-slider__slide__image")).GetAttribute("src");
                return (group, photoUrl);
            }

            return ("None", "https://upload.wikimedia.org/wikipedia/commons/0/0a/No-image-available.png");
        }

        protected override ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver)
        {
            return driver.FindElements(By.CssSelector("div.new-product-item.catalog-taxons-product"));
        }



        protected override bool ShouldScrapeIf(IWebElement product)
        {
            if (product.FindElements(By.CssSelector(".catalog-taxons-product__out-of-stock")).Count > 0)
            {
                return false;
            }

            return true;
        }

        protected override bool ShouldStopScraping(ChromeDriver nextPage, string urlBefor)
        {
            return nextPage.Url.Equals(urlBefor);
        }
    }
}
