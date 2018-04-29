using System;
using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training
{
    [TestFixture]
    public class Lection_1
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

        [Test]
        public void FirstTest()
        {
            const string text = "selenium", textBoxlocator = "lst-ib";
            driver.FindElement(By.Id(textBoxlocator)).SendKeys(text + Keys.Enter);
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id(textBoxlocator), text));
        }

        [Test]
        public void Login()
        {
            const string login = "admin", password = "admin";
            driver.Url = "http://localhost/litecart/admin";
            driver.FindElement(By.CssSelector("[name='username']")).SendKeys(login);
            driver.FindElement(By.CssSelector("[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("[name='login']")).Click();
            wait.Until(ExpectedConditions.ElementExists((By.CssSelector("[title='Logout']"))));
        }

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }
        //[Запуск браузера]
        //1.Selenium пытается найти ВСПОМАГАЕМЫЙ ИСПОЛНЯЕМЫЙ ФАЙЛ(тот самый, что делают разработчики браузеров),
        //передет ему некоторые опции и запускает его 
        //2. Это вспомагаемый исполняемый файл (chromedriver.exe), в свою очередь,
        //пытается найти браузер и запустить его.
        //
        //если поменять имя ВСПОМАГАЕМОГО ИСПОЛНЯЕМОГО ФАЙЛА chromedriver.exe или удалить/переместить его из solution'а,
        //то браузер не будет запущен.
        //смоделировал такую ситуацию переименовав chromedriver.exe на no-chromedriver.exe
        //по адресу X:\project\selenium_training\selenium_training\selenium_training\bin\Debug

        //[Открытие страниц]
        // адреса типа www.google.com.ua т.е без http:// нельзя использовать, selenium будет ругаться.
        //  driver.Url это все равно что driver.Navigate().GoToUrl(). Сначала был .Url, потом его заменили на  Navigate().GoToUrl(), НО
        //  driver.Url уже много кто использует (селениуму >10 лет) по этому, решили .Url не удалять.
        // в java это метод get, а в C# это свойство Url.
        //
        //driver.get() : It's used to go to the particular website , But it doesn't maintain the browser History and cookies so 
        //, we can't use forward and backward button , if we click on that , page will not get schedule
        //driver.navigate() : it's used to go to the particular website , but it maintains the browser history and cookies,
        //so we can use forward and backward button to navigate between the pages during the coding of Testcase

        //[Поиск элементов]
        [Test]
        public void StaleElementExceptionExample()
        {
            const string text = "selenium", textBoxlocator = "lst-ib";
            driver.Url = "https://www.google.com.ua";
            var a = driver.FindElement(By.Id(textBoxlocator));

            driver.Navigate().Refresh();
            a.SendKeys(text + Keys.Enter);
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id(textBoxlocator), text));
            //когда браузер находит элемент, то он запоминает его уникуальный идентефикатор
            // типа {Element (id = 0.33902733583273736-1)}
            //каждый раз, когда надо получить информацию о элементе или совершить действие,
            //селениум передает этот идентафикатор. НО если страница перезагрузилась, то этот 
            //идентефикатор БУДЕТ УЖЕ ДРУГИМ даже для того же самого элемента.
            // когда мы пытаемся кликнуть по элементу, идентафикатор которого уже обновился, то мы получаем StaleElementException.

            //СУТЬ: нужно выполнять поиск элемента НЕПОСРЕДСТВЕННО ПЕРЕД ИСПОЛЬЗОВАНИЕМ.
            //НИКОГДА не пихать его в перменную! 
        }

        //[Действия с элементами]
        [Test]
        public void InvisibleElementErrorExample()
        {
            const string text = "selenium", textBoxlocator = "lst-ib";
            driver.Url = "https://www.google.com.ua";
            driver.FindElement(By.Id(textBoxlocator)).SendKeys(text + Keys.Enter);
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id(textBoxlocator), text));

            driver.FindElement(By.Id("gs_ok0")).Click();// open keyboard
            driver.FindElement(By.Id("gs_ok0")).Click();// close keyboard
            driver.FindElement(By.Id("K32")).Click();// space button is invisible
            //OpenQA.Selenium.ElementNotVisibleException

            //Нельзя взаимодействовать с невидимым элементом, потому, что ЛЮДИ 
            // на нажимают на те обьекты, которые они не могут видеть.
            //НИКАКИХ ДЕЙСТВИЙ С НЕВИДИМЫМИ ЭЛЕМЕНТАМИ.
        }
    }
}
