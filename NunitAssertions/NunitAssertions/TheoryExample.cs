using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitAssertions
{
    [TestFixture]
    public class TheoryExample
    {
        [DatapointSource]
        private int[] numberArray = [-9,-2,-16];

        [Datapoint]
        private int num = -5;

        /// <summary>
        /// Theories will take Datapoints and DatapointSources as values for their parameters
        /// </summary>
        /// <param name="number">provided at test time from the datapoints</param>
        [Theory]
        public void TheoryTest(int number)
        {
            Assume.That(number > 0);

            double sqrt = Math.Sqrt(number);

            Assert.That(sqrt > 0);
            Assert.That(sqrt * sqrt, Is.EqualTo(number).Within(0.0001));
        }

        [Theory]
        public void SecondTheoryTest(int number)
        {
            Assume.That(number, Is.Positive);

            double result = number / number;
            Assert.That(result, Is.EqualTo(1));
        }
    }
}
