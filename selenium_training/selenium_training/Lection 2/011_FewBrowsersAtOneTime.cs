using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_2
{
    [TestFixture()]
    public class FewBrowsersAtOneTime
    {
        //If you set few IE browsers, take into account that they will have COMMON set of cookies.
        //ie.force.CreateProcessApi - it can help but you'd better not use it.
        //Why is above? requireWindowFocus(IE)
        //IF Firefox is not above the OBLUR event will not occur in Firefox browser. Some developers can use this event, so that
        //the browser should  be in background mode.

        private IWebDriver driver;
        private IWebDriver driver2;
        private WebDriverWait wait;
        private WebDriverWait wait2;
        [SetUp]
        public void SetUp()
        {
            driver = new InternetExplorerDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver2 = new InternetExplorerDriver();
            driver2.Manage().Window.Maximize();
            driver2.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }


        [Test]
        public void Test()
        {
            const string login = "admin", password = "admin";
            driver.Url = "http://localhost/litecart/admin";
            driver.FindElement(By.CssSelector("[name='username']")).SendKeys(login);
            driver.FindElement(By.CssSelector("[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("[name='login']")).Click();
            wait.Until(ExpectedConditions.ElementExists((By.CssSelector("[title='Logout']"))));

            var s = driver.Manage().Cookies.AllCookies;
            var f = driver2.Manage().Cookies.AllCookies;

            driver2.Url = "http://localhost/litecart/admin";
            driver2.FindElement(By.CssSelector("[name='username']")).SendKeys(login);
            driver2.FindElement(By.CssSelector("[name='password']")).SendKeys(password);
            driver2.FindElement(By.CssSelector("[name='login']")).Click();
           // wait2.Until(ExpectedConditions.ElementExists((By.CssSelector("[title='Logout']"))));

            var a = driver.Manage().Cookies.AllCookies;
            var b = driver2.Manage().Cookies.AllCookies;

            driver.Manage().Cookies.DeleteAllCookies();

            var v = driver.Manage().Cookies.AllCookies;
            var n = driver2.Manage().Cookies.AllCookies;

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
