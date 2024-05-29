using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitAssertions
{
    [TestFixture("Hello","World")]
    [TestFixture(1,2)]
    [TestFixture("Hello","Brave", "World")]
    [TestFixture]
    public class TestFixtures
    {
        private string constructorStringA = "";
        private string constructorStringB = "";
        private string constructorStringC = "";

        public TestFixtures(string a, string b)
        {
            constructorStringA = a + " " + b;
        }

        public TestFixtures(string a, string b, string c)
        {
            constructorStringB = a + " " + b + " " + c;
        }

        public TestFixtures(int a, int b)
        {
            constructorStringA = a + " " + b;
        }

        [Test]
        public void Test()
        {
            Assume.That(5, Is.Positive);
        }
    }
}
