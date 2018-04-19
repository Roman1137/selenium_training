using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_2
{
    [TestFixture]
    class FirefoxSetUp
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        //new way =>  driver = new FirefoxDriver();
        //old way var options = new FirefoxOptions();
        //options.UseLegacyImplementation = false;
        //driver = new FirefoxDriver(options);

        //old way using firefox 59.0.2 - nothing works
        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new FirefoxOptions();
        //    options.UseLegacyImplementation = true;

        //    driver = new FirefoxDriver(options);
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //    Console.Out.Write(options.ToCapabilities());
        //}

        //old way using firefox 45
        [SetUp]
        public void SetUp()
        {
            var options = new FirefoxOptions();
            options.UseLegacyImplementation = true;
            options.BrowserExecutableLocation = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe"; 

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            Console.Out.Write(options.ToCapabilities());
        }

        //new way using Nightly and specifying the path to the firefox.exe
        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new FirefoxOptions();
        //    options.BrowserExecutableLocation = @"C:\Program Files\Firefox Nightly\firefox.exe";

        //    driver = new FirefoxDriver(options);
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //    Console.Out.Write(options.ToCapabilities());
        //}

        //NEW WAY
        //[SetUp]
        //public void SetUp()
        //{
        //    driver = new FirefoxDriver();
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //}

        //NEW WAY more explicitly
        //[SetUp]
        //public void SetUp()
        //{
        //    FirefoxOptions options = new FirefoxOptions();
        //    options.UseLegacyImplementation = false;
        //    driver = new FirefoxDriver(options);
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //}

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
