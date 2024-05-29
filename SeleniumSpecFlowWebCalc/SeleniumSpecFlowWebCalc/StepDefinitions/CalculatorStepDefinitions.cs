using SeleniumSpecFlowWebCalc.Drivers;
using SeleniumSpecFlowWebCalc.PageObject;

namespace SeleniumSpecFlowWebCalc.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly CalculatorPageObject _calculatorPageObject;

        public CalculatorStepDefinitions(BrowserDriver browserDriver)
        {
            _calculatorPageObject = new CalculatorPageObject(browserDriver.Current);
        }

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            _calculatorPageObject.EnterFirstNumber(number.ToString());
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            _calculatorPageObject.EnterSecondNumber(number.ToString());
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            _calculatorPageObject.ClickAddButton();
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int expectedResult)
        {
            var actualResult = _calculatorPageObject.WaitForNonEmptyResult();

            actualResult.Should().Be(expectedResult.ToString());
        }
    }
}
