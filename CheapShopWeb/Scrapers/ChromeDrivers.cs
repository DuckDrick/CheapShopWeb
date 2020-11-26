using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using OpenQA.Selenium.Chrome;

namespace CheapShopWeb.Scrapers
{
    public class ChromeDrivers
    {
        public List<ChromeDriver> ProductChromeDrivers { get; }
        public ChromeDriver MainChromeDriver { get; }

        public ChromeDrivers()
        {
            var options = new ChromeOptions();
            var chromeDriverService = ChromeDriverService.CreateDefaultService();

            var scraperAmount = int.Parse(ConfigurationManager.AppSettings["ScraperAmount"] ?? "3");

            chromeDriverService.HideCommandPromptWindow = true;
            options.AddArguments("--window-size=1920,1080", "--no-sandbox", "--headless");

            ProductChromeDrivers = FillWithDrivers(scraperAmount, options, chromeDriverService).ToList();
            MainChromeDriver = new ChromeDriver(chromeDriverService, options);
        }

        private static IEnumerable<ChromeDriver> FillWithDrivers(int amount, ChromeOptions chromeOptions, ChromeDriverService chromeDriverService)
        {
            for (var i = 0; i < amount; i++)
            {
                ChromeDriver driver;
                try
                {
                    driver = new ChromeDriver(chromeDriverService, chromeOptions);
                }
                catch
                {
                    driver = new ChromeDriver(chromeDriverService, chromeOptions);
                }

                yield return driver;
            }
        }
    }
}