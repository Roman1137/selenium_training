using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_4
{
    [TestFixture]
    public class HomeTask
    {
        public IWebDriver driver;
        public WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
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
        public void VerifyEveryDugHasSticker()
        {
            GoToMainPage();
            var allDucks = duckElements;
            foreach (var duck in allDucks)
            {
                var countOfStickers = duck.FindElements(By.CssSelector(stickerLocator));
                countOfStickers.Count.Should().Be(1);
            }
        }

        [Test]
        public void VerifyAllAdminDepartments()
        {
            Login();
            for (int i = 0; i < departmentsElements.Count; i++)
            {
                departmentsElements.ToList()[i].Click();
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector(titleLocator)));
                if (SubDepartmentElements(i).Count != 0)
                {
                    for (int j = 0; j < SubDepartmentElements(i).Count; j++)
                    {
                        SubDepartmentElements(i).ToList()[j].Click();
                        wait.Until(ExpectedConditions.ElementExists(By.CssSelector(titleLocator)));
                    }
                }
            }
        }


        public void Login()
        {
            const string login = "admin", password = "admin";
            driver.Url = "http://localhost/litecart/admin";
            driver.FindElement(By.CssSelector("[name='username']")).SendKeys(login);
            driver.FindElement(By.CssSelector("[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("[name='login']")).Click();
            wait.Until(ExpectedConditions.ElementExists((By.CssSelector("[title='Logout']"))));
        }

        public void GoToMainPage()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#box-most-popular"), "Most Popular"));
        }

        //locators for MainPage
        public string duckLocator => ".product";
        public string stickerLocator => ".sticker";
        //elements for MainPage
        public IReadOnlyCollection<IWebElement> duckElements => driver.FindElements(By.CssSelector(duckLocator));

        //Locators for Admin page
        public string departmentLocator => "#app-";
        public string subDepartmentLocator => "[id^=doc]";
        public string titleLocator => "h1[style]";
        //elements for Admin page
        public IReadOnlyCollection<IWebElement> departmentsElements => driver.FindElements(By.CssSelector(departmentLocator));
       // public IWebElement title => driver.FindElement(By.CssSelector(titleLocator));
        public IReadOnlyCollection<IWebElement> SubDepartmentElements(int index)
        {
            return departmentsElements.ToList()[index].FindElements(By.CssSelector(subDepartmentLocator));
        }
    }
}
