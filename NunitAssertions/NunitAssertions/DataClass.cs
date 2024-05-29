using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitAssertions
{
    public class DataClass
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(12, 3);//.Returns(4);
                yield return new TestCaseData(12, 4);//.Returns(3);
                yield return new TestCaseData(12, 6);//.Returns(2);
            }
        }

        public static IEnumerable FixtureData
        {
            get
            {
                yield return new TestFixtureData("Hello", "World");
                yield return new TestFixtureData("Hello", "Brave", "World");
            }
        }


        public static IEnumerable<string> ValueSourceTestData()
        {
            yield return "Hello";
            yield return "Brave";
            yield return "World";
        }
    }
}
