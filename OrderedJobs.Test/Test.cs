using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderedJobs.Test
{
    public class Test
    {
        private IOrderedJobs target;

        [SetUp]
        public void Setup()
        {
            target = new JobOrganizer();
        }

        [TestCase('a', ExpectedResult = "a")]
        [TestCase('b', ExpectedResult = "b")]
        [TestCase('x', ExpectedResult = "x")]
        public string TestParseChar(char value)
        {
            target.Register(value);

            return target.Sort();
        }

        [TestCase('a', 'a', ExpectedResult = "a")]
        [TestCase('b', 'b', ExpectedResult = "b")]
        [TestCase('a', 'b', ExpectedResult = "ba")]
        [TestCase('b', 'a', ExpectedResult = "ab")]
        public string TestParseChar(char dependant, char independant)
        {
            target.Register(dependant, independant);

            return target.Sort();
        }

        [TestCase('a', 'b', 'c', ExpectedResult = "abc")]
        [TestCase('a', 'c', 'b', ExpectedResult = "acb")]
        [TestCase('b', 'a', 'c', ExpectedResult = "bac")]
        [TestCase('b', 'c', 'a', ExpectedResult = "bca")]
        [TestCase('c', 'a', 'b', ExpectedResult = "cab")]
        [TestCase('c', 'b', 'a', ExpectedResult = "cba")]
        public string TestParseCharMin(char x, char y, char z)
        {
            target.Register(y, x);
            target.Register(z, y);

            return target.Sort();
        }

        [TestCase('a', 'b', 'c', ExpectedResult = "abc")]
        [TestCase('a', 'c', 'b', ExpectedResult = "acb")]
        [TestCase('b', 'a', 'c', ExpectedResult = "bac")]
        [TestCase('b', 'c', 'a', ExpectedResult = "bca")]
        [TestCase('c', 'a', 'b', ExpectedResult = "cab")]
        [TestCase('c', 'b', 'a', ExpectedResult = "cba")]
        public string TestParseCharMax(char x, char y, char z)
        {
            target.Register(x);
            target.Register(y, x);
            target.Register(y);
            target.Register(z, x);
            target.Register(z, y);
            target.Register(z);

            return target.Sort();
        }


        [Test]
        public void CircularTest()
        {
            target.Register('a');
            target.Register('b', 'a');
            target.Register('c', 'b');
            target.Register('c', 'a');

            var result = target.Sort();
        }
    }
}
