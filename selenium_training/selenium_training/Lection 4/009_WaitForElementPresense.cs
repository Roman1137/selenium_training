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
    class WaitForElementPresense
    {
        //Если элемент не найден:
        //1. Не та страница открыта
        //2. Неправильный локатор
        //3. Элемент находится внутри фрейма. (нужно сначала переключиться во фрейм, а потом выполнять поиск)
        //4. Нужно немного подождать.

        //Неявные ожидания
        //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //Если элемент не находится, то выбрасывается исключение NoSuchElementExeption (а в explicit - Timeout Exception)
        //Неявные ожидания нацелены на одну проверку - они ждут пока появится элемент
        //Со стороны селениума отправляется в браузер findelement и уже в браузере выполняется ожидание.Частота проверок 100мс.
        //Прийдется отключать неявные ожидания, при проверке того, что элемента нет.


        //Явные ожидания 
        //на стороне клиента(селениума) 1 раз в 500 мс посылается запрос(раньше и не нужно.При запуске
        //тестов на удаленной машине, пока прийдет скриншот, то и 500 мск и пройдет). если условие ожидания выполнилось, то оно завершается успешно
        //если нет, то выполняется ожидание 500 мс и опять тоже самое до тех пока пока елемент найдется или пока не закончится тайм аут.
        //Если элемент появится через 100 мс, то явные ожидания заметят этот черз 500 мс во время выполнения след попытки.

        private IWebDriver driver;
        private WebDriverWait wait;


        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }
        public bool IsElementPresentGood(IWebDriver driver, By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }

        //[Test]
        public void WithoutWaits()
        {
            //неявные
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //явные
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id("1234"), "444"));

            driver.Url = "https://www.google.com.ua";
            driver.FindElement(By.Name("q")).SendKeys("webdriver");
            var a = driver.FindElement(By.Name("btnG"));
            a.Click();
            Assert.IsTrue(IsElementPresentGood(driver, By.XPath("//*[contains(@class,'rc')]")));
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
