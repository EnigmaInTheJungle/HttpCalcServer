using System;
using NUnit.Framework;
using System.Net;
using System.IO;
using Moq;
using System.Text;
using TestStack.White;
using System.Reflection;

namespace CalcTests
{
    [TestFixture]
    public class UnitTestProtocol
    {
        Application app = null;

        [OneTimeSetUp]
        public void Init()
        {
            var outputDir = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.FullName;
            var appPath = outputDir + @"\HttpCalcServer.exe";
            app = Application.Launch(appPath);
        }

        [OneTimeTearDown]
        public void CloseServer()
        {
            app.Close();
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
