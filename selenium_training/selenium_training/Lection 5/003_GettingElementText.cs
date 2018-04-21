using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_5
{
    class GettingElementText
    {
        //Селениум пытается получить ТОТ ТЕКСТ, КОТОРЫЙ ВИДИТ ПОЛЬЗОВАТЕЛЬ
        //Из этого вытекает:
        //1.если элемент невидимый, то текст у него пустой.
        //2.если нужно получить текст невидимого элемента, то нужно использовать element.GetAttribute("textContent")

        //3.если внутри видимого элемента находится невидимы элемент, то текст невидимого элемента будет игнорироваться.
        //4.если нужно получить весь текст в том числе и невидимого элемента, то нужно использовать element.GetAttribute("textContent")

        //5.Селениум возвращает нормальзованный текст(проблелы)=> если идут несколько подрят пробелов, то они схлопываются в один.

        //6.Если текст оформлен с помощью стиля, указывающего, у текста долно сохраняться форматирование => Preformatted,
        //то селениум при попытке получить текст этого элемента тоже сохранит все пробелы. Это следствие основного правила => 
        //селениум должен возвращать текст, который ВИДИТ пользователь!
        //7. Для полей ввода метод, возвращающий текст вернет пустую строку.
        //ИТОГ: всгда нужно проверить что вернет Text у любого элемента, если это нужно для теста, т.к тест может 
        //оказаться пустым и для того, чтобы добраться к тому тексту, нужно будет использовать другую команду.

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
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#box-most-popular"),"Most Popular"));
            var element = driver.FindElement(By.CssSelector("[placeholder=Search]"));
            element.Clear();
            element.SendKeys("test           test"+Keys.Enter);
            //var a = driver.FindElement(By.CssSelector("title")).Text; //вообще ничего не возвращает.
            //т.е ТО, ЧТО ВЫГЛЯДИТ КАК ТЕКСТ У ЭЛЕМЕНТА НЕ ФАКТ, ЧТО БУДЕТ ВОЗВРАЩАТЬСЯ С ПОМОЩЬЮ МЕТОДА getText(в C# свойства Text)
            wait.Until(x=>x.FindElement(By.CssSelector("title")).GetAttribute("textContent").Contains("test"));
            var elementTitle = driver.FindElement(By.CssSelector("title"));
            elementTitle.GetAttribute("textContent");
        }
    }
}
