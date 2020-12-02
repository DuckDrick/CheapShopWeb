using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CheapShopWeb.DataContext;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Scrapers.Selenium
{
    internal abstract class AbstractSeleniumScraper
    {
        private string _scrape;
        private readonly ProductDbContext _db = new ProductDbContext();

        private List<ChromeDriver> _productDrivers;
        private ChromeDriver _mainDriver;


  

        private static ChromeDriver createChromeDriver()
        {
            var options = new ChromeOptions();
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            options.AddArguments("--window-size=1920,1080", "--no-sandbox", "--headless");
       
            return new ChromeDriver(chromeDriverService, options);
        }

        private readonly Func<List<ChromeDriver>, Func<ChromeDriver, string, (string, string)>, ConcurrentBag<Models.Product>,
            string, Action<Product, ParallelLoopState, long>> _scrapeInfoFromProductPage =
            (drivers, giGetter, productBag, site) =>
                (Action<Product, ParallelLoopState, long>) (
                    (product, state, index) =>
                    {
                        var productDriver = drivers[(int) index];
                        try
                        {
                            productDriver.Navigate().GoToUrl(product.Link);

                            var tries = 0;
                            while (tries < 10)
                                try
                                {
                                    (product.Group, product.ImageUrl) = giGetter(productDriver, product.ImageUrl);

                                    productBag.Add(new Models.Product
                                    {
                                        name = product.Name,
                                        source = site,
                                        price = product.Price.Replace("€", "").Replace(",", ".").Replace(" ", ""),
                                        photo_link = product.ImageUrl,
                                        product_link = product.Link,
                                        group = product.Group
                                    });
                                    Console.WriteLine(product.Name);
                                    break;
                                }
                                catch (NoSuchElementException)
                                {
                                    tries++;
                                }
                        }
                        catch
                        {
                            productDriver.Close();
                            productDriver.Quit();
                            drivers[(int) index] = createChromeDriver();
                        }
                    }
                );

        private readonly Func<string, string, Func<Models.Product, bool>> _check = (name, source) => (product) => product.name == name && product.source == source;

        public void ScrapeWithSelenium(object args)
        {
            try
            {
                var parameters = new object[2];
                parameters = (object[])args;
                _scrape = (string) parameters.GetValue(0);
                var chromeDrivers = (ChromeDrivers) parameters.GetValue(1);
                _productDrivers = chromeDrivers.ProductChromeDrivers;
                _mainDriver = chromeDrivers.MainChromeDriver;

                try
                {
                    _mainDriver.Navigate().GoToUrl(_scrape);


                    var siteFromUrlRegex = new Regex("\\/[^.]*\\.");
                    
                    bool anyElements;
                    try
                    {
                        anyElements = AnyElements(_mainDriver);
                    }
                    catch (NoSuchElementException)
                    {
                        anyElements = false;
                    }


                    if (!anyElements) return;

                    
                    var site = siteFromUrlRegex.Match(_scrape).Value.Substring(2).Replace(".", ""); // nuoroda.parduotuves.lt
                    
                    string urlBefore;
                    do
                    {
                        var products = new List<Product>();
                        try
                        {

                            var productList = GetProductList(_mainDriver);
                            foreach (var product in productList)
                            {
                                if (!ShouldScrapeIf(product)) continue;
                                var (price, name, productUrl, photoUrl) = GetInfo(product);
                                if (!_db.Products.Any(_check(name, site)))
                                    products.Add(new Product(
                                        name,
                                        price,
                                        productUrl,
                                        photoUrl, 
                                        "None", 
                                        site
                                        )
                                    );
                            }
                        }
                        catch (NoSuchElementException e)
                        {
                            try
                            {
                                var path = "C:/Log/";
                                var fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() +
                                               DateTime.Now.Year.ToString() + DateTime.Now.Second +
                                               DateTime.Now.Millisecond + "WebDriverException_Logs.txt";
                                System.IO.StreamWriter file = new System.IO.StreamWriter(path + fileName, true);
                                file.WriteLine(DateTime.Now + ": " + e +
                                               " // NO SUCH ELEMENT EXCEPTION -- CHECK SCRAPED PAGE -- " + _scrape);
                                file.Close();
                                return;
                            }
                            catch
                            {
                                Trace.WriteLine(e.ToString());
                            }
                        }

                        var amount = int.Parse(ConfigurationManager.AppSettings["ScraperAmount"] ?? "3");
                        for (var take = 0;; take++)
                        {
                            var start = take * amount;
                            var count = products.Count - start >= amount
                                ? amount
                                : products.Count - start;
                            
                            if (count > 0)
                            {
                                var productBag = new ConcurrentBag<Models.Product>();
                                Parallel.ForEach(products.GetRange(start, count), _scrapeInfoFromProductPage(_productDrivers, GetProductGroupAndMaybePhotoLink, productBag, site));
                                _db.Products.AddRange(productBag);
                                _db.SaveChanges();
                            }
                            else
                            {
                                break;
                            }
                        }

                        urlBefore = _mainDriver.Url;
                        NavigateToNextPage(_mainDriver);
                    } while (!ShouldStopScraping(_mainDriver, urlBefore));
                    
                }
                catch (WebDriverException e)
                {
                    try
                    {
                        Trace.WriteLine(e.ToString());
                        var path = "C:/Log/";
                        var fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() +
                                       DateTime.Now.Year.ToString() + DateTime.Now.Second + DateTime.Now.Millisecond +
                                       "WebDriverException_Logs.txt";
                        System.IO.StreamWriter file = new System.IO.StreamWriter(path + fileName, true);
                        file.WriteLine(DateTime.Now.ToString() + ": " + e.ToString() +
                                       "// WebDriverException -- CHECK SCRAPED PAGE -- " + _scrape);
                        file.Close();
                    }
                    catch
                    {
                        Trace.WriteLine(e.ToString());
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        
                        var path = "C:/Log/";
                        var fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second + DateTime.Now.Millisecond + "WebDriverException_Logs.txt";
                        System.IO.StreamWriter file = new System.IO.StreamWriter(path + fileName, true);
                        file.WriteLine(DateTime.Now.ToString() + ": " + e.ToString());
                        file.Close();
                    }
                    catch
                    {
                        Trace.WriteLine(e.ToString());
                    }

                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Thread aborted");
            }

    
        }

        private void CloseDriverList(List<ChromeDriver> drivers)
        {
            foreach (var driver in drivers)
            {
                driver.Close();
                driver.Quit();
            }
        }


        protected abstract void NavigateToNextPage(ChromeDriver driver);
        protected abstract bool AnyElements(ChromeDriver driver);
        protected abstract (string, string) GetProductGroupAndMaybePhotoLink(ChromeDriver driver, string productUrl);
        protected abstract bool ShouldStopScraping(ChromeDriver nextPage, string urlBefor);
        protected abstract ReadOnlyCollection<IWebElement> GetProductList(ChromeDriver driver);
        protected abstract bool ShouldScrapeIf(IWebElement product);

        protected abstract (string, string, string, string)
            GetInfo(IWebElement product); //price, name, product url, photo url
    }
}