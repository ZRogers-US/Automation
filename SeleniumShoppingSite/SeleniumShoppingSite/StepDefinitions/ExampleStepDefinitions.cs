using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumShoppingSite.StepDefinitions
{
    [Binding]
    public class ExampleStepDefinitions
    {
        [Given(@"I am logged in as admin")]
        public void GivenIAmLoggedInAsAdmin()
        {
            Console.WriteLine("This background step is run for each scenario");
        }

        [Given("there are (.*) green bottles")]
        public void ThereAreGreenBottle(int number)
        {
            Console.WriteLine(number);
        }

        [When(@"(.*) green bottles fall")]
        public void WhenGreenBottlesFall(int number)
        {
            Console.WriteLine(number);
        }

        [Then(@"I should have (.*) green bottles left")]
        public void ThenIShouldHaveGreenBottlesLeft(int number)
        {
            Console.WriteLine(number);
        }

        [Given(@"the following users are registered")]
        public void GivenTheFollowingUsersAreRegistered(Table table)
        {
            Console.WriteLine("The users username is: " + table.Rows[0][0]);
            Console.WriteLine("The users email is: " + table.Rows[0][1]);
        }

        [Given(@"to post to blog")]
        public void GivenToPostToBlog(string multilineText)
        {
            Console.WriteLine(multilineText);
        }

    }
}
