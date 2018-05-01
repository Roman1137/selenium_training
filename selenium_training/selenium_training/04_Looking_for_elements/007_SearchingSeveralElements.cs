using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_3
{
    class SearchingSeveralElements
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

        public void FirstTest()
        {
            //Пример: чтение таблицы. Мы НЕ знаем сколько строк в этой таблице
            var table = driver.FindElement(By.XPath("//*[@id='users']"));
            var rows = table.FindElements(By.XPath("//tr"));

            foreach (var row in rows)
            {
                var name = row.FindElement(By.XPath("./tr[1]")).Text; //   ./ потому, что ищем отсносительно другого элемента
                var email = row.FindElement(By.XPath("./tr[2]")).Text; //   ./ потому, что ищем отсносительно другого элемента
            }
        }
        public void FirstTest1()
        {
            //Иногда бывает, что удобнее сделать так:
            //Пример: чтение таблицы. Мы НЕ знаем сколько строк в этой таблице
            var table = driver.FindElement(By.XPath("//*[@id='users']"));
            var rows = table.FindElements(By.XPath("//tr"));

            foreach (var row in rows)
            {
                var cells = row.FindElements(By.XPath("./td"));//   ./ потому, что ищем отсносительно другого элемента
                var name = cells[0].Text;
                var email = cells[1].Text;
            }
        }

        //Плохой пример метода IsElementPresent
        public bool IsElementPresentBad(IWebDriver driver, By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }

        //Хороший пример метода IsElementPresent
        public bool IsElementPresentGood(IWebDriver driver, By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }
    }
}
