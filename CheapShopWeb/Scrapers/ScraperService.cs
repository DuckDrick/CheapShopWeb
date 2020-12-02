using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CheapShopWeb.Scrapers.Selenium;

namespace CheapShopWeb.Scrapers
{
    public class ScraperService
    {
        public CustomList<string> queryList;
        public static int running;
        private List<KeyValuePair<ScrapedSites, Thread>> scrapers;

        public ScraperService()
        {
            queryList = new CustomList<string>();
            scrapers = new List<KeyValuePair<ScrapedSites, Thread>>();
            

            queryList.BecameNonEmpty += delegate
            {
                CreateScrapers();
                OpenScrapers();
                while (!queryList.IsEmpty())
                {
                    Scrape(queryList.Peek());
                    queryList.Remove();
                    CreateScrapers();
                }
               
            };

            queryList.BecameEmpty += delegate { Kill(); };
        }


        public void CreateScrapers()
        {
            KillThreads();
            var sources = Enum.GetNames(typeof(ScrapedSites)).ToArray();
            foreach (var source in sources)
            {
                try
                {
                    var className = source.First().ToString().ToUpper() + source.Substring(1) + "Scraper";
                    var scraper = Type.GetType("CheapShopWeb.Selenium." + className);
                    var instance = (AbstractSeleniumScraper) Activator.CreateInstance(scraper);
                    var thread = new Thread(instance.ScrapeWithSelenium);
                    scrapers.Add(
                        new KeyValuePair<ScrapedSites, Thread>((ScrapedSites) Enum.Parse(typeof(ScrapedSites), source),
                            thread));
                }
                catch
                {
                    Trace.WriteLine("SCRAPER NOT FOUND " + source);
                }
            }
        }

        private void KillThreads()
        {
            foreach (var scraper in scrapers)
            {
                scraper.Value.Abort();
            }
            scrapers.Clear();
        }

        private List<ChromeDrivers> _scraperDrivers;
        public void OpenScrapers()
        {
            _scraperDrivers = new List<ChromeDrivers>();
            foreach (var _ in scrapers)
            {
                _scraperDrivers.Add(new ChromeDrivers());
            }
        }

        public void Scrape(string query)
        {
            int index = 0;
            foreach (var scraper in scrapers)
            {
                var url = Values.Urls[(int) scraper.Key];
                var urlSplit = url.Split('☼');
                var ending = "";
                if (urlSplit.Length > 1)
                {
                    url = urlSplit[0];
                    ending = urlSplit[1];
                }
            
                var scrapeSite = url + query.Replace(" ", "+") + ending;
                scraper.Value.Start(new object[] {scrapeSite, _scraperDrivers[index++]});
                running++;
            }
           
            foreach (var scraper in scrapers)
            {
                scraper.Value.Join();

            }
        }

        public void Kill()
        {
            foreach (var driver in _scraperDrivers)
            {
                foreach (var productDriver in driver.ProductChromeDrivers)
                {
                    productDriver.Close();
                    productDriver.Quit();
                }
                driver.MainChromeDriver.Close();
                driver.MainChromeDriver.Quit();
            }
        }
    }
}