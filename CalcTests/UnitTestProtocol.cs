using System;
using NUnit.Framework;
using System.Net;
using System.IO;
using Moq;
using System.Text;

namespace CalcTests
{
    [TestFixture]
    public class UnitTestProtocol
    {
        HttpServer.HttpServer server = null;

        [SetUp]
        public void Init()
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://test/");
            server = new HttpServer.HttpServer("http://test/");
        }

        [TestCase("1", "2", "%2B", 3)]
        [TestCase("3", "4", "-", -1)]
        [TestCase("6", "7", "*", 42)]
        [TestCase("99", "9", "/", 11)]
        public void HttpRequestCalc(string a, string b, string op, int res)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:2345/?" + "a=" + a + "&b=" + b + "&op=" + op);
            httpWebRequest.Method = WebRequestMethods.Http.Get;

            string responseText;

            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    responseText = streamReader.ReadToEnd().ToLowerInvariant();
                }
            }

            Assert.AreEqual(res.ToString(), responseText);

        }
    }
}
