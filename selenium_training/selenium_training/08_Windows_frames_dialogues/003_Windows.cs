using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._08_Windows_frames_dialogues
{
    [TestFixture]
    class Windows
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string windowNameIWantToSwitchTo;
        //Основные операции:
        //1.Открытие
        //Приложение само может открывать окно(табу)
        //Или это можно сделать через JavaScript(в Selenium такой команды ПОКА НЕТ): 
        public void Method()
        {
            driver.ExecuteJavaScript("window.open();"); //открыть новое окно
            //если мы открыли разные браузеры, которые управляются
            //разными драйверами, то у каждого уз этих браузеров будет свой независимый список окон.
            var allWindowHandles = driver.WindowHandles; //возвращается список идентефикаторов всех окнон браузеров В СЛУЧАЙНОМ ПОРЯДКЕ
            var activeWindow = driver.CurrentWindowHandle; //возвращается string ИДЕНТЕФИКАТОР текущего окна
            //для того, чтобы переключиться в какое-то окно, нужно узнать его идентефикатор, а потом уже выполнить команду:
            driver.SwitchTo().Window(windowNameIWantToSwitchTo);
            //после того, как окно(табак) созданы, Selenium автоматически не переключается туда, и нужно самостоятельно
            //определить идентефикатор окна, и самому переключиться туда используя команду driver.SwitchTo().Window()
            driver.Close(); //после закрытия окна(табы) Selenium автоматически не преключается в "живую" табу,
            //нужно самому выполнить переключение используя команду driver.SwitchTo().Window().
        }

        [Test]
        public void LaunchNewBrowser()
        {
            //driver = new InternetExplorerDriver(); //тут открывается окно

            driver = new ChromeDriver(); //В ChromeDriver открывается не окно, а ТАБА при выполнении driver.ExecuteJavaScript("window.open();");

            //var options = new FirefoxOptions();  //при запуске Firefox ПО СТАРОЙ СХЕМЕ ОТКРЫВАЕСЯ ОКНО при выполнении driver.ExecuteJavaScript("window.open();")
            //options.UseLegacyImplementation = true; //если в окне открыть несколько вкладок, то Selenium их не заметит и будет думать, что это одно окно.
            //options.BrowserExecutableLocation = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
            //driver = new FirefoxDriver(options);

            //FirefoxOptions options = new FirefoxOptions(); //при запуске Firefox ПО НОВОЙ СХЕМЕ ОТКРЫВАЕТСЯ ТАБА при выполнении driver.ExecuteJavaScript("window.open();")
            //options.UseLegacyImplementation = false;
            //driver = new FirefoxDriver(options);
            
            wait=new WebDriverWait(this.driver,TimeSpan.FromSeconds(5));
            driver.Url = "https://selenium2.ru/";
            var allWindowHandles = driver.WindowHandles;
            var firstWindow = driver.CurrentWindowHandle;
            driver.ExecuteJavaScript("window.open();");
            var newWindowIdenteficator = wait.Until(x => x.WindowHandles.Except(allWindowHandles).First());
            allWindowHandles = driver.WindowHandles; //порядок идентефикаторов выстроен по алфивиту, а не по порядку создания окон!
            var justCreatedWindow = allWindowHandles.First(x => !x.Equals(firstWindow));
            var currentWindow = driver.CurrentWindowHandle;
            driver.SwitchTo().Window(justCreatedWindow);
            currentWindow = driver.CurrentWindowHandle;
            driver.Url = "https://dou.ua/";
            driver.Close();
            //возникает исключение  NoSuchWindowException: т.к мы не переключились в "живое окно"
            //кстати, это же исключение будет если мы попробует выполнить любой действие в этом, уже закрытом окне(не переключившись)
            currentWindow = driver.CurrentWindowHandle;
            //var tittle = driver.Title; //и тут тоже ошибка т.к я ещё не переключился
            //Какие операции доступны в таком состоянии? 1.Взять список всех "живых окон".2.Переключение в одно из живых окон
            allWindowHandles = driver.WindowHandles;
            driver.SwitchTo().Window(firstWindow);
            driver.Url = "https://google.com/";
        }

        [Test]
        public void SwitchingToOtherWindows()
        {
            driver = new ChromeDriver();
            //запоминвем идентефикатор текущего окна
            var originalWindow = driver.CurrentWindowHandle;
            //запоминаем идентефикаторы уже открытых окон
            var existingWindows = driver.WindowHandles;
            //кликаем кнопку, которая открывает новое окно
            driver.FindElement(By.ClassName(".button")).Click();
            //ждем появления нового окна с новым идентефикатором
            var newWindowIdenteficator = wait.Until(x => x.WindowHandles.Except(existingWindows).First());
            //переключаемся в новое окно
            driver.SwitchTo().Window(newWindowIdenteficator);
            //закрываем его
            driver.Close();
            //возвращаемся в исходное окно
            driver.SwitchTo().Window(originalWindow);
        }

        [TearDown]
        public void Stop()
        {
            driver.Dispose();
        }
    }
}
