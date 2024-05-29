using NUnit.Framework;
using SeleniumShoppingSite.Drivers;
using SeleniumShoppingSite.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumShoppingSite.StepDefinitions
{
    [Binding]
    public class OnlineShopLoginStepDefinitions
    {
        private readonly OnlineShopLoginPageObject _loginPageObject;

        public OnlineShopLoginStepDefinitions(BrowserDriver browserDriver)
        {
            _loginPageObject = new OnlineShopLoginPageObject(browserDriver.Current);
        }

        [Given ("Username is (.*)")]
        public void GivenUsernameIsProvided(string username)
        {
            _loginPageObject.EnterUserName(username);
        }

        [Given("Password is (.*)")]
        public void GivenPasswordIsProvided(string password)
        {
            _loginPageObject.EnterPassword(password);
        }

        [When("User selects the login button")]
        public void WhenUserSelectsLoginButton()
        {
            _loginPageObject.ClickLogin();
        }

        [Then("New page is loaded")]
        public void ThenNewPageIsLoaded()
        {
            if (_loginPageObject.GetUrl() != "https://www.saucedemo.com/inventory.html")
            {
                Assert.Fail("User failed to log in");
            }
        }
    }
}
