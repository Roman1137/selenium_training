using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._10_RecordKeeping
{
    class TakingScreenShot
    {
        //Видео, конечно, лучше, чем скриншот. Проблема может успеть "исчезнуть" перед тем, как после сбоя в тестовом
        //сценарии будет сделан скриншот, а видео можно отмотать назад и посмотреть тот момент, который нужен.

        //Есть Видеорекордер (Java): https://github.com/SergeyPirogov/video-recorder-java
        //А так же большинство облачных сервисов, предоставляющий тестовый грид так же предоставляют запись видео, но
        //Это реализуется не срадствами Selenium. В облаке стертует виртуальная машина и сразу же включается запись видео.
        //Видео записывается до тех пор, пока тесты "ранятся".

        //Рекомандуют писать видео не каждого теста, а писать видео непрерывно пока работает узел. Нужно настроить
        //профиль операционной системы на машине, где будут запускатся тесты так, чтобы при старте, сразу же 
        //запускался рекордер и он записывал все до того момента, пока контейнер не будет остановлен.

        //СКРИШОН
        //Снятие скриншота организуется с помощью Selenium.
        //В какой момент нужно снимать скриншот? 
        //Во- первых, можно вставить код снятия скриншота в произвольное место внутри сценария в тот момент, когда
        //я считаю, что ВОТ ЗДЕСЬ БЫЛО БЫ ПОЛЕЗНО СОХРАНИТЬ КАРТИНКУ.
        //Во вторых - полезно снимать скриншот при условии, что тест сохранился неуспешно.
        //КСТАТИ- снимать скришноты полезно перед/после какого-то события в Selenium, например - перед
        //каждый кликом, или после возникновения исключения. Это можно сделать в помощью событий из прошлого модуля(EventFiringWebDriver).

        private EventFiringWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new EventFiringWebDriver(new ChromeDriver());
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
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var fileName = TestContext.CurrentContext.TestDirectory + "\\" +
                               DateTime.Now.ToString("yy-MM-dd-HH-mm-ss-FFF") + "-" + GetType().Name + "-" +
                               TestContext.CurrentContext.Test.FullName + "." + ScreenshotImageFormat.Jpeg;
                try
                {
                    ((ITakesScreenshot) driver).GetScreenshot().SaveAsFile(fileName, ScreenshotImageFormat.Jpeg);
                    TestContext.AddTestAttachment(fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }
    }
}
