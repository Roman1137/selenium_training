using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_3
{
    [TestFixture()]
    public class DocumentObjectModel
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

        public IWebElement element => driver.FindElement(By.Id("lst-ib"));
        //после выполнение метода FindElement возвращается уникальный модификатор элемента,
        //который генерируется динамически(физический адрес элемента).
        //если элемент с таким идентефикатором пропадает или страница перезагружается - то при
        //попытке обращения к этому элементу возникнет ошибка StaleElementRefferanceExpection
        [Test]
        public void Login()
        {
            driver.Url = "https://www.google.com.ua";
            element.SendKeys("selenium");
            driver.Navigate().Refresh();
            element.SendKeys("selenium");
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
