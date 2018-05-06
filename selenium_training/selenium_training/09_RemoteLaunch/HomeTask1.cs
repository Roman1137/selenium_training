using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace selenium_training._09_RemoteLaunch
{
    [TestFixture]
    class HomeTask1
    {
        //  java -jar selenium-server-standalone-3.11.0.jar -role node -hub http://192.168.0.102:4444/wd/hub -capabilities browserName=iexplorer,maxInsances=5

        private IWebDriver driver;
        //[SetUp]
        //public void SetUp()
        //{
        //    var options = new ChromeOptions(); //должен запуститься Chrome на основной машине
        //    var capabilities = options.ToCapabilities() as DesiredCapabilities;
        //    //платформу не указываем т.к я настроил, чтобы на виртуальной машине могли запускаться ТОЛЬКО тесты в IE, а на другой - все кроме IE
        //    driver = new RemoteWebDriver(new Uri("http://192.168.0.102:4444/wd/hub"), capabilities);
        //}

        //[SetUp]
        //public void SetUp()
        //{
        //    //платформу не указываем т.к я настроил, чтобы на виртуальной машине могли запускаться ТОЛЬКО тесты в IE, а на другой - все кроме IE
        //    var options = new InternetExplorerOptions();
        //    options.GetType().GetProperty("BrowserName").SetValue(options, "iexplorer"); //работает

        //    var capabilities = options.ToCapabilities() as DesiredCapabilities;
        //    driver = new RemoteWebDriver(new Uri("http://192.168.0.102:4444/wd/hub"), capabilities);

        //    // capabilities: Capabilities {
        //    //browserName: iexplorer, maxInstances: 5, platform: VISTA,
        //}

        [SetUp]
        public void SetUp()
        {
            //платформу не указываем т.к я настроил, чтобы на виртуальной машине могли запускаться ТОЛЬКО тесты в Chrome, а на другой - только FireFox
            var options = new ChromeOptions();
            var capabilities = options.ToCapabilities() as DesiredCapabilities;
            driver = new RemoteWebDriver(new Uri("http://192.168.0.102:4444/wd/hub"), capabilities);
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
        public void RemoteTest()
        {
            this.driver.Url = "https://selenium2.ru/";
        }
    }
}
