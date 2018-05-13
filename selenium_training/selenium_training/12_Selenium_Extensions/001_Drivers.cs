using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace selenium_training._12_SeleniumExtensions
{
    [TestFixture]
    class Drivers
    {
        //Для Selenium`а дело не ограничивается браузерами. По такой схеме Клиентская Библиотека=> драйвер => приложение
        //можно управлять чем угодно.

        //Драйверы:
        //Selenium включал драйверы для IE, FF, Chrome, Safari, Opera. Сейчас, создание этих драйверов передано самим разрабротчикам браузеров.

        //Но, помимо этих браузеров, используемых реальными людьми, есть ПСЕВДОбраузеры, которые используются достаточно часто для автоматизации.
        //Так же, по такой же схеме, можно создать драйвер, который будет управлять чем-нибудь другим, напрример, мобильными приложениями.
        //Речь идет о настоящих нативных и гибридных приложениях.

        //Таких приложений достаточно много и среди них есть приложения, которые работают по классической схеме, 
        //предлагаемой Selenium`ом - Appium, Selendrid, ios-driver.

        //Для тестирования не мобильных, а декстопных приложений тоже есть различные инструменты и среди них есть те,
        //которые тоже используют архитектуру Selenium: Winium.Desktop, AutoItDriverServer, WinAppDriver

        //При написании теста буду использовать Winium.Desktop.
        //На той машине, где будет запускаться приложение СНАЧАЛА НУЖНО СТАРТОВАТЬ АГЕНТ, который будет принимать команды
        //по протоколу W3C Webdriver и выполнять эти команды в нативном приложении.
        //https://github.com/2gis/Winium.Desktop/releases - качаем последнюю версию и просто запускаем!

        public IWebDriver driver;

        [SetUp]
        public void Start()
        {
            var cap = new DesiredCapabilities();
            cap.SetCapability("app", @"C:\Windows\System32\calc.exe");
            this.driver = new RemoteWebDriver(new Uri("http://localhost:9999"), cap); //запускается калькулятор
        }

        //как узнать локаторы? есть специальные инструменты, которые позволяют узнать локтаторы элементов в декстопных приложениях.
        //https://github.com/2gis/Winium.Cruciatus/blob/master/tools/UISpy/UISpy.exe
        [Test]
        public void Test()
        {
            var bar = driver.FindElement(By.Id("MenuBar"));
            bar.FindElement(By.Id("Item 1")).Click();
            bar.FindElement(By.Id("Item 304")).Click();
            bar.FindElement(By.Id("Item 1")).Click();
            bar.FindElement(By.Id("Item 305")).Click();

            driver.FindElement(By.Id("132")).Click();
            driver.FindElement(By.Id("93")).Click();
            driver.FindElement(By.Id("133")).Click();
            driver.FindElement(By.Id("121")).Click();
            Assert.IsTrue(driver.FindElement(By.Id("158")).GetAttribute("Name")=="5");
        }

        [TearDown]
        public void Quit()
        {
            this.driver.Close();
            this.driver.Quit();
            this.driver.Dispose();
            this.driver = null;
        }
    }
}
