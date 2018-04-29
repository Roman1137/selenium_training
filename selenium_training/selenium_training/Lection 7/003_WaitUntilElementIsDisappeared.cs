using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_7
{
    class WaitUntilElementIsDisappeared
    {
        private IWebDriver driver;
        private IWebElement element;
        private WebDriverWait wait;

        //Проверка на ИСЧЕЗНОВЕНИЕ элемента это не то же самое, проверка на ОТСУТСТВИЕ элемента
        //ОТСУТСТВИЕ - это когда у нас есть локатор, но мы по этому локатору ничего не можем найти
        //ИСЧЕЗНОВЕНИЕ - это когда элемент есть, потом он пропадает и на его место появлятеся точно такой же 
        //элемент или очень похожий.
        //Речь идет об именно том элементе, который ранее был найден по локатору.
        //Сначала надо найти элемент, а потом ждать его исчезновения.

        //Если браузер пытается обратиться к элементу использую идентафикатор, который вернул метод FindElement
        //и возникает StaleElementRefferanceException - именно по этому признаку можно определить, что элемент исчез.

        public void Method()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //Здесь не используется никаких неявных ожиданий т.к мы ничего не ищем!
            wait.Until(ExpectedConditions.StalenessOf(element));
        }
        //Хороший способ использовать StalenessOf - пагинация.
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
        public void Test()
        {
            this.driver.Navigate().GoToUrl("http://pagination.js.org/");
            var paginationFormElement = this.driver.FindElement(By.CssSelector("#demo1"));
            var rows = paginationFormElement.FindElements(By.CssSelector(".data-container ul li"));
            var text1 = rows[0].Text;
            var pagination = paginationFormElement.FindElements(By.CssSelector(".paginationjs-pages ul li"));
            pagination[2].Click();
            wait.Until(ExpectedConditions.StalenessOf(rows[0]));
            rows =paginationFormElement.FindElements(By.CssSelector(".data-container ul li"));
            var text2 = rows[0].Text;
            Assert.IsTrue(!text1.Equals(text2));
        }

        //ИТОГ: если нам нужно подождать чтобы страница обновилась(или ее часть), то нужно найти элемент
        //который должен исчезнуть(или поменятся на другой) и использовать метод StalenessOf, передав параметром этот элемент.
        //Ещё проще: всегда и спользовать когда страница обновляется и на месте одного повляется другой, но который имеет такой же локатор.
    }
}
