using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitAssertions
{
    [TestFixtureSource(typeof(DataClass), nameof(DataClass.FixtureData))]
    public class TestFixtureDataTest
    {
        string _message = string.Empty;

        public TestFixtureDataTest(string a, string b)
        {
            _message = a + " " + b;
        }
        public TestFixtureDataTest(string a, string b, string c)
        {
            _message = a + " " + b + " " + c;
        }

        [Test]
        public void WelcomeMessageTest()
        {
            Assert.That(_message, Is.EqualTo("Hello World"));
        }
    }
}
