using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_3
{
    [TestFixture]
    class SearchForElementJavaScript
    {
        private IWebDriver driver;
        private WebDriverWait wait;


        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        //[Test]
        public void FirstTest()
        {
            driver.Url = "https://www.google.com.ua";
            var field= driver.ExecuteJavaScript<IWebElement>("return document.getElementById('lst-ib')");
            field.SendKeys("Test"+ Keys.Enter);

            var searchResults = driver.ExecuteJavaScript<IReadOnlyCollection<IWebElement>>("return document.querySelectorAll('.bkWMgd')");
        }

        [Test]
        public void FirstTest1()
        {
            //На странице этого приложение есть библиотека JQuery, которая позволяет писать '$$' вместо document.querySelectorAll
            driver.Url = "https://selenium2.ru/";
            var fields = driver.ExecuteJavaScript<IReadOnlyCollection<IWebElement>>("return $$('ul,menu li')");

            //а так же библиотека JQuery использует расширенную версию CSS селектров и может выполнять поиск по тексту элемента,
            //чего нельзя сделать через стандартный CSS, используемый в Selenium. Тут ищем слово Selenium
            var fieldsByText = driver.ExecuteJavaScript<IReadOnlyCollection<IWebElement>>("return $$('a:contains(Selenium)')");
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
