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
        public string RegisterOneTest(char value)
        {
            target.Register(value);

            return target.Sort();
        }

        [TestCase('a', 'a', ExpectedResult = "a")]
        [TestCase('b', 'b', ExpectedResult = "b")]
        [TestCase('a', 'b', ExpectedResult = "ab")]
        [TestCase('b', 'a', ExpectedResult = "ba")]
        public string RegisterTwoTest(char x, char y)
        {
            target.Register(y, x);

            return target.Sort();
        }

        [TestCase('a', 'b', 'c', ExpectedResult = "abc")]
        [TestCase('a', 'c', 'b', ExpectedResult = "acb")]
        [TestCase('b', 'a', 'c', ExpectedResult = "bac")]
        [TestCase('b', 'c', 'a', ExpectedResult = "bca")]
        [TestCase('c', 'a', 'b', ExpectedResult = "cab")]
        [TestCase('c', 'b', 'a', ExpectedResult = "cba")]
        public string RegisterThreeMinimalTest(char x, char y, char z)
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
        public string RegisterThreeMaximalTest(char x, char y, char z)
        {
            target.Register(x);
            target.Register(y, x);
            target.Register(y);
            target.Register(z, x);
            target.Register(z, y);
            target.Register(z);

            return target.Sort();
        }

        [TestCase('a', 'b', 'c', ExpectedResult = "abc")]
        [TestCase('a', 'c', 'b', ExpectedResult = "abc")]
        [TestCase('b', 'a', 'c', ExpectedResult = "abc")]
        [TestCase('b', 'c', 'a', ExpectedResult = "abc")]
        [TestCase('c', 'a', 'b', ExpectedResult = "abc")]
        [TestCase('c', 'b', 'a', ExpectedResult = "abc")]
        public string RegisterUnorderedTest(char x, char y, char z)
        {
            target.Register(x);
            target.Register(y);
            target.Register(z);

            return target.Sort();
        }
    }
}
