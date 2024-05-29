using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSpecFlowWebCalc.PageObject
{
    public class CalculatorPageObject
    {
        private const string CalculatorUrl = "https://specflowoss.github.io/Calculator-Demo/Calculator.html";

        private readonly IWebDriver _webDriver;

        public const int DefaultWaitInSeconds = 5;

        public CalculatorPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        private IWebElement FirstNumberElement
        {
            get
            {
                return _webDriver.FindElement(By.Id("first-number")); //assignes the web page element by the ID first-number to the IWebElement.
            }
        }

        private IWebElement SecondNumberElement
        {
            get
            {
                return _webDriver.FindElement(By.Id("second-number"));
            }
        }

        private IWebElement AddButtonElement
        {
            get
            {
                return _webDriver.FindElement(By.Id("add-button"));
            }
        }

        private IWebElement ResultElement => _webDriver.FindElement(By.Id("result")); // shorted version of the ones above

        private IWebElement ResetButtonElement => _webDriver.FindElement(By.Id("reset-button"));

        public void EnterFirstNumber(string number)
        {
            FirstNumberElement.Clear(); //clear the first number page element
            FirstNumberElement.SendKeys(number);
        }

        public void EnterSecondNumber(string number)
        {
            SecondNumberElement.Clear();
            SecondNumberElement.SendKeys(number);
        }

        public void ClickAddButton()
        {
            AddButtonElement.Click(); // clicks on the web page element
        }

        public void EnsureCalculatorIsOpenAndReset()
        {
            // if calculator page isnt open, open it else reset it
            if(_webDriver.Url != CalculatorUrl)
            {
                _webDriver.Url = CalculatorUrl;
            }
            else
            {
                ResetButtonElement.Click();

                WaitForEmptyResult();
            }
        }

        //wait untill the result element's value to not be empty
        public string WaitForNonEmptyResult()
        {
            return WaitUntil(() => ResultElement.GetAttribute("value"), result => !string.IsNullOrEmpty(result));
        }
        
        // wait until the result element's value is empty.
        public string WaitForEmptyResult()
        {
            return WaitUntil(() => ResultElement.GetAttribute("value"), result => result == string.Empty);
        }

        private T WaitUntil<T>(Func<T> getResult, Func<T,bool> isResultAccepted) where T : class
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
            return wait.Until(drive =>
            {
                var result = getResult();
                if (!isResultAccepted(result)) return default;

                return result;
            });
        }

    }
}
