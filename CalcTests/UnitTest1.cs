using System;
using NUnit.Framework;

namespace CalcTests
{
    [TestFixture]
    public class UnitTest1
    {
        HttpServer.HttpServer server = null;

        [SetUp]
        public void Init()
        {
            server = new HttpServer.HttpServer("http://test/");
        }

        [TestCase("1", "2", "+", 3)]
        [TestCase("3", "4", "-", -1)]
        [TestCase("6", "7", "*", 42)]
        [TestCase("99", "9", "/", 11)]
        public void CalcTests(string a, string b , string op, int res)
        {
            Assert.AreEqual(res, server.Calc(a, b, op));
        }
    }
}
