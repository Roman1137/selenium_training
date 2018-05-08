using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._10_RecordKeeping
{
    [TestFixture]
    class HomeTask1
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }

        [Test]
        public void VerifyErrorsAfterClickingProductItem()
        {
            Login();
            GoToCatalogePage();
            var allProducts = this.ProductElements;
            allProducts.ForEach(pr =>
            {
                var newWidnowId = OpenTheProductPage(pr);
                VerifyNoErrorsArePresent(newWidnowId).Should()
                    .BeTrue("Errors should not occur after openning the product page");
                CloseTab();
            });
        }

        private void CloseTab()
        {
            var currentWindowId = driver.CurrentWindowHandle;
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.First(x => !x.Equals(currentWindowId)));
        }

        private bool VerifyNoErrorsArePresent(string newWidnowId)
        {
            var logsInMainPage = driver.Manage().Logs.GetLog(LogType.Browser);
            driver.SwitchTo().Window(newWidnowId);
            var logsOpenedPage = driver.Manage().Logs.GetLog(LogType.Browser);

            return logsInMainPage.Count == 0 && logsOpenedPage.Count == 0;
        }

        private string OpenTheProductPage(IWebElement pr)
        {
            driver.ExecuteJavaScript("arguments[0].setAttribute('target','_blank')", pr);
            var allWindows = driver.WindowHandles;
            pr.Click();

            return wait.Until(x => x.WindowHandles.Except(allWindows).First());
        }

        public void Login()
        {
            const string login = "admin", password = "admin";
            driver.Url = "http://localhost/litecart/admin";
            this.LoginNameElement.SendKeys(login);
            this.LoginPasswordElement.SendKeys(password);
            this.LoginButtonElement.Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(this.LogoutButtonLocator)));
        }

        public void GoToCatalogePage()
        {
            this.driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";
            wait.Until(d => d.FindElement(By.CssSelector(this.DataTableLocator)).Displayed);
        }

        //admin page
        public string LoginNameLocator => "[name='username']";
        public string LoginPasswordLocator => "[name='password']";
        public string LoginButtonLocator => "[name='login']";
        public string LogoutButtonLocator => "[title='Logout']";
        public IWebElement LoginNameElement => this.driver.FindElement(By.CssSelector(LoginNameLocator));
        public IWebElement LoginPasswordElement => this.driver.FindElement(By.CssSelector(LoginPasswordLocator));
        public IWebElement LoginButtonElement => this.driver.FindElement(By.CssSelector(LoginButtonLocator));

        //cataloge page
        public string DataTableLocator => ".dataTable";
        public string ProductLocator => "img[src]+a[href]";
        public IWebElement DataTableElement => this.driver.FindElement(By.CssSelector(DataTableLocator));
        public List<IWebElement> ProductElements => this.DataTableElement.FindElements(By.CssSelector(ProductLocator)).ToList();
    }
}
