using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using S9.Utility;

namespace S9.Utility.Tests
{
    [TestClass]
    public class HTMLPageExtractorTests
    {
        [TestMethod]
        public void Test_ExtractContent_Success()
        {
            HTMLPageExtractor pageExplorer = new HTMLPageExtractor("", "window._sharedData = ", ";</script>");
            pageExplorer.InitWithHTML("window._sharedData = HELLO;</script>");
            string content = pageExplorer.GetContent();
            Assert.AreEqual("HELLO", content);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Test_ExtractContent_StartTagNotFound()
        {
            HTMLPageExtractor pageExplorer = new HTMLPageExtractor("", "window._sharedData = ", ";</script>");
            pageExplorer.InitWithHTML("window.HELLO;</script>");
            string content = pageExplorer.GetContent();
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Test_ExtractContent_EndTagNotFound()
        {
            HTMLPageExtractor pageExplorer = new HTMLPageExtractor("", "window._sharedData = ", ";</script>");
            pageExplorer.InitWithHTML("window._sharedData = HELLO;</");
            string content = pageExplorer.GetContent();
        }

        [TestMethod]
        public void Test_ExtractContent_GetPage_Success()
        {
            const string URL = "https://www.instagram.com/explore/tags/socket9";
            HTMLPageExtractor pageExplorer = new HTMLPageExtractor(URL, "window._sharedData = ", ";</script>");
            string content = pageExplorer.GetContent();
            Assert.IsTrue(content.Length > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void Test_ExtractContent_GetPage_Fail()
        {
            const string URL = "https://www.instagram12345.com/explore/tags/socket9";
            HTMLPageExtractor pageExplorer = new HTMLPageExtractor(URL, "window._sharedData = ", ";</script>");
            string content = pageExplorer.GetContent();
        }
    }
}
