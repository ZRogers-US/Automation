using System;
using TechTalk.SpecFlow;
using StockApp;

namespace HowToUseSpecFlow.StepDefinitions
{
    [Binding]
    public class PurchasingStockStepDefinitions
    {
        public StockAppClass stockApp = new StockAppClass();

        [Given(@"that i am a StockApp user")]
        public void GivenThatIAmAStockAppUser()
        {
            stockApp.User = "DemoUser";
        }

        [Given(@"my initial portfolio has a value of '([^']*)'")]
        public void GivenMyInitialPortfolioHasAValueOf(string portfolioValue)
        {
            stockApp.SetPortfolioValue(int.Parse(portfolioValue));
        }

        [When(@"i purchase '([^']*)' amount of '([^']*)' at the latest value")]
        public void WhenIPurchaseAmountOfAtTheLatestValue(string purchaseAmount, string stockCode)
        {
            stockApp.PurchaseStock(purchaseAmount, stockCode);
        }

        [Then(@"My portfolio has increased in value '([^']*)'")]
        public void ThenMyPortfolioHasIncreasedInValue(string portfolioValue)
        {
            int initialPortfolioValue = Int32.Parse(portfolioValue);
            int newPortfolioValue = stockApp.PortfolioValue;

            newPortfolioValue.Should().BeGreaterThan(initialPortfolioValue);
        }
    }
}
