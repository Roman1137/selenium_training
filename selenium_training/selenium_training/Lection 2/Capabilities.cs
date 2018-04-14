using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_2
{
    [TestFixture]
    public class Capabilities
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        //https://peter.sh/experiments/chromium-command-line-switches/
        //https://github.com/SeleniumHQ/selenium/wiki/DesiredCapabilities
        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new ChromeOptions();
        //    options.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;
        //
        //    driver = new ChromeDriver(options);
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //
        //    Console.Out.Write(options.ToCapabilities());
        //}

        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new InternetExplorerOptions();
        //    options.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;
        //    options.IgnoreZoomLevel = true;

        //    driver = new InternetExplorerDriver();
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //    Console.Out.Write(options.ToCapabilities());
        //}

        [SetUp]
        public void SetUp()
        {
            var options = new FirefoxOptions();
            options.AcceptInsecureCertificates = true;

            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            Console.Out.Write(options.ToCapabilities());
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
