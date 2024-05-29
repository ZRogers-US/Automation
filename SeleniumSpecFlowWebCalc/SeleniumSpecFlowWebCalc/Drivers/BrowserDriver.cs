using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSpecFlowWebCalc.Drivers
{
    public class BrowserDriver : IDisposable
    {
        private readonly Lazy<IWebDriver> _driver;
        private bool _isDisposed;

        public BrowserDriver()
        {
            _driver = new Lazy<IWebDriver>(CreateWebDriver);
        }

        public IWebDriver Current =>_driver.Value;

        private IWebDriver CreateWebDriver()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();

            var chromeOptions = new ChromeOptions();

            var chromeDriver = new ChromeDriver(chromeDriverService, chromeOptions);

            return chromeDriver;
        }

        public void Dispose()
        {
            if(_isDisposed) return;

            if(_driver.IsValueCreated)
            {
                Current.Quit();
            }

            _isDisposed = true;
        }
    }
}
