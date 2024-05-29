using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//[assembly: NonTestAssembly]

namespace NunitAssertions
{
    // [SetUpFixture] - marks the class as containing the one-time setup or teardown methods for all test fixtures in the same namespace,
    //                 class is required to be public and have a default constructor
    //[SingleThreaded] - to be used on a testfixture to indicate the the onetime setup, teardown method and all child tests must run on the same thread,
    //                   parallelscope will be ignored

    // [FixtureLifeCycle(LifeCycle.InstancePerTestCase)] - will create a new instance of the testfixture for each test
    // [FixtureLifeCycle(LifeCycle.SingleInstance)]     , allowing tests to run without shareing the same varibles usefull for parael testcases
    //                                                  requires onetimesetup and onetimeteardown to be static

    [TestFixture]
    [SetCulture("en-US")] // should set the thread culture, only test methods/fixtures with the correct culture will run
    [SetUICulture("en-US")] // should set the thread UI culture
    internal class SetupTeardown
    {
        private int setupInt = 0;

        //runs once before all tests
        [OneTimeSetUp]
        public void Init ()
        {
            setupInt = 1;
        }


        //runs before each test
        [SetUp]
        public void Setup ()
        {
            setupInt++;
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); - attempting to set the culture another way as not working
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        [Test]
        [Culture("en-US")] // specifies in what culture the test should run
        public void firstTest()
        {
            Console.WriteLine(setupInt);
            Assert.That(setupInt, Is.EqualTo(2));
        }

        [Test]
        public void secondTest()
        {
            Console.WriteLine(setupInt);
            Assert.That(setupInt, Is.EqualTo(3));
        }

        // run after each test
        [TearDown]
        public void TearDown()
        {
            setupInt -= 1;
            Console.WriteLine(setupInt);
        }

        // run after all tests have completed
        [OneTimeTearDown]
        public void Exit()
        {
            setupInt= 0;
            Console.WriteLine(setupInt);
        }
    }
}
