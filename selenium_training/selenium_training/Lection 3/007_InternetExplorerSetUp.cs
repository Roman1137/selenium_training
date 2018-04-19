using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_2
{
    [TestFixture]
    class InternetExplorerSetUp
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        //1. Настройки бразура - безопаснось - для каждой из зон: "Интернет", "Местная интрасеть", "Надежные сайты", "Опасные сайты"
        // нужно либо включить, либо выключить защищенный режим.
        //есть альтернатива - options.IntroduceInstabilityByIgnoringProtectedModeSettings, но лучше никогда не использовать
        //2. Масштаб должен быть 100%
        [SetUp]
        public void SetUp()
        {
            var options = new InternetExplorerOptions();
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;
            options.IgnoreZoomLevel = true; //игнорирует масштаб
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true; //лучше НИКОГДА не использовать!

            options.AddAdditionalCapability("-embedding", true);

            driver = new InternetExplorerDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            Console.Out.Write(options.ToCapabilities());
        }


        [Test]
        public void Test()
        {
            const string text = "selenium", textBoxlocator = "lst-ib";
            driver.Url = "https://www.google.com.ua";
            driver.FindElement(By.Id(textBoxlocator)).SendKeys(text + Keys.Enter);
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id(textBoxlocator), text));
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
