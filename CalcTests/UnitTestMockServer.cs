using System;
using NUnit.Framework;
using System.Net;
using System.IO;
using Moq;
using System.Text;

namespace CalcTests
{
    public interface IHttpWebRequestFactory
    {
        HttpWebRequest Create(string uri);
    }

    [TestFixture]
    public class UnitTestMockServer
    {
        HttpServer.HttpServer server = null;

        [SetUp]
        public void Init()
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://test/");
            server = new HttpServer.HttpServer("http://test/");
        }

        [TestCase("1", "2", "+", 3)]
        [TestCase("3", "4", "-", -1)]
        [TestCase("6", "7", "*", 42)]
        [TestCase("99", "9", "/", 11)]
        public void HttpMockRequestCalc(string a, string b, string op, int res)
        {
            //Assert.AreEqual(res, server.Calc(a, b, op));

            var expected = res.ToString();
            var expectedBytes = Encoding.UTF8.GetBytes(expected);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<HttpWebResponse>();
            response.Setup(c => c.GetResponseStream()).Returns(responseStream);

            var request = new Mock<HttpWebRequest>();
            request.Setup(c => c.GetResponse()).Returns(response.Object);

            var factory = new Mock<IHttpWebRequestFactory>();
            factory.Setup(c => c.Create(It.IsAny<string>()))
                .Returns(request.Object);

            // act
            var actualRequest = factory.Object.Create("http://localhost:2345/?" + "a=" + a + "&b=" + b + "&op=" + op);
            actualRequest.Method = WebRequestMethods.Http.Get;

            string actual;

            using (var httpWebResponse = (HttpWebResponse)actualRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    actual = streamReader.ReadToEnd();
                }
            }

            Assert.AreEqual(expected, actual);

            // assert
            //actual.Should().Be(expected);
        }
    }
}
