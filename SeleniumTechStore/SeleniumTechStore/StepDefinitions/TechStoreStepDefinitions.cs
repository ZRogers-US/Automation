using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using OpenQA.Selenium;
using SeleniumTechStore.Drivers;
using SeleniumTechStore.PageModel;
using System.Runtime.InteropServices;

namespace SeleniumTechStore.StepDefinitions
{
    [Binding]
    public sealed class TechStoreStepDefinitions
    {
        private string _techStoreUrl = "https://www.demoblaze.com/index.html";
        private TechStorePageObjectModel _techStorePageObjectModel;

        public TechStoreStepDefinitions(BrowserDriver webDriver)
        {
            _techStorePageObjectModel = new TechStorePageObjectModel(webDriver.Current);
        }

        [Given ("A New user wants to sign up")]
        public void GivenANewUserToSignUp()
        {
            _techStorePageObjectModel.SelectSignUpNavButton();
            _techStorePageObjectModel.UserState = userState.UNREGISTERED;
        }

        [Given ("A returning user wants to sign in")]
        public void GivenReturningUserToSignIn()
        {
            _techStorePageObjectModel.SelectLogInNavButton();
            _techStorePageObjectModel.UserState = userState.REGISTERED;
        }

        [Given ("the username is (.*)")]
        public void GivenTheUsernameIs(string username)
        {
            //_techStorePageObjectModel.SelectSignUpNavButton(); // move to before hook, tempmoved to new given step
            if (_techStorePageObjectModel.UserState == userState.UNREGISTERED)
            {
                _techStorePageObjectModel.AddSignupUsername(username);
            }
            else
            {
                _techStorePageObjectModel.AddLoginUsername(username);
            }
            
        }

        [Given ("the password is (.*)")]
        public void GivenThePasswordIs(string password)
        {
            if (_techStorePageObjectModel.UserState == userState.UNREGISTERED)
            {
                _techStorePageObjectModel.AddSignupPassword(password);
            }
            else
            {
                _techStorePageObjectModel.AddLoginPassword(password);
            }
        }

        [When ("the user selects the signup button")]
        public void WhenTheUserSelectsTheSignUpButton()
        {
            _techStorePageObjectModel.SelectSignupButton();
        }

        [When("the user selects the login button")]
        public void WhenTheUserSelectsTheLoginButton()
        {
            _techStorePageObjectModel.SelectLoginButton();
        }

        [Then ("an alert appears saying Signup Successful")]
        public void ThenSignUpSuccessfulAlertDisplays()
        {
            _techStorePageObjectModel.ConfirmSignup();
        }


        [Then("the user logs out")]
        public void ThenTheUserLogsOut()
        {
            //_techStorePageObjectModel.ConfirmSignin(username);
            _techStorePageObjectModel.SelectLogOutNavButton();
        }
    }
}
