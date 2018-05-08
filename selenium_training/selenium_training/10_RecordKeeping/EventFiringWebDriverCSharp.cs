using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;

namespace selenium_training._10_RecordKeeping
{
    [TestFixture]
    class EventFiringWebDriverCSharp
    {
        //Протоклирование - ФИКСАЦИЯ некоторых событий, которые происходят при выполнении сценариев.
        //Самый простой механизм фиксации - это использование класса Event Firing Webdriver.
        //http://seleniumhq.github.io/selenium/docs/api/dotnet/index.html => OpenQA.Selenium.Support.Events => EventFiringWebDriver Class
        //Если перейти по ссылке выше и зайти в EventFiringWebDriver Class можно увидеть СОБЫТИЯ, которые Driver может порождать.
        //Для того, чтобы эти события перехватить - нужно заригестрировать обработчики, которые будут выполнять всю необходимую работу.

        //Добавим протоколирование выполнения методов, которые занимаются поиском элементов.
        //1. добавляем using OpenQA.Selenium.Support.Events;

        private EventFiringWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new EventFiringWebDriver(new ChromeDriver());
            //driver.FindingElement - СОБЫТИЕ, которое генерируется ПЕРЕД началом поиска элемента. 
            //args.FindMethod - выводит ЛОКАТОР, но есть методы для вывода ДРАЙВЕРА, который выполнял поиск - args.Driver и элемента ОТ КОТОРОГО выполнялся поиск
            driver.FindingElement += (sender, args) => Console.WriteLine(args.FindMethod);
            //driver.FindElementCompleted - событие, которое происходит тогда, когда элемент находится методом FindElement(s)
            driver.FindElementCompleted += (sender, args) => Console.WriteLine(string.Concat(args.FindMethod, " was found"));
            // driver.ExceptionThrown - событие, которое происходит тогда, когда выбрасывается ИСКЛЮЧЕНИЕ. args.ThrownException, args.Driver
            driver.ExceptionThrown += (sender, args) => Console.WriteLine(args.ThrownException);
            driver.Manage().Window.Maximize();
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Login()
        {
            driver.Url = "http://google.com";
            var element = wait.Until(d => d.FindElement(By.Name("q")));
            element.SendKeys("webdriver");
            new Actions(this.driver).MoveToElement(driver.FindElement(By.CssSelector("img[alt=Google]"))).Click().Perform();
            var okButton = wait.Until(d => d.FindElement(By.Name("_btnK")));
            okButton.Click();
            wait.Until(d => d.Title.Equals("webdriver - Поиск в Google"));

        }

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }

        //КОГДА ТЕСТ РАБОТАЕТ
        //By.Name: q
        //By.Name: q was found
        //By.CssSelector: img[alt = Google]
        //By.CssSelector: img[alt = Google] was found
        //By.Name: btnK
        //By.Name: btnK was found

        //Когда поставил несуществующий локатор - в логах видно как бросаются исключения OpenQA.Selenium.NoSuchElementException: no such element: Unable to locate element: {"method":"name","selector":"_b


        //ИТОГ:
        //Разуемеется, необязательно выводить эту инфирамию на консоль.
        //Её можно записывать куда угодно: в файл, базу данных, в облачное хранилище и т.д.
        //КОГДА ТЕСТ УПАЛ - эти логи могут быть очень полезны т.к можно понять что случилось.
        //Можно делать все, что угодно. Как это сделать? Все, что мы хотим сделать нужно 
        //делать через обработчики. Т.е вместо Console.WriteLine(args.FindMethod) написать все что угодно.
    }
}
