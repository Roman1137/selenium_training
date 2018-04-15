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
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_2
{
    [TestFixture()]
    public class Cookies
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        //Why to change cookies?
        //to login without clicking buttons
        //to logout without clicking buttons
        //clearing the session => Chrome and Firefox always set up clean browser and it's not needed for them
        //but it doesn't relate to Internet Explorer. The best way is to delete cookies for him.
        
        //The most important rule:
        //The cookies are deleted only by the path you are at! for instance google.com => when i delete cookies all cookies for sites which
        //domens start from google will be deleted.
        //If it is needed to delete cookies from another site you should go to its url and then delete. Or just reload browser (Chrome and Firefox)

        //There is an opportunity to set the clean IE but you'd better not use it. ie.ensureCleanSession. it is very hard operation!

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            Console.Out.Write(options.ToCapabilities());
        }

        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new InternetExplorerOptions();

        //    driver = new InternetExplorerDriver(options);
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //    Console.Out.Write(options.ToCapabilities());
        //}

        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new FirefoxOptions();

        //    driver = new FirefoxDriver(options);
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //    var b = driver.Manage().Cookies.AllCookies;

        //}

        [Test]
        public void Test()
        {
            const string login = "admin", password = "admin";
            driver.Url = "http://localhost/litecart/admin";
            driver.FindElement(By.CssSelector("[name='username']")).SendKeys(login);
            driver.FindElement(By.CssSelector("[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("[name='login']")).Click();
            wait.Until(ExpectedConditions.ElementExists((By.CssSelector("[title='Logout']"))));
            var a = driver.Manage().Cookies.AllCookies;
            driver.Manage().Cookies.DeleteAllCookies();
            var c = driver.Manage().Cookies.AllCookies;
            driver.Navigate().Refresh();
            driver.Url = "http://localhost/litecart/admin";
            foreach (var cook in a)
            {
                driver.Manage().Cookies.AddCookie(cook);
            }
            driver.Url = "http://localhost/litecart/admin";
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
