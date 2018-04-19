using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_3
{
    //Fail fast - падай как можно быстрее.
    //Не нужно перехватывать исключения, которые связанны с элементами. Пусть они будут,
    //так мы сможем узнать, что используем невалидный локатор.

    //Если мы запускаем тесты удаленно с помощью сервера Selenium, то НА КАЖДОЕ ИСКЛЮЧЕНИЕ(перехваченное или нет)
    //нам будет слаться скриншот, который весит прилично. Нам нужно этого избежать. Так что случше использовать FindElements
    class ElementNotFound
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
        public bool IsElementPresentGood(IWebDriver driver, By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }
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

        public bool IsElementPresentBadGood(IWebDriver driver, By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (InvalidSelectorException ex)
            {
                throw ex;
            }
            catch (NoSuchElementException ex1)
            {
                return false;
            }
        }
        //[Test]
        public void FirstTest()
        {
            driver.Url = "https://www.google.com.ua";
            Assert.IsFalse(IsElementPresentGood(driver, By.XPath("lst-ib["))); //OpenQA.Selenium.InvalidSelectorException
            //это из-за невалидного локатора
            //это классно т.к тест падает сразу и сразу вы знаете причину падения теста.
            //чем быстрее он упадет, тем лучше.
        }
        //[Test]
        public void FirstTest1()
        {
            driver.Url = "https://www.google.com.ua";
            Assert.IsFalse(IsElementPresentBad(driver, By.XPath("lst-ib["))); //такой тест пройдет!
            //он прийдет потому, что мы ЛОВИМ исключение
            //исключение InvalidSelectorException является подклассом NoSuchElementException, по этому оно и перехватывается.
        }
        //[Test]
        public void FirstTest2()
        {
            driver.Url = "https://www.google.com.ua";
            Assert.IsFalse(IsElementPresentBadGood(driver, By.XPath("lst-ib["))); //такой упадет
            //в методе IsElementPresentBadGood=> если возникает InvalidSelectorException, то он пробрасывается дальше и тест падает
            //если возникает NoSuchElementException, то оно перехватывается.
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
