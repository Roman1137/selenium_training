using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_2
{
    [TestFixture()]
    class ComandLineOptions
    {
        private IWebDriver driver;
        private WebDriverWait wait;


        //Capabilities are not only browres settings, they are used for:
        // - selecting right browser
        // - set the driver
        // - set the browser
        //options.AddArguments("start-fullscreen"); "start-fullscreen" was taken from 
        //https://peter.sh/experiments/chromium-command-line-switches/

        //Chrome: http://peter.sh/experiments/chromium-command-line-switches/

        //Firefox: https://developer.mozilla.org/en-US/docs/Mozilla/Command_Line_Options

        //Internet Explorer: https://msdn.microsoft.com/ru-ru/library/hh826025(v=vs.85).aspx


        //Summary
        //When it is needed to set some settings in browser => 
        //first - look for CAPABILITIES
        //if there were no capabilities found you there commands. options.AddAdditionalCapability();



        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new ChromeOptions();
        //    options.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;

        //    options.AddArguments("start-fullscreen");

        //    driver = new ChromeDriver(options);
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));       
        //}

        //there is example of using --use-fake-ui-for-media-stream  command for Chrome in lection.

        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new InternetExplorerOptions();
        //    options.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;
        //    options.IgnoreZoomLevel = true;

        //    options.AddAdditionalCapability("-embedding", true);

        //    driver = new InternetExplorerDriver(options);
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //    Console.Out.Write(options.ToCapabilities());
        //}

        [SetUp]
        public void SetUp()
        {
            var options = new FirefoxOptions();
            options.AddArguments("-devtools");

            driver = new FirefoxDriver(options);
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
