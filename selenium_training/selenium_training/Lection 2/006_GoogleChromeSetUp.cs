using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_2
{
    [TestFixture]
    public class GoogleChromeSetUp
    {
        private IWebDriver driver;
        private WebDriverWait wait;


        //http://seleniumtestings.com/selenium-c-chromedriver-chromeoptions/
        //https://sites.google.com/a/chromium.org/chromedriver/capabilities

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            // use this in order to run browser with extension
            options.AddExtension(@"C:\Speedtest-by-Ookla_v1.0.8.crx");
            // this is useful for specify explicitly the way to Chrome.exe file(to run browser, not driver)
            options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            options.AddArgument("start-maximized");
            //to find more info write chrome://version/ in url line
            options.AddArgument(@"--user-data-dir = C:\Users\Дракон\AppData\Local\Google\Chrome\User Data\Profile 1");
            //options.AddArguments("--profile-directory = Profile 1");

            //Proxy proxy = new Proxy();
            //proxy.ProxyAutoConfigUrl = "myhttpproxy:3337";
            //options.AddAdditionalCapability("proxy", proxy);
            driver = new ChromeDriver(options);
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
