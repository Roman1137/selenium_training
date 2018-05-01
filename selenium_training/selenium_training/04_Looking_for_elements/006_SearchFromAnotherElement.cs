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
    class SearchFromAnotherElement
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
            //преймущество -> если что-то всрется, то легче отлаживать.
            //недостаток -> сложно отлаживать в браузере
            var form = driver.FindElement(By.ClassName("b-footer"));
            var input = form.FindElement(By.CssSelector(".l-content.m-db"));

            //Это же можно выполнить, построив сложный локатор, по которому будет найден нужный элемент
            //преймущество -> можем легко отлаживать в браузере
            //недостаток -> если что-то стрется, по попробуй пойми в чем причина.
            input = driver.FindElement(By.XPath("//*[@class='b-footer']//*[contains(@class,'l-content') and contains(@class,'m-db')]"));

            //Эти два способа РАБОТАЮТ НЕ ОДИНАКОВО:
            //способ поиска от элементов задает алгоритм и порядок выполнения действий.
            // если оказалось, что первой части локатора удовлетворяют несколько элементов
            //и первый из них не удовлетворяет второму условию , то поиск закончится и будет NoSuchElementException

            //способ поиска со сложным локатором -> если оказалось,что первой части локатора удовлетворяют несколько элементов
            //и первый из них не удовлетворяет второму условию , то поиск продолжится т.к тут используется не алгоритм!
        }

        //ОЧЕНЬ частая ошибка при поиске от другого элемента и спользованием XPath
        //var form = driver.FindElement(By.ClassName("b-footer"));
        //var input = form.FindElement(By.XPath(".//*[@class='b-footer']"));
        //Так, как мы ищем от элемента, то нужно не забыть УКАЗАТЬ ТОЧКУ в XPath запросе .//
        // // - так ничинается абсолютный запрос(от корня дерева) .// - так начинается относительный запрос(от элемента)
        //У CSS такой проблеммы нет.
    }
}
