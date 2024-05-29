using FluentAssertions.Specialized;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTechStore.PageModel
{
    public enum userState  
        {
            UNREGISTERED,
            REGISTERED
        };

    public class TechStorePageObjectModel
    {
        private string _techStoreUrl = "https://www.demoblaze.com/index.html";
        private readonly IWebDriver _driver;
        public userState UserState { get; set; } = userState.UNREGISTERED;

        private int _defaultWaitInSeconds = 5;

        public TechStorePageObjectModel(IWebDriver driver)
        {
            _driver = driver;
            _driver.Url =_techStoreUrl;
            //_driver
        }

        private IWebElement signUpNavButton
        {
            get
            {
                return _driver.FindElement(By.Id("navbarExample")).FindElement(By.Id("signin2"));
            }
        }

        private IWebElement loginNavButton
        {
            get
            {
                return _driver.FindElement(By.Id("login2"));
            }
        }
        private IWebElement logoutNavButton
        {
            get
            {
                return _driver.FindElement(By.Id("logout2"));
            }
            set
            {
                logoutNavButton = value;
            }
        }

        private IWebElement signUpUsernameInput
        {
            get
            {
                return _driver.FindElement(By.Id("signInModal")).FindElement(By.ClassName("modal-content")).FindElement(By.Id("sign-username"));
            }
        }

        private IWebElement signUpPasswordInput
        {
            get
            {
                return _driver.FindElement(By.Id("sign-password"));
            }
        }

        private IWebElement loginUsernameInput
        {
            get
            {
                return _driver.FindElement(By.Id("loginusername"));//.FindElement(By.Id("logInModal")).FindElement(By.ClassName("modal-content"))
            }
        }

        private IWebElement loginPasswordInput
        {
            get
            {
                return _driver.FindElement(By.Id("loginpassword"));
            }
        }

        private IWebElement signUpButton
        {
            get
            {
                return _driver.FindElement(By.Id("signInModal")).FindElement(By.ClassName("btn-primary"));
            }
        }

        private IWebElement loginButton
        {
            get
            {
                return _driver.FindElement(By.Id("logInModal")).FindElement(By.ClassName("btn-primary"));
            }
        }

        private IWebElement welcomeMessage
        {
            get
            {
                return _driver.FindElement(By.Id("nameofuser"));
            }
        }

        public void SelectSignUpNavButton()
        {
            WaitForElementToBeDisplayed(signUpNavButton);
            signUpNavButton.Click();
        }

        public void SelectLogInNavButton()
        {
            WaitForElementToBeDisplayed(loginNavButton);
            loginNavButton.Click();
        }

        public void SelectLogOutNavButton()
        {
            bool elementIsStale = true;
            int numOfTries = 10;
            while(elementIsStale && numOfTries > 0)
            {
                try
                {
                    WaitForElementToBeDisplayed(logoutNavButton);
                    elementIsStale = false;
                    numOfTries--;
                    logoutNavButton.Click();
                }
                catch (StaleElementReferenceException)
                {
                    //logoutNavButton = _driver.FindElement(By.Id("logout2"));
                    elementIsStale = true;
                }
            }
        }

        public void AddSignupUsername(string username)
        {
            WaitForElementToBeDisplayed(signUpUsernameInput);
            signUpUsernameInput.Clear();
            signUpUsernameInput.SendKeys(username);
        }

        public void AddSignupPassword(string password)
        {
            WaitForElementToBeDisplayed(signUpPasswordInput);
            signUpPasswordInput.Clear();
            signUpPasswordInput.SendKeys(password);
        }

        public void AddLoginUsername(string username)
        {
            WaitForElementToBeDisplayed(loginUsernameInput);
            loginUsernameInput.Clear();
            loginUsernameInput.SendKeys(username);
        }

        public void AddLoginPassword(string password)
        {
            WaitForElementToBeDisplayed(loginPasswordInput);
            loginPasswordInput.Clear();
            loginPasswordInput.SendKeys(password);
        }

        public void SelectSignupButton()
        {
            signUpButton.Click();
        }

        public void SelectLoginButton()
        {
            loginButton.Click();
        }

        public void ConfirmSignin(string username)
        {

            //WaitForElementToBeDisplayed(welcomeMessage);
            string[] welcomeMessageText = welcomeMessage.Text.Split(" ");
            if (welcomeMessageText[1] != username)
            {
                Assert.Fail("Signin Failed");
            }
        }

        public void ConfirmSignup()
        {
            IAlert alert = WaitForAlert();
            if (alert.Text != "Sign up successful.") Assert.Fail(alert.Text);
        }

        public IWebElement WaitForElementToBeDisplayed(IWebElement element)
        { return WaitUntil(
                () => element,
                (result) => result.Displayed
                );
        }

        public IAlert WaitForAlert()
        {
            WebDriverWait wait = new WebDriverWait(_driver,TimeSpan.FromSeconds(_defaultWaitInSeconds));

            return wait.Until( (driver) =>
            {
                try
                {
                    IAlert alert = driver.SwitchTo().Alert();
                    return alert;
                }
                catch(NoAlertPresentException)
                {
                    return default;
                }
            });//wait.Until(ExpectedConditions.AlertIsPresent());
        }


        private T WaitUntil<T>(Func<T> getResults, Func<T,bool> isResultAccepted) where T: class
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(_defaultWaitInSeconds));

            return wait.Until( (driver) =>
            {
                var result = getResults();
                if (!isResultAccepted(result)) return default;
                return result;
            });
        }
    }
}
