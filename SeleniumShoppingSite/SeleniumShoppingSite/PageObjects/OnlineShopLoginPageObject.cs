using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumShoppingSite.PageObjects
{
    public class OnlineShopLoginPageObject
    {
        private string _shopUrl = "https://www.saucedemo.com/";

        private readonly IWebDriver _webDriver;

        public int DefaultWaitInSeconds = 5;

        public OnlineShopLoginPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _webDriver.Url = _shopUrl;
        }

        private IWebElement userNameElement
        {
            get
            {
                return _webDriver.FindElement(By.Id("user-name"));
            }
        }

        private IWebElement passwordElement
        {
            get
            {
                return _webDriver.FindElement(By.Id("password"));
            }
        }

        private IWebElement loginButton => _webDriver.FindElement(By.Id("login-button"));

        public void EnterUserName(string userName)
        {
            userNameElement.Clear();
            userNameElement.SendKeys(userName);
        }

        public void EnterPassword(string password)
        {
            passwordElement.Clear();
            passwordElement.SendKeys(password);
        }

        public void ClickLogin()
        {
            loginButton.Click();
        }

        public void EnsureOnlineShopIsOpen()
        {
            if(_webDriver.Url != _shopUrl)
            {
                _webDriver.Url = _shopUrl;
            }
            else
            {
                userNameElement.Clear();
                passwordElement.Clear();
            }
        }

        public string GetUrl()
        {
            return _webDriver.Url;
        }


    }
}
