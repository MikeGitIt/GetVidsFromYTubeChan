using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;


namespace NUnitPhantomJSSeleniumTest
{
    [TestFixture]
    public class PhantomjsTests
    { 
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            _driver = new PhantomJSDriver();
        }

        [Test]
        public void should_be_able_to_search_google()
        {
            _driver.Navigate().GoToUrl("http://www.google.com");

            IWebElement element = _driver.FindElement(By.Name("q"));
            string stringToSearchFor = "BDDfy";
            element.SendKeys(stringToSearchFor);
            element.Submit();

            NUnit.Framework.Assert.That(_driver.Title, Is.StringContaining(stringToSearchFor));
            ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile("google.png", ImageFormat.Png);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
