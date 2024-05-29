using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumShoppingSite.Drivers;
using SeleniumShoppingSite.PageObjects;

namespace SeleniumShoppingSite.StepDefinitions
{
    [Binding]
    public class ExampleFeatureStepDefinitions
    {
        private readonly OnlineShopInventoryPageObject _inventoryPageObject;

        private ScenarioContext _scenarioContext;

        public ExampleFeatureStepDefinitions(BrowserDriver browserDriver, ScenarioContext scenarioContext)
        {
            _inventoryPageObject = new OnlineShopInventoryPageObject(browserDriver.Current);
            _scenarioContext = scenarioContext;
        }


        [Given("An item name of (.*)")]
        public void GivenAnItemNameOf(string itemName) 
        {
            (int,string) results = _inventoryPageObject.FindItem(itemName);
            _inventoryPageObject.StoreItemIndex(results.Item1);
            _inventoryPageObject.StoreItemName(results.Item2);

            _scenarioContext["itemName"] = results.Item2;
            Console.WriteLine($"the scenario's steps title is: {_scenarioContext.ScenarioInfo.Title}");
            Console.WriteLine($"the scenario's step type is: {_scenarioContext.CurrentScenarioBlock}");
            Console.WriteLine($"the scenario's execution status is: {_scenarioContext.ScenarioExecutionStatus}");
        }

        [When("the price is below (.*)")]
        public void WhenPriceIsBelow(int price)
        {
            string itemPrice = _inventoryPageObject.FindPrice();
            _inventoryPageObject.CheckPrice(itemPrice,price);
        }

        [Then("Add to cart")]
        public void ThenAddToCart()
        {
            IWebElement addToCartButton = _inventoryPageObject.GetCartButton(_inventoryPageObject.ItemIndex, _inventoryPageObject.ItemName);
            addToCartButton.Click();

            Console.WriteLine("The item added was " + _scenarioContext["itemName"]);
        }
    }
}
