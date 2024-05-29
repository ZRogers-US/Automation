using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestingFluent
{

    [TestClass]
    public class Numbers
    {
        enum statusEnum
        {
            RUNNING,
            STOPPED,
            STARTING
        }

        [TestMethod]
        public void IntergerTest()
        {
            int value = 15;
            int[] arrayOfNumbers = { 5, 6, 7, 8 };
            using (new AssertionScope())
            {
                value.Should().BeGreaterThanOrEqualTo(14)
                    .And.BeGreaterThan(16)
                    .And.BeLessThanOrEqualTo(14)
                    .And.BeLessThan(14)
                    .And.BeNegative()
                    .And.Be(3)
                    .And.NotBe(15)
                    .And.BeInRange(0, 3)
                    .And.NotBeInRange(0, 20)
                    .And.Match((x) => x % 2 == 1)
                    .And.BeOneOf(arrayOfNumbers);
            }
        }

        [TestMethod]
        public void FloatTest()
        {
            float value = 5.2343223F;
            using (new AssertionScope())
            {
                value.Should().BeApproximately(5, 0.03F);
                value.Should().NotBeApproximately(5, 1F);
            }
        }


        [TestMethod]
        public void DateTimeTest()
        {
            DateTime date = DateTime.Now;
            DateTime dateTwo = new DateTime(2024, 04, 25);
            DateTimeOffset dateWithOffset = 1.March(2078).At(10, 55).AsUtc().WithOffset(2.Hours());

            using (new AssertionScope())
            {
                date.Should().Be(15.April(2024))
                    .And.Be(26.April(2024).At(09, 21))
                    .And.BeAfter(new DateTime(2025))
                    .And.BeBefore(5.April(2023))
                    .And.BeOnOrAfter(22.March(2026))
                    .And.BeOnOrBefore(5.August(2012))
                    .And.BeIn(DateTimeKind.Utc)
                    .And.BeSameDateAs(dateWithOffset.UtcDateTime)
                    .And.HaveDay(24)
                    .And.HaveYear(2012)
                    .And.NotHaveYear(2024)
                    .And.BeLessThan(1.Days()).Before(dateTwo)
                    .And.BeWithin(1.Minutes()).After(dateTwo)
                    .And.BeMoreThan(1.Hours()).After(dateTwo)
                    .And.BeExactly(24.Hours()).Before(dateTwo);


                dateWithOffset.Should().Be(5.March(2024).At(5, 58).WithOffset(1.Hours()))
                    .And.BeExactly(2.February(2019).At(15, 16).WithOffset(2.Hours()))
                    .And.HaveOffset(TimeSpan.FromHours(1));
            }

        }

        [TestMethod]
        public void IEnumatorTest()
        {
            IEnumerable<int> collection = new[] { 1, 12, 34, 5, 6, 8 };

            using (new AssertionScope())
            {
                collection.Should().Equal(1, 12, 34, 5, 8, 6)
                    .And.BeEquivalentTo(new[] { 1, 34, 5, 6, 8 })
                    .And.ContainItemsAssignableTo<string>()
                    .And.ContainInOrder(new[] { 12, 5 });
            }
        }

        [TestMethod]
        public void DictionaryTest()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
            Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
            Dictionary<string, WorkingClass> dictionary4 = new Dictionary<string, WorkingClass>();
            dictionary.Add("Fred", 25);
            dictionary.Add("Harry", 2);
            dictionary.Add("Ron", 22);
            dictionary2.Add("Sue", 55);
            dictionary2.Add("Polly", 99);
            dictionary3 = dictionary;
            WorkingClass wrkingClass = new WorkingClass();
            dictionary4.Add("workingClass", wrkingClass);

            KeyValuePair<string, int> pair1 = new KeyValuePair<string, int>("Harry", 2);
            KeyValuePair<string, int> pair2 = new KeyValuePair<string, int>("Sue", 55);
            KeyValuePair<string, int> pair3 = new KeyValuePair<string, int>("Sue", 51);

            using (new AssertionScope())
            {
                dictionary.Should().BeNull()
                               .And.BeEmpty()
                               .And.NotEqual(dictionary2)
                               .And.Equal(dictionary3)
                               .And.ContainValue(5)
                               .And.ContainKey("Sue")
                               .And.ContainValues(5, 21)
                               .And.ContainKeys("Sue", "Polly")
                               .And.NotContainValue(22)
                               .And.NotContainKey("Fred")
                               .And.HaveCount(2)
                               .And.HaveSameCount(dictionary2)
                               .And.NotHaveSameCount(dictionary3)
                               .And.NotHaveSameCount(dictionary3.Keys)
                               .And.HaveSameCount(dictionary2.Keys)
                               .And.Contain(pair2)
                               .And.NotContain(pair1)
                               .And.Contain(pair2, pair3);
                dictionary4.Should().ContainValue(wrkingClass).Which.Name.Should().NotBeEquivalentTo("George");
            }
        }

        [TestMethod]
        public void GUIDTest()
        {
            Guid user1GUID = Guid.NewGuid();
            Guid user2GUID = Guid.NewGuid();
            Guid user3GUID = user1GUID;

            using (new AssertionScope())
            {
                user1GUID.Should().Be(user2GUID)
                              .And.NotBe(user3GUID)
                              .And.BeEmpty()
                              .And.Be("00000000-0000-0000-0000-000000000001");
            }
        }

        [TestMethod]
        public void EnumTest()
        {
            statusEnum test = statusEnum.RUNNING;
            statusEnum test2 = statusEnum.STARTING;

            using (new AssertionScope())
            {
                test.Should().Be(statusEnum.STARTING)
                         .And.NotBe(statusEnum.RUNNING)
                         .And.BeOneOf(statusEnum.STOPPED, statusEnum.STARTING)
                         .And.NotBe(0)
                         .And.HaveValue(0)
                         .And.HaveSameNameAs(test2)
                         .And.HaveSameValueAs(test2);
            }
        }

        [TestMethod]
        public void DataSetTest()
        {
            DataSet firstDataSet = new DataSet("first");
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("FirstName", typeof(string));
            dataTable.Columns.Add("Age");
            dataTable.Rows.Add("Zak", 0);
            dataTable.Rows.Add("Bob", 4);
            firstDataSet.Tables.Add(dataTable);
            DataSet secondDataSet = new DataSet("second");
            DataTable secondDataTable = new DataTable();
            secondDataTable.Columns.Add("FirstName", typeof(string));
            secondDataTable.Columns.Add("Age");
            secondDataTable.Rows.Add("Zak", 0);
            secondDataTable.Rows.Add("Bob", 4);
            secondDataTable.Rows.Add("Frodo", 35);
            secondDataSet.Tables.Add(secondDataTable);

            using (new AssertionScope())
            {
                firstDataSet.Should().BeNull()
                                 .And.BeEquivalentTo(secondDataSet);
                firstDataSet.Tables[0].Should().HaveColumns("LastName")
                                           .And.HaveRowCount(4);
            }

        }

        [TestMethod]
        public void ExceptionsTest()
        {
            Func<int> func = () =>
            {
                int[] exampleArray = { 1, 2, 3 };
                return exampleArray[2];
            };
            func.Should().Throw<IndexOutOfRangeException>();
        }

        [TestMethod]
        public void ObjectGraphTest()
        {
            WorkingClass firstClass = new WorkingClass();
            firstClass.Name = "Fred";
            firstClass.Age = 6;

            SecondWorkingClass secondClass = new SecondWorkingClass();
            secondClass.Name = "George";
            secondClass.Age = 6;

            using( new AssertionScope())
            {
                secondClass.Should().BeEquivalentTo(firstClass);
                firstClass.Name = "George";
                secondClass.Should().NotBeEquivalentTo(secondClass);
            }
        }

        [TestMethod]
        public void EventInvokedTest()
        {
            SecondWorkingClass secondClass = new SecondWorkingClass();
            secondClass.AgeUpdatedEvent += AgeUpdated;
            using ( new AssertionScope())
            {
                using (var monitoredClass = secondClass.Monitor())
                {
                    monitoredClass.Should().Raise("AgeUpdatedEvent");
                    secondClass.Age = 8;
                    monitoredClass.Should().NotRaise("AgeUpdatedEvent");
                };
            }
                
        }

        public void AgeUpdated()
        {
            Console.WriteLine("hello this is a test");
        }

        [TestMethod]
        public void TypeOfTest()
        {
            WorkingClass wrkingClass = new WorkingClass();
            using ( new AssertionScope())
            {
                typeof(WorkingClass).Should().NotBeStatic();
                typeof(WorkingClass).Should().NotBeAbstract();
                typeof(WorkingClass).Should().NotBeSealed();
                typeof(WorkingClass).Should().BeSealed();
                typeof(WorkingClass).Should().BeStatic();
                typeof(WorkingClass).Should().BeAbstract();
                typeof(WorkingClass).Methods().ThatArePublicOrInternal.ThatReturnVoid.Should().BeVirtual()
                                                                                          .And.BeAsync();
                typeof(WorkingClass).Methods().ThatDoNotReturn<String>().Should().BeVirtual();
                typeof(WorkingClass).Properties().Should().BeWritable();
                typeof(WorkingClass).Methods().ReturnTypes().Should().BeInNamespace("TestingFluent");
            }
        }

        [TestMethod]
        public void AssemblyTest()
        {
            using ( new AssertionScope())
            {
                //assembly.Should().Reference(anotherAssembly);
                //assembly.Should().NotReference(anotherAssembly);
                //assembly.Should().HavePublicKey("e0851575614491c6d25018fadb75");
                //assembly.Should().BeUnsigned();
            }
        }

        [TestMethod]
        public void XMLTest()
        {
            XDocument xDocument = new XDocument();
            XElement xElement = new XElement("hello");
            XAttribute xAttribute = new XAttribute("hello", "59");
            
            using( new AssertionScope())
            {
                xDocument.Should().HaveRoot("configuration")
                              .And.HaveElement("Settings")
                              .And.HaveElement("Settings", Exactly.Once())
                              .And.HaveElement("Settings", AtLeast.Twice());
                xElement.Should().HaveValue("45")
                             .And.HaveAttribute("age","45")
                             .And.HaveElement("address")
                             //.And.HaveInnerText("some inner text")
                             .And.HaveElement("Settings", Exactly.Once())
                             .And.HaveElement("Settings", AtLeast.Twice());

                xDocument.Should().BeEquivalentTo(XDocument.Parse("<configuration><item>value</item></configuration>"));
                xElement.Should().BeEquivalentTo(XElement.Parse("<item>value</item>"));

                xDocument.Should().HaveElement("child").Which.Should().BeOfType<XElement>().And.HaveAttribute("attr", "2");

            }
        }

        [TestMethod]
        public async Task ExecutionTimeTest()
        {
                WorkingClass wrkingClass = new WorkingClass();
            Action sleepAction = () => Thread.Sleep(1);
            using (new AssertionScope())
            {
                wrkingClass.ExecutionTimeOf(c => c.ASlowMethod()).Should().BeLessThanOrEqualTo(500.Microseconds());

                sleepAction.ExecutionTime().Should().BeCloseTo(15.Seconds(), 1.Seconds());
                sleepAction.ExecutionTime().Should().BeLessThan(10.Minutes());
                sleepAction.ExecutionTime().Should().BeGreaterThan(10.Hours());
                sleepAction.ExecutionTime().Should().BeGreaterThanOrEqualTo(5.Minutes());

                Func<Task> someAsyncMethod = () => wrkingClass.TaskMethod();
                await someAsyncMethod.Should().CompleteWithinAsync(1.Milliseconds());
                await someAsyncMethod.Should().NotCompleteWithinAsync(100.Milliseconds());
                await someAsyncMethod.Should().ThrowWithinAsync<InvalidOperationException>(1.Microseconds());
            }
        }

        [TestMethod]
        public void HttpResponsesTest()
        {
            var sucessfullResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var redirectResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Moved);
            var clientErrorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            var serverErrorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            var anotherResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Moved);

            using (new AssertionScope())
            {
                sucessfullResponse.Should().BeRedirection();
                redirectResponse.Should().BeSuccessful();
                serverErrorResponse.Should().HaveClientError();
                serverErrorResponse.Should().HaveError();
                clientErrorResponse.Should().HaveServerError();
                clientErrorResponse.Should().HaveError();

                anotherResponse.Should().NotHaveStatusCode(System.Net.HttpStatusCode.Moved);
                anotherResponse.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            }
        }
    }
}