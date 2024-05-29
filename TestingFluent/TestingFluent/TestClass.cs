using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;
using FluentAssertions.Execution;

namespace TestingFluent
{
    [TestClass]
    public class TestClass
    {
        WorkingClass _workingClass = new WorkingClass();
        string _fullName = "ZaK Benjamin Rogers";

        /// <summary>
        /// This method will fail on Z being capitialised
        /// </summary>
        [TestMethod]
        public void FirstNameCheckStartAndEndLetters()
        {
            string firstName = _workingClass.GetFirstName(_fullName);

            firstName.Should()
                .StartWith("z")
                .And
                .EndWith("k");
        }

        /// <summary>
        /// This method will fail on Z and K being capitialised using an assertionScope
        /// </summary>
        [TestMethod]
        public void FirstNameCheckStartAndEndLettersAssertionScope()
        {
            string firstName = _workingClass.GetFirstName(_fullName);

            using (new AssertionScope())
            {
                firstName.Should()
                    .StartWith("z")
                    .And
                    .EndWith("k")
                    .And
                    .BeLowerCased();
            }
        }

        /// <summary>
        /// Test to ensure the full name is split in two, firstname and lastname
        /// </summary>
        [TestMethod]
        public void FullNameSplitCheck()
        {
            string [] name = _workingClass.SplitFullName(_fullName);
            name.Should().HaveCount(2);


            string result = $"First name is: {name[0]}, last name is: {name[1]}";
            Console.WriteLine(result);
        }

        [TestMethod]
        public void CheckNull()
        {
            string nullCheck = "";

            using (new AssertionScope())
            {
                nullCheck.Should().BeNull("Didnt i set to null?");
                nullCheck.Should().BeEmpty();
                nullCheck = _workingClass.SetStringToNull(nullCheck);
                nullCheck.Should().NotBeNull();
            }

        }

        [TestMethod]
        public void CompareObjects()
        {
            TestObject obj1 = new TestObject(_fullName, 17, 1.73);
            TestObject obj2 = new TestObject(_fullName, 17, 1.73);
            TestObject obj3 = obj1;

            using (new AssertionScope())
            {
                obj1.Should().NotBeOfType<TestObject>();
                obj1.Should().BeOfType<WorkingClass>();
                obj1.Should().NotBeSameAs(obj2);
                obj1.Should().BeSameAs(obj3);
            }
        }

        [TestMethod]
        public void IsInArrayOfNumberAbove10()
        {
            int[] numbers = {10, 20, 30, 40, 50};

            using(new AssertionScope())
            {
                using (new AssertionScope()) ;
                numbers.Should().OnlyContain((number) => number > 10)
                    .And.HaveCount(4, "I though we only added 4");
            }
        }

        [TestMethod]
        public void BoolisTrue()
        {
            bool result = _workingClass.GenerateBool();

            using(new AssertionScope())
            result.Should().BeTrue()
                .And.NotBe(false);
        }

    }
}
