using SeleniumSpecFlowWebCalc.Drivers;
using SeleniumSpecFlowWebCalc.PageObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSpecFlowWebCalc.Hooks
{
    public class CalculatorHooks
    {
        //hook to run before the scenarios to ensure the calculator is open and reset
        [BeforeScenario("Calculator")]
        public static void BeforeScenario(BrowserDriver browserDriver)
        {
            var calculatorPageObject = new CalculatorPageObject(browserDriver.Current);
            calculatorPageObject.EnsureCalculatorIsOpenAndReset();
        }
    }
}
