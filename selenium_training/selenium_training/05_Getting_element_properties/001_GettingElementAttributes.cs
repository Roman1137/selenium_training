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

namespace selenium_training.Lection_5
{
    [TestFixture]
    class GettingElementAttributes
    {
        //Команда GetAttribute называется неправильно т.к она возвращает не значение аттрибута, А ЗНАЧЕНИЕ СВОЙСТВ.
        //То, что мы видим на вкладке Elements - то именно аттрибуты.
        //Так же есть вкладка Proresties.
        //Каждый обьект в Document Object Model представляет собой обьект языка программирования JavaScript.
        //У этого обьекта есть набор свойств. Есть свойства, НАЗВАНИЕ которых и иногда значение совпадает с аттрибутами.
        //СВОЙСТВА СТРОЯТСЯ ИЗ АТТРИБУТОВ.
        //С помощью команды GetAttribute можно получать свойства элемента.

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
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#box-most-popular"), "Most Popular"));


            var attribute = driver.FindElement(By.CssSelector("[name=login]")).GetAttribute("textContent");
            var field = driver.ExecuteJavaScript<IWebElement>("return document.querySelector('[name=login')");
            //var property = field.GetProperty("type");

            //Берем исходный код DOM всех страницы используя GetAttribute("outerHTML") у элемента body
            var bodyAttribute = driver.FindElement(By.CssSelector("body")).GetAttribute("outerHTML");

            //GetAttribute
            //1.value - содержимое полей ввода. Ввел что-то в поле ввода.
            //аттрибут value пустой, а вот PROPERTY value имеет значение того текста, который я ввел.
            //т.е аттрибут и property разные! Метод  GetAttribute вернет значение property. Значение ТЕКСТА поля ввода будет ""
            var element = driver.FindElement(By.CssSelector("[placeholder=Search]"));
            element.Clear();
            element.SendKeys("test");
            var valueTextBox = element.GetAttribute("value");
            element.Clear();
            element.SendKeys("test2");
            valueTextBox = element.GetAttribute("value");

            //2. href , src. Аттрибуты и Property могут отличаться. Атрибут имеет значение /litecart/  - это относительная ссылка т.к в ней 
            //не указан адрес сервера. А Property имеет значение "http://localhost/litecart/". 
            var linkElement = driver.FindElement(By.CssSelector("#breadcrumbs-wrapper a"));
            var hrefProperty = linkElement.GetAttribute("href"); //имеет значение "http://localhost/litecart/". 
            //3. Булевские аттрибуты :selected, readonly, checked etc. Для этих аттрибутов селениум возвращает всегда
            //одно значение : true - если этот аттрибут присутствует и null если он отсутсвует. Когда аттрибут присутствует, то
            // у него может быть значение. Это значение полностью игнорируется. Если он есть, то всегда возвращается значение true.
            //откроем поп ап
            driver.FindElement(By.CssSelector(".fancybox-region")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#fancybox-content")));
            var selectedElement = driver.FindElement(By.CssSelector("[name=currency_code] [value=USD]"));
            var notSelectedElement = driver.FindElement(By.CssSelector("[name=currency_code] [value=EUR]"));

            var selectedValue = selectedElement.GetAttribute("selected"); //вернуло значение true
            var selectedValue1 = selectedElement.Selected; //вернуло значение true

            var notSelectedValue = notSelectedElement.GetAttribute("selected"); //вернуло null т.к этот элемент не выбран. 
            //хотя и свойство selected этого элемента имеет значение false, но Selenium вернул null т.к он возвращает или true или null.
            var selectedValue2 = notSelectedElement.Selected; //свойство Selected вернуло false , а не null
        }
    }
}
