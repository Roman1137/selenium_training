using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._10_RecordKeeping
{
    [TestFixture]
    class BrowserLogs
    {
        //Помимо тестирования пользовательского интерфейса иногда бывает полезно заглядывать в логи браузера,
        //ОСОБЕННО на консоль, которая содержит на ошибку JavaScript.

        //Реальный пример из жизни: даже если эти ошибки никак не проявляются в пользовательском интрерфейсе 
        //и кажется, что все работает правильно, лучше бы все таки разобраться, почему такие ошибки возникают и почему
        //в логах возникают такие сообщения. Точно так же можно анализировать и логи сервера, но это
        //не имеет совершенно никакого отношения и инструменту Selenium - этим нужно заниматься при помощи
        //других инструментов(Прокси).
        //А вот ЛОГИ БРАУЗЕРА, которые сожержат ошибки JavaScript, возникших на странице - их МОЖНО ПОЛУЧИТЬ при помощи Selenium.

        //К сожалению, эта функция , пока, работает не во всех браузерах, хотя она есть в стандарте W3C.
        //Наиболее стабильной является браузер Chrome.
        //В Firefox, где используется GeckoDriver данная функция не реализована совсем.
        //Но, если использзуется СТАРАЯ ВЕРСИЯ Firefox, то доступ к логам браузера МОЖНО ПОЛУЧИТЬ.
        //IE - не понятно, но вроде не работает.

        private ChromeDriver driver;
        private WebDriverWait wait;

        //[SetUp]
        //public void SetUp()
        //{
        //    driver = new ChromeDriver();
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //    var allLogTypes = driver.Manage().Logs.AvailableLogTypes; //С первого раза вывело "browser" и "driver"
        //    //логи "driver" в основном бесполезны и спользуются для ОТЛАДКИ САМОГО ДРАЙВЕРА.
        //}

        [SetUp]
        public void SetUpForPerformanceLogs()
        {
            //http://chromedriver.chromium.org/logging/performance-log
            //Кроме того, драйвер для браузера Chrome позволяет получить информация о производительности
            //тех операций, которые выполняются внутри браузера => Performance log прочитать.
            //По умолчанию эта фозможность отключена, но ее можно активировать, с помощью Capabilities
            var perfLogsPref = new ChromePerformanceLoggingPreferences();
            perfLogsPref.AddTracingCategory("devtools.timeline");
            var options = new ChromeOptions();
            options.PerformanceLoggingPreferences = perfLogsPref;
            options.SetLoggingPreference("performance", LogLevel.All);

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var allLogTypes = driver.Manage().Logs.AvailableLogTypes; //Теперь видно, что я могу посмотреть Performance логи
            //Performance логов очень много потому, что там можно увидеть информацию о загрузке всех ресурсов, которые
            //встречаются на этой странице(картинки, скрипты и т.д). Можно посмотреть сколько времени заняла загрузка,
            //сколько данных загрузилось, сколько времени ушло на отрисовку страницы и т.д.
            //Читать такие логи очень сложно, но есть люди, которые этим занимаются - измеряют и тестируют производительность страницы(скорость ее загрузки).
            //НО это НЕ ТЕСТИРОВАНИЕ ПРОИЗВОДИТЕЛЬНОСТИ(когда сильно нагружием сервер).
            //Здесь идет речь о другом тестировании производительности, о том, как быстро загружаются и отрисовываются страницы в браузере.
            //Для пользователя важно и то и то - чтобы сервер быстро присылал и чтобы все быстро отрисовывалось.
        }


        [Test]
        public void Login()
        {
            driver.Url = "https://itvdn.com/ru";
            wait.Until(d => d.FindElement(By.CssSelector(".menu-items-list")));

            //ЛОГИ
            //Даже если эта ошибка не проявляется в пользовательском интерфейсе, очень может быть, что из-за этой ошибки
            //какая-то функция НЕ РАБОТАЕТ.
            //var browserLogs = driver.Manage().Logs.GetLog(LogType.Client);

            var browserLogs = driver.Manage().Logs.GetLog("performance"); //Performance логи можно посмотреть только так
            browserLogs.ToList().ForEach(Console.WriteLine);
        }

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }

        public void TestCleanup()
        {
            var errorStrings = new List<string>
            {
                "SyntaxError",
                "EvalError",
                "ReferenceError",
                "RangeError",
                "TypeError",
                "URIError"
            };

            var jsErrors = driver.Manage().Logs.GetLog(LogType.Browser).Where(x => errorStrings.Any(e => x.Message.Contains(e)));

            if (jsErrors.Any())
            {
                Assert.Fail("JavaScript error(s):" + Environment.NewLine + jsErrors.Aggregate("", (s, entry) => s + entry.Message + Environment.NewLine));
            }
        }
    }
}
