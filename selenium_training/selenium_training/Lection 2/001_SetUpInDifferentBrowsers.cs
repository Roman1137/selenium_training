using System;
using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace selenium_training
{
    [TestFixture]
    public class DiffBrowsers
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        //[SetUp]
        //public void SetUp()
        //{
        //    driver = new ChromeDriver();
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait= TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //}

        //[SetUp]
        //public void SetUp()
        //{
        //    driver = new InternetExplorerDriver();
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //}

        [SetUp]
        public void SetUp()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test()
        {
            const string text = "selenium", textBoxlocator = "lst-ib";
            driver.Url = "https://www.google.com.ua";
            driver.FindElement(By.Id(textBoxlocator)).SendKeys(text + Keys.Enter);
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id(textBoxlocator), text));
        }

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }
    }
}
