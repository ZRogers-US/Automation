using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;

namespace NunitAssertions
{
    [TestFixture]
    [Author("Zak Rogers", "zrogers@usspeaking.com")]
    [Platform("Windows10")] //sets the test fixtures platform to windows 10, will only run tests for windows 10 platform
    [SetCulture("en-US")]
    public class Tests
    {

        private int intFromSetup = 0;
        private static int[] staticValues = { 10, 9, 8 };
        private int orderInt = 0;
        private int retryCount = 0;

        [SetUp]
        public void Setup()
        {
            intFromSetup = 1; // set before each test
        }

        [Test]
        [Author("Zak Rogers", "zrogers@usspeaking.com")]
        public void NunitAssertionTest()
        {
            int[] array = [ 1, 2, 3 ];
            int[] array2 = [ 1, 6, 7 ];
            string[] array3 = [ "a", "bb", "ccc" ];
            string? nullString = null;
            string helloString = "";
            double zero = 0;

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Exactly(3).LessThan(1));
                Assert.That(array, Has.Exactly(3).LessThan(1).Or.GreaterThan(4));
                Assert.That(array, Has.Exactly(3).LessThanOrEqualTo(0.5F));
                Assert.That(array, Has.Exactly(3).GreaterThan(3), "the Array should have 3 numbers that are greater than 3");
                Assert.That(array, Has.Exactly(3).GreaterThanOrEqualTo(3), "the Array should have 3 numbers that are greater than or equal too 3");
                Assert.That(array, Has.Property("Length").GreaterThan(4));
                Assert.That(array, Has.Length.GreaterThan(4));
                Assert.That(array, Has.Property("Length"));

                Assert.That(array, Has.Some.GreaterThan(4));
                Assert.That(array, Has.Some.LessThan(1));
                Assert.That(array3, Has.Some.Length.EqualTo(4));

                ClassicAssert.True(array[1] == 4);
                ClassicAssert.False(array[1] == 2);
                Assert.That(array[1] == 4, Is.True);

                ClassicAssert.Null(array);
                ClassicAssert.NotNull(nullString);
                Assert.That(array, Is.Null);
                Assert.That(nullString, Is.Not.Null);

                ClassicAssert.Zero(array[1]);
                ClassicAssert.NotZero(0);
                Assert.That(array[1], Is.Zero, "should it be zero?");

                ClassicAssert.IsNaN(zero);
                Assert.That(zero, Is.NaN);

                ClassicAssert.IsEmpty(nullString);
                Assert.That(nullString, Is.Empty);
                ClassicAssert.IsNotEmpty(helloString);
                Assert.That(helloString, Is.Not.Empty);

                ClassicAssert.AreEqual(5, 4);
                Assert.That(5 == 4, Is.True);

                ClassicAssert.AreEqual(5, 5F);
                ClassicAssert.AreNotEqual(5, 5);
                Assert.That(5 == 5F, Is.True);
                Assert.That(5, Is.EqualTo(4));
                Assert.That(array, Is.EqualTo(array2));

                Assert.That(array, Is.All.Negative); // All- applies constraint to each item in an IEnumerable
                Assert.That(array, Is.All.InstanceOf<string>());

                Assert.That(array[0], Is.GreaterThan(0).And.LessThan(1)); // And - used to combine two constraints and only succeed if both succeed

                Assert.That(5, Is.AnyOf(array2)); // AnyOf - checks if value provided is in the array provided

                Assert.That("Hello", Is.AssignableFrom(typeof(int))); // AssignableFrom - checks if the provided value can be assigned from the specified type
                Assert.That(5, Is.Not.AssignableFrom(typeof(int)));

                Assert.That("Hello", Is.AssignableTo(typeof(int)));
                Assert.That(5, Is.Not.AssignableTo(typeof(int)));
                RandomClass rndClass = new RandomClass();

                Assert.That(rndClass, Has.Attribute<TestFixtureAttribute>().Property("Description").EqualTo("My description")); //pretty sure this should be failing
                Assert.That(rndClass, Has.Attribute(typeof(TestFixtureAttribute)).Property("Description").EqualTo("My description"));

                Assert.That(rndClass, Has.Attribute<TestFixtureAttribute>()); // likewise believe this should be failing


            });
        }

        [Test, Description("this test demostrates the assert.fail method")]
        [Category("TestPractise")]
        [Culture("en-US")]
        public void FailAssertion()
        {
            TestContext.WriteLine("hello this is a context test");
            Assert.Fail("Fails assertion");
        }

        [Test(Description = "ensureing intFromSetup is assign correctly and asserts a pass if it is")]
        public void PassAssertion()
        {
            if (intFromSetup == 1) Assert.Pass("Passes assertion");
        }

        [Test]
        public void IgnoreAssertion()
        {
            Assert.Ignore("ignore this fail");
            Assert.Fail("it fails");
        }

        [Test(ExpectedResult = "if the fullname is an empty string Assert the test as inconclusive otherwise return the fullName")]
        public string InconclusiveTest()
        {
            string fullName = "";
            if (fullName == "")
            {
                Assert.Inconclusive("This test was inconclusive due to receiving incorrect data");
                return String.Empty;
            }
            else
            {
                return fullName;
            }
        }

        [Test]
        [TestCase(5)]
        [TestCase(0)]
        [TestCase(-5)]
        public void numberDividedByItsSelfIs1(int number)
        {
            Assume.That(number, Is.Not.Zero, "This number should not be 0");
            int result = number / number;

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        [TestCase(7)] //runs multiple tests with each parameter
        [TestCase(6)]
        public void WarningTest(int number)
        {
            Warn.If(number / 2 == 3); //Test will continue to run after a warning, the test can still pass or fail. Warning messages will only display if the test ends up failing.
            Warn.Unless(number % 2 == 0);
            Assert.Warn("warning test");
            if (number == 7) Assert.Fail("Test ended up failing");
            if (number == 6) Assert.Pass();
        }

        [Test]
        [TestCaseSource(typeof(DataClass), nameof(DataClass.TestCases))] // gets the testcase data from a separate class and will run multiple timesfor each lot of data 
        public void TestCaseSourceTest(int a, int b)
        {
            int result = a / b;
            Assert.That(result, Is.EqualTo(2));
            //return result;
        }

        [Test, Combinatorial] // Combinatorial - specifies that testcases should be generated for each possible combinations
                              // of the individual data items provided,this is the default behaviour so is optional.
        public void RandomValuesTest([Random(-5, 9, 2)] int rndValue, [Values(3, 4, 5)] int value)
        {
            Console.WriteLine(value); // runs test for each value
            Console.WriteLine(rndValue); // set to a random value, will run each value test with two different random numbers
        }

        [Test]
        public void SetValuesTest([ValueSource("staticValues")] int value)
        {
            Console.WriteLine(value); //runs the tet with each value in the static int[] "staticValues"
        }

        [Test, Order(1)]
        public void RunFirst()
        {
            orderInt = 1;
            Console.WriteLine(orderInt);
        }

        [Test, Order(2)]
        public void RunSecond()
        {
            orderInt = 2;
            Console.WriteLine(orderInt);
        }

        [Test, Order(2)] //there is no order between tests set with the same order
        public void ExtraRunSecond()
        {
            orderInt++;
            Console.WriteLine(orderInt);
        }

        [Test]
        public void RunAfterOrderedTests()
        {
            orderInt++;
            Console.WriteLine(orderInt);
        }

        [Test, Retry(9)]
        [TestCase("Pass")]
        [TestCase("Fail")]
        public void RetryTest(string passOrFail)
        {
            retryCount++;
            if (retryCount == 8 && passOrFail == "Pass")
            {
                Assert.Pass($"Assert Passed after {retryCount} retries");
            }
            Assert.Fail($"Assert Failed after {retryCount} retries");
        }

        [Test, Explicit("Testings out the explicit Attribute")]
        public void ThisShouldntRun()
        {
            Assert.Fail("This test shouldnt run unless specificly selected");
        }

        [Test, Parallelizable]
        public void RuningInParallelOne()
        {
            Assert.Pass("first test to run in parallel");
        }

        [Test, Parallelizable]
        public void RuningInParallelTwo()
        {
            Assert.Pass("second test to run in parallel");
        }

        //deprecated
        [Test, Timeout(100)]
        public void TimeoutTest()
        {
            Thread.Sleep(1000);
        }

        //Cancleation token can be passed into a http request
        [Test, CancelAfter(100)]
        public void TimeoutTestTwo(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(100000);
                Assert.Pass("didnt timeout");
            }
            Assert.Fail("test timed out");
        }

        [Test, MaxTime(5)]
        public void MaxTimeTest()
        {
            Thread.Sleep(1000);
            Assert.Pass("completed before max time");
        }

        //TearDownruns after each test
        [TearDown]
        public void TearDown()
        {
            intFromSetup = 0;
            Console.WriteLine(intFromSetup);
        }

        [Test]
        [Ignore("Has not been fully implemented", Until = "2024-05-03")]
        public void NotFullyImplemented()
        {

        }

        [Test]
        [DefaultFloatingPointTolerance(0.04F)]
        public void OneDividedByThree()
        {
            var result = 1F / 3F;
            TestContext.WriteLine(result);
            Assert.That(result, Is.EqualTo(0.3F));
        }

        [Test, Pairwise] // Pairwise - unsures the test is only executed enough times that each possible pair is covered,
                         // this will not coverall possibilites like combinatorial and can be used to reduce the number of total combinational tests
        public void PairwiseTest(
            [Values("a", "b", "c")] string a,
            [Values("+", "-")] string b,
            [Values("x", "y")] string c)
        {
            TestContext.WriteLine("{0} {1} {2}", a, b, c);
        }

        [Test]
        [Platform("Windows11")]
        public void Window11Test()
        {
            TestContext.WriteLine("test platform set to windows 11");
        }

        [Test]
        [Platform(Exclude = "Windows10")]
        public void ExcludeWindow10Test()
        {
            TestContext.WriteLine("test platform set to exclude windows 10");
        }

        [Test, Property("Location", "Bedford")]
        public void PropertyTest()
        {
            TestContext.WriteLine("Tests can be filtered via the Property Attribute through the command-line");
        }

        [Test]
        public void RangeTest([Range(0.1F, 1.0F, 0.2F)] float number) // Random takes from, to, step. With step being optional for ints and defaulting to 1
                                                                      // works with int, long, float and double
        {
            TestContext.WriteLine(number);
            Assert.That(number, Is.InRange(20, 100));
        }

        [Test, RequiresThread]
        public void RequiresThread()
        {
            TestContext.WriteLine("a seperate thread will be created for this test to run on, Can also be set on the testfixure or assembly");
        }

        [Test, RequiresThread(ApartmentState.STA)]
        public void ApartmentStateThreadRequired()
        {
            TestContext.WriteLine("a separate STA thread will be created for this test");
        }

        [Test,Sequential]
        public void SequentialTest(
            [Values(1,2,3)] int x,
            [Values("a","b")] string y)
        {
            TestContext.WriteLine($"{x} {y}");
            TestContext.WriteLine("Sequential attribute will run multiple tests " +
                                  "pairing the variables together eg: 1 and a, 2 and b, 3 and null as a third string is not provided");
        }

        [Test, TestOf(typeof(DataClass))]
        public void TestOfTest()
        {
            TestContext.WriteLine("The TestOf attribute is used for grouping/filtering test as well as enhancing readability, can also use nameof(class)");
        }

        [Test]
        public void ValueSourceTest(
            [ValueSource(typeof(DataClass), nameof(DataClass.ValueSourceTestData))] string word) // gets the parameter values from the value source,
                                                                    // which must be an IEnumerable ortpye that implements it
                                                                    // if sourceTypeis specified it represents the class that provides
                                                                    // the data if not the current class is used.
        {
            TestContext.WriteLine(word);
        }

        [Test]
        public void CollectionContainsConstraintTest()
        {
            int[] intArray = [2, 3, 4];
            string[] stringArray = ["hello", "world"];

            Assert.Multiple(() =>
            {
                Assert.That(intArray, Has.Member(1));
                Assert.That(intArray, Has.No.Member(3));
                Assert.That(intArray, Does.Contain(1));
                Assert.That(intArray, Contains.Item(1));

                Assert.That(stringArray, Has.Member("b"));
            });
        }

        [Test]
        public void CollectionEquivalentConstraintTest()
        {
            int[] intArray = [2, 3, 4];
            string[] stringArray = ["hello", "world"];

            Assert.Multiple(() =>
            {
                Assert.That(intArray, Is.EquivalentTo(stringArray));
                Assert.That(new string[] { "world", "Hop", "hello" }, Is.EquivalentTo(stringArray)); // case sensitive
                Assert.That(new string[] { "world", "Hop", "Hello" }, Is.EquivalentTo(stringArray).IgnoreCase); 
                //Assert.That(new string[] { "world", "Hop", "Hello" }, Is.EquivalentTo(stringArray).IgnoreWhiteSpace); // implemented in version 4.2
                Assert.That(new string[] { "world", "hello" }, Is.EquivalentTo(stringArray)); // passes - order doesnt matter
                Assert.That(new string[] { "world", "hello" }, Is.Not.EquivalentTo(stringArray));
            });
        }

        [Test]
        public void CollectionOrderedTest()
        {
            int[] intArray = [2, 4, 3];
            string[] stringArray = ["a", "bbb", "cc"];

            Assert.Multiple(() =>
            {
                Assert.That(intArray, Is.Ordered.Ascending);
                Assert.That(intArray, Is.Ordered.Descending);
                Assert.That(stringArray, Is.Ordered.Descending);

                Assert.That(stringArray, Is.Ordered.Ascending.By("Length")); //Length being a propertyName
                Assert.That(stringArray, Is.Ordered.By("Length"));


                Assert.That(stringArray, Is.Ordered.By("A").Then.By("B")); //properties can be chained
            });
        }

        [Test]
        public void CollectionSubsetTest()
        {
            int[] intArray = new int[] { 1, 3 };
            Assert.That(intArray, Is.SubsetOf(new string[] { "a", "b", "c" }));
            Assert.That(intArray, Is.SubsetOf(new int[] { 1, 2, 3 })); //passes
        }

        [Test]
        public void CollectionSupersetTest()
        {
            int[] intArray = new int[] { 1, 3 };
            Assert.That(intArray, Is.SupersetOf(new string[] { "a", "b", "c" }));
            Assert.That(intArray, Is.SupersetOf(new int[] { 1, 3 })); //passes
        }

        [Test]
        public void DelayConstraintTest()
        {
            int[] intArray = new int[] { 1, 3 };
            Assert.Multiple(() =>
            {
                Assert.That(intArray, Is.SupersetOf(new string[] { "a", "b", "c" }).After(4)); //Defaults to MilliSeconds
                Assert.That(intArray, Is.SupersetOf(new string[] { "a", "b", "c" }).After(4).MilliSeconds);
                Assert.That(intArray, Is.SupersetOf(new string[] { "a", "b", "c" }).After(4).Seconds);
                Assert.That(intArray, Is.SupersetOf(new string[] { "a", "b", "c" }).After(1).Minutes);
                Assert.That(intArray, Is.SupersetOf(new string[] { "a", "b", "c" }).After(1).Minutes.PollEvery(30).Seconds);
                Assert.That(intArray, Is.SupersetOf(new string[] { "a", "b", "c" }).After(100,30)); // 100 milliSeconds with 30 milliSecond Polling
            });
        }

        [Test]
        public void DictionaryTest()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string> { { "Hello", "World" }, { "Under", "Sea" } };
            DictionaryContainsKeyValuePairConstraint dictionaryContainsKeyValuePair = new DictionaryContainsKeyValuePairConstraint("Key", "Value");

            Assert.Multiple(() =>
            {
                Assert.That(dictionary, Contains.Key("Hello ")); // contains whitespace
                Assert.That(dictionary, Contains.Key("hello")); // case sensitive
                Assert.That(dictionary, Contains.Key("Hello ").IgnoreCase); // contains whitespace
                //Assert.That(dictionary, Contains.Key("Hello ").IgnoreWhiteSpace); // implemented in version 4.2
                
                Assert.That(dictionary, Does.ContainKey("Hello ")); // contains whitespace
                Assert.That(dictionary, Does.Not.ContainKey("Hello")); 

                Assert.That(dictionary, Does.ContainKey("Hello").WithValue("Sea"));


                Assert.That(dictionary, Contains.Value("Sea ")); // contains whitespace
                Assert.That(dictionary, Contains.Value("world")); // case sensitive
                Assert.That(dictionary, Contains.Value("world ").IgnoreCase); // contains whitespace
                                                                            

                Assert.That(dictionary, Does.ContainValue("Sea ")); // contains whitespace
                Assert.That(dictionary, Does.Not.ContainValue("Sea"));


                Assert.That(dictionary, dictionaryContainsKeyValuePair);
                Assert.That(dictionary, new DictionaryContainsKeyValuePairConstraint("Sea", "Under"));
                Assert.That(dictionary, new DictionaryContainsKeyValuePairConstraint("Sea", "Under").IgnoreCase);
                //Assert.That(dictionary, new DictionaryContainsKeyValuePairConstraint("Sea", "Under").IgnoreWhiteSpace); // implemented in version 4.2
            });
        }

        [Test]
        public void EmptyCollection()
        {
            int[] array1 = { 1, 2 };
            int[] array2 = new int[0];

            Assert.Multiple(() =>
            {
                Assert.That(array1, Is.Empty);
                Assert.That(array2, Is.Not.Empty);
            });
        }


        [Test]
        public void EmptyTest()
        {
            string emptyString = string.Empty;
            Dictionary<string, int> emptyDictionary = new Dictionary<string, int>();
            List<int> emptyList = new List<int>();

            Assert.Multiple(() =>
            {
                Assert.That(emptyString, Is.Not.Empty);
                Assert.That(emptyDictionary, Is.Not.Empty);
                Assert.That(emptyList, Is.Not.Empty);
            });
        }

        [Test]
        public void EmptyDirectory()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("D:\\GIT"); 

            Assert.Multiple(() => 
            {
                Assert.That(directoryInfo, Is.Empty);
                Assert.That(new DirectoryInfo("D:\\GIT\\temp"), Is.Not.Empty);
            });
        }

        [Test]
        public void EndsWithTest()
        {
            string endsWithString = "Hello World!";

            Assert.Multiple(() =>
            {
                Assert.That(endsWithString, Does.EndWith("?"));
                Assert.That(endsWithString, Does.EndWith("world!"));
                Assert.That(endsWithString, Does.EndWith("WORLD").IgnoreCase);
            });
        }

        [Test]
        public void EqualTest()
        {
            string stringOne = "This is string One";
            string stringTwo = "This is string Two";
            int intOne = 1;
            float floatOne = 1.0F;
            float floatOneAndHalf = 1.5F;

            Assert.Multiple(() =>
            {
                Assert.That(stringOne, Is.EqualTo(stringTwo));
                Assert.That(stringOne, Is.EqualTo(stringTwo).IgnoreCase);
                //Assert.That(stringOne, Is.EqualTo(stringTwo).IgnoreWhiteSpace); // implemtented in version 4.2

                Assert.That(intOne + intOne == 3);
                Assert.That(intOne, Is.EqualTo(floatOneAndHalf));
                Assert.That(intOne, Is.EqualTo(floatOne)); // Different types can be compared successfully if their values are equal.
                Assert.That(intOne, Is.EqualTo(floatOneAndHalf).Within(0.05f));
                Assert.That(2 + 2, Is.Not.EqualTo(4));


                Assert.That(double.PositiveInfinity, Is.EqualTo(double.NegativeInfinity));
                Assert.That(double.NegativeInfinity, Is.EqualTo(double.PositiveInfinity));
                Assert.That(double.NaN, Is.EqualTo(0.58d));

                //Within - .Ulps
                         //.Percent
                         //.Days
                         //.Hours
                         //.Minutes
                         //.Seconds
                         //.Milliseconds
                         //.Ticks
            });
        }

        [Test]
        public void TypeOfTest()
        {
            RandomClass rndClass = new RandomClass();
            Assert.Multiple(() =>
            {
                Assert.That("Hello", Is.TypeOf(typeof(int)));
                Assert.That("Hello", Is.Not.TypeOf(typeof(string)));

                Assert.That(rndClass, Is.InstanceOf(typeof(DataClass)));
                Assert.That("Hello", Is.InstanceOf(typeof(int))); 

            });
        }

        [Test]
        public void IsTrueFalseTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(2+2==4, Is.False);
                Assert.That(2+2==5, Is.True);
            });
        }

        [Test]
        public void FileDirectoryExistsTest()
        {
            FileInfo fileInfo = new FileInfo("D:\\GIT\\DataClass.cs");
            DirectoryInfo directoryInfo = new DirectoryInfo("D:\\GIT\\Hello");
            Assert.Multiple(() =>
            {
                Assert.That(fileInfo, Does.Exist);
                Assert.That(directoryInfo, Does.Exist);
            });
        }

        [Test]
        public void NoItemsTest()
        {
            int[] intArray = new int[] { 1, 2, 3 };
            string[] stringArray = new string[] { "a", "b", "c" };
            Assert.Multiple(() =>
            {
                Assert.That(intArray, Has.None.Null);
                Assert.That(stringArray, Has.None.EqualTo("b"));
                Assert.That(intArray, Has.None.LessThan(2));
            });
        }

        [Test]
        public void PropertyConstraintTest()
        {
            RandomClass rndClass = new RandomClass();
            string[] stringArray = new string[] { "a", "b", "c" };
            Assert.Multiple(() =>
            {
                Assert.That(rndClass, Has.Property("Name"));
                Assert.That(rndClass, Has.Property("Age"));
                Assert.That(rndClass, Has.Property("TestProperty"));
                Assert.That(stringArray, Has.Property("Length"));
            });
        }

        [Test]
        public void RegexTest()
        {
            string sentence = "Welcome to a brave new world";
            Assert.Multiple(() =>
            {
                Assert.That(sentence, Does.Match("Welcome.*A.*world"));
                Assert.That(sentence, Does.Match("Welcome.*A.*rabbit").IgnoreCase);
                Assert.That(sentence, Does.Not.Match("Welcome.*a.*world"));
            });
        }

        [Test]
        public void ReusableTest()
        {
            Constraint myConstraint = Is.GreaterThan(5);
            ReusableConstraint myReusableConstraint = Is.GreaterThan(5);
            int firstInt = 6;
            int secondInt = 8;
            Assert.Multiple(() =>
            {
                //Assert.That(firstInt, myConstraint); // would pass
                //Assert.That(secondInt, myConstraint); // would fail, cant reuse a constraint

                Assert.That(firstInt, myReusableConstraint); // will pass
                Assert.That(secondInt, myReusableConstraint); // will pass, can reuse a resuableContraint
            });
        }

        [Test]
        public void SameAsTest()
        {
            RandomClass rndClass1 = new RandomClass();
            RandomClass rndClass2 = rndClass1;
            RandomClass rndClass3 = new RandomClass();

            Assert.Multiple(() =>
            {
                Assert.That(rndClass1, Is.SameAs(rndClass3));
                Assert.That(rndClass1, Is.Not.SameAs(rndClass2));
            });
        }

        [Test]
        public void PathTest()
        {
            Assert.Multiple(() =>
            {
                //Assert.That(@"D:\GIT", Is.SamePath("D:\\GIT")); // passes
                //Assert.That("D:\\GIT", Is.SamePath("D:\\GiT").IgnoreCase); //passes
                //Assert.That("/folder1/./junk/../folder2", Is.SamePath("/folder1/folder2")); // passes
                Assert.That("D:\\GIT", Is.SamePath("D:\\git").RespectCase);
                Assert.That("D:\\GIT", Is.Not.SamePath("D:\\git"));

                Assert.That("D:\\GIT", Is.SamePathOrUnder("D:\\git\\temp"));
                // Assert.That("D:\\git\\temp", Is.SamePathOrUnder("D:\\GIT")); // passes


                Assert.That("D:\\GIT", Is.SubPathOf("D:\\git\\temp"));
                Assert.That("D:\\GIT", Is.SubPathOf("D:\\git\\temp").IgnoreCase);
                Assert.That("D:\\git\\temp", Is.SubPathOf("D:\\GIT").RespectCase);
                Assert.That("D:\\git\\temp", Is.Not.SubPathOf("D:\\git"));

            });
        }

        [Test]
        public void StartWithTest()
        {
            string message = "Hello brave new world";


            Assert.Multiple(() =>
            {
                Assert.That(message, Does.StartWith("h"));
                Assert.That(message, Does.StartWith("hell"));
                Assert.That(message, Does.Not.StartWith("hello").IgnoreCase);

            });
        }

        [Test]
        public void SubStringTest()
        {

            string message = "Hello brave new world";


            Assert.Multiple(() =>
            {
                Assert.That(message, Does.Contain("hello"));
                Assert.That(message, Does.Not.Contain("Hello"));
                Assert.That(message, Does.Contain("brve").IgnoreCase); 
                //Assert.That(message, Does.Contain("ell")); // passes
            });
        }

        [Test]
        public void ExceptionThrownTest()
        {

            Assert.Multiple(() =>
            {
                Assert.That(() => { throw new Exception(); }, Throws.Exception);
                Assert.That(() => { throw new TargetInvocationException(new Exception()); }, Throws.TargetInvocationException);
                Assert.That(() => { throw new ArgumentException(); }, Throws.ArgumentException);
                Assert.That(() => { throw new ArgumentNullException(); }, Throws.ArgumentNullException);
                Assert.That(() => { throw new InvalidOperationException(); }, Throws.InvalidOperationException);
                Assert.That(() => { TestContext.WriteLine("Hello World"); }, Throws.Nothing);
            });
        }

        [Test]
        public void ClassicExceptionThrownTest()
        {

            Assert.Multiple(() =>
            {
                Assert.Throws<Exception>(() => { throw new Exception(); });
                Assert.Throws<TargetInvocationException>(() => { throw new TargetInvocationException(new Exception()); });
                Assert.Throws<ArgumentException>(() => { throw new ArgumentException(); });
                Assert.Throws<ArgumentNullException>(() => { throw new ArgumentNullException(); });
                Assert.Throws<InvalidOperationException>(() => { throw new InvalidOperationException(); });
                Assert.DoesNotThrow(() => { TestContext.WriteLine("Hello World"); });
            });
        }

        [Test]
        public void ClassicExceptionThrownAsyncTest()
        {
            Assert.Multiple(() =>
            {
                Assert.ThrowsAsync<Exception>(async () => await AsyncMethodWithThrow());
                Assert.DoesNotThrowAsync(async () => await AsyncMethod());
            });
        }

        private async Task AsyncMethodWithThrow()
        {
            await Task.Delay(10);
            throw new Exception();
        }
        private async Task AsyncMethod()
        {
            await Task.Delay(10);
        }

        [Test]
        public void AssertCatchTest()
        {
            Assert.Multiple(() =>
            {
                Assert.Catch(typeof(Exception), () => { throw new Exception(); }); // will pass is the exception is the one specified
                Assert.Catch<Exception>(() => { throw new Exception(); }); // will pass is the exception is the one specified

                Assert.CatchAsync(typeof(Exception), () => AsyncMethodWithThrow()); // will pass is the exception is the one specified
                Assert.CatchAsync<Exception>(() => AsyncMethodWithThrow()); // will pass is the exception is the one specified
            });
        }

        [Test]
        public void UniqueItemsTest()
        {
            int[] array = [ 1, 2, 3 ];
            int[] array2 = [ 1, 2, 2,  3 ];
            List<string> stringList = ["Hello","hello", "World", "world"];
            List<string> stringList2 = ["Hello ", "Hello", " world"];

            Assert.Multiple(() =>
            {
                //Assert.That(array, Is.Unique); // passes
                Assert.That(array2, Is.Unique); 
                //Assert.That(stringList, Is.Unique);  // passes
                Assert.That(stringList, Is.Unique.IgnoreCase); 
                //Assert.That(stringList2, Is.Unique); //passes
                //Assert.That(stringList2, Is.Unique.IgnoreWhiteSpace); // not implemented until version 4.2
            });
        }

        [Test]
        public void WhiteSpaceTest()
        {
            string noSpace = "Hello";
            string space = " ";

            Assert.Multiple(() =>
            {
               // Assert.That(noSpace, Is.WhiteSpace); // not implemented untill version 4.2
               // Assert.That(space, Is.Not.WhiteSpace); // not implemented untill version 4.2
            });
        }
        
        [Test]
        public void XMLSerializableTest()
        {
            string xmlBody = "<book><title>The Wonderful Wizard of Oz</title><author>L. Frank Baum</author><year>1900</year><price>39.95</price></book>";
            
            Assert.Multiple(() =>
            {
                 Assert.That(xmlBody, Is.XmlSerializable); 
            });
        }

        [Test]
        public void AreSameTest()
        {
            RandomClass rndClass1 = new RandomClass();
            RandomClass rndClass2 = rndClass1;
            RandomClass rndClass3 = new RandomClass();
            Assert.Multiple(() =>
            {
                ClassicAssert.AreSame(rndClass1, rndClass3);
                ClassicAssert.AreSame(rndClass1, rndClass2); // passes
                ClassicAssert.AreNotSame(rndClass1, rndClass2); 

                ClassicAssert.IsInstanceOf(typeof(DataClass), rndClass3);
                ClassicAssert.IsInstanceOf<DataClass>(rndClass3);

                ClassicAssert.IsNotInstanceOf(typeof(RandomClass), rndClass3);
                ClassicAssert.IsNotInstanceOf<RandomClass>(rndClass3);
            });
        }

        [Test]
        public void ClassicContainsTest()
        {
            int[] intArray = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            Assert.Multiple(() =>
            {
                ClassicAssert.Contains(10, intArray);
            });
        }

        [Test]
        public void ClassicGreaterTest()
        {
            Assert.Multiple(() =>
            {
                ClassicAssert.Greater(5, 10);
                ClassicAssert.GreaterOrEqual(5, 10);

                ClassicAssert.Less(10, 5);
                ClassicAssert.LessOrEqual(10, 5);
            });
        }

        [Test]
        public void ClassicPositiveNegativeTest()
        {
            Assert.Multiple(() =>
            {
                ClassicAssert.Positive(-10);
                ClassicAssert.Negative(10);
            });
        }

        [Test]
        public void ClassicAssignableTest()
        {
            Assert.Multiple(() =>
            {
                ClassicAssert.IsAssignableFrom(typeof(string), 1);
                ClassicAssert.IsAssignableFrom(typeof(int), 1.5F);
                ClassicAssert.IsAssignableFrom(typeof(float), 1F); // passes

                ClassicAssert.IsNotAssignableFrom(typeof(float), 1F); 
            });
        }

        [Test]
        public void StringAssertTest()
        {
            string message = "Hello World";
            Assert.Multiple(() =>
            {
                StringAssert.Contains("trello", message);
                StringAssert.DoesNotContain("ello", message);
                StringAssert.StartsWith("ello", message);
                StringAssert.DoesNotStartWith("ello", message);
                StringAssert.EndsWith("hello", message);
                StringAssert.DoesNotEndWith("World", message);
                StringAssert.AreEqualIgnoringCase("World", message);
                StringAssert.AreNotEqualIgnoringCase("World", message); 
                StringAssert.IsMatch("Hello.* A.* world", message); 
                StringAssert.DoesNotMatch(".* World", message); 
            });
        }

        [Test]
        public void CollectionAssertTest()
        {
            List<int> intList = new List<int>{ 1, 3, 2 , 3 };
            List<int> intList2 = new List<int>{ 1, 2, 3 };
            List<int> intList3 = new List<int>{ 3, 2, 1 };
            List<int> intList4 = new List<int>();
            Assert.Multiple(() =>
            {
                CollectionAssert.AllItemsAreInstancesOfType(intList, typeof(string));
                CollectionAssert.AllItemsAreNotNull(intList); //passes
                CollectionAssert.AllItemsAreUnique(intList);
                CollectionAssert.AreEqual(intList, intList2);
                CollectionAssert.AreEquivalent(intList, intList2);
                CollectionAssert.AreEquivalent(intList2, intList3); //passes

                CollectionAssert.AreNotEqual(intList, intList2);
                CollectionAssert.AreNotEquivalent(intList2, intList3);

                CollectionAssert.IsSubsetOf(intList3, intList); //passes
                CollectionAssert.IsNotSubsetOf(intList3, intList); 

                CollectionAssert.IsEmpty(intList3); 
                CollectionAssert.IsNotEmpty(intList4); 

                CollectionAssert.IsOrdered(intList); 
            });
        }

        [Test]
        public void FileAssertTest()
        {
            FileInfo fileInfo = new FileInfo("D:\\GIT\\temp\\temp.txt");
            string filePath = "D:\\GIT\\temp\\temp.txt";
            //also works with streams

            Assert.Multiple(() =>
            {
                FileAssert.AreEqual(filePath, filePath);               
                FileAssert.AreEqual(fileInfo, fileInfo);

                FileAssert.Exists(fileInfo);
                FileAssert.Exists(filePath);

                FileAssert.DoesNotExist(fileInfo);
                FileAssert.DoesNotExist(filePath);
            });
        }

        [Test]
        public void DirectoryAssertTest()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("D:\\GIT\\temp");
            string directoryPath = "D:\\GIT\\temp";
            //also works with streams

            Assert.Multiple(() =>
            {
                DirectoryAssert.AreEqual(directoryInfo, directoryInfo);
                DirectoryAssert.AreNotEqual(directoryInfo, directoryInfo);

                DirectoryAssert.Exists(directoryInfo);
                DirectoryAssert.Exists(directoryPath);

                DirectoryAssert.DoesNotExist(directoryInfo);
                DirectoryAssert.DoesNotExist(directoryPath);
            });
        }

        [Test]
        public void RandomizerTest()
        {
            Randomizer rnd = new Randomizer();

            int randomInt = rnd.Next();
            int randomInt2 = rnd.Next(9); // max of 9
            int randomInt3 = rnd.Next(9,15); // min of 9 maxof15
            Console.WriteLine(randomInt);
            Console.WriteLine(randomInt2);
            Console.WriteLine(randomInt3);

            uint randomUInt = rnd.NextUInt();
            Console.WriteLine(randomUInt);

            long randomLong = rnd.NextLong();
            Console.WriteLine(randomLong);

            ulong randomULong = rnd.NextULong();
            Console.WriteLine(randomULong);

            short randomShort = rnd.NextShort();
            Console.WriteLine(randomShort);

            ushort randomUShort = rnd.NextUShort();
            Console.WriteLine(randomUShort);

            sbyte randomSByte = rnd.NextSByte();
            Console.WriteLine(randomSByte);

            byte randomByte = rnd.NextByte();
            Console.WriteLine(randomByte);

            double randomDouble = rnd.NextDouble();
            Console.WriteLine(randomDouble);

            float randomFloat = rnd.NextFloat();
            Console.WriteLine(randomFloat);

            decimal randomDecimal = rnd.NextDecimal();
            Console.WriteLine(randomDecimal);

            bool rndBool = rnd.NextBool();
            Console.WriteLine(rndBool);

            var randomEnum = rnd.NextEnum(typeof(Severity));
            Console.WriteLine(randomEnum);

            string randomString = rnd.GetString();
            string randomString2 = rnd.GetString(5); // length of 5
            string randomString3 = rnd.GetString(5, "heo"); // length of 5 usings supplied characters
            Console.WriteLine(randomString);
            Console.WriteLine(randomString2);
            Console.WriteLine(randomString3);

            Guid randomGuid = rnd.NextGuid();
            Console.WriteLine(randomGuid);
        }

    }

enum Severity
{
    NONE = 0,
    LOW =1,
    MEDIUM =2,
    HIGH =3
}

}