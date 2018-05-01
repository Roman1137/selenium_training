using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_5
{
    [TestFixture]
    class GettingElementStyles
    {
        //Обычно делают так: какой-то стиль привязывают к определенному идентефикатору классу. Тем элементы,
        //которые нужно выделить этим стилем вешают этот идентефикатор или класс.И именно это нужно проверять
        //в автотестах, что элемент имеет идентефикатор или класс, но желательно не проверять стили каждого элемента.

        //Например: предполагается проверить, что сообщению о ошибке назначен класс error, а какой именно размер и какой именно
        //цвет там, онисано в стилевом файле и это проверять не надо. Т.е в тестах мы проверяем СТРУКТУРНУЮ правильность траницы, то, что
        //элементы там правильно размечены и не проверяем, что этим элементам дейсвительно назначаются правильные цвета и размеры.
        //Почему? так проще. Так более стабильные тести получаются. Классы  меняться не будут, а вот концкретные цвета
        //и конкретное оформление, могут меняться.

        //GetCssValue - почему лучше не пользоваться? Потому, что значение в разных браузерах может отображаться по-разному.
        //В W3C ничего не написано по поводу того, что эта команда должна возвращать. Возвращает то, что решает браузер. А разные браузеры
        //решают по разному.
        //Цвет (color)  - нормализация RGBa (Red Green Blue alfa).      alfa - прозрачность.

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
            driver = new FirefoxDriver();
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
        public void test()
        {
            driver.Url = "https://selenium2.ru/";
            var element = driver.FindElement(By.CssSelector("[itemprop=headline] a"));
            var text = element.Text;
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("[itemprop=headline] a"),"Что такое"));

            //Сhrome и Firefox показуют немного разные данные.
            var color = element.GetCssValue("color");
            var backGroundColor = element.GetCssValue("background-color");
            var borderColor = element.GetCssValue("border-color");
            //комбинированные свойства (font, background)-> Драйвер IE вообще не возвращает комбинированные свойства.
            //Нужно указывать каждое свойство по отдельности.

            //ИТОГ: нужно проверить, что нужным элементам назначены правильные классы, а цвета, шрифты и т.д - лучше провкрять вручную.
        }
    }
}
