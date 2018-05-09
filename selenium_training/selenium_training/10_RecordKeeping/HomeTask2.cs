using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatedTester.BrowserMob;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using AutomatedTester.BrowserMob.HAR;
using OpenQA.Selenium.Firefox;

namespace selenium_training._10_RecordKeeping
{
    [TestFixture]
    class HomeTask2
    {
        //Трафик, которым обменивается браузер и сервер представляет большой интерес.
        //Например, можно посмотреть, нету ли каких либо "плохих" ответов, полученных от сервера.
        //К сожалению Selenium не предоставляет возможности доступиться к трафику, которым обменивается браузер и сервер.
        //По этому приходится привлекать инструменты Proxy(на русский - посредник).
        //Такой инструмент встраивается между трафиком и сервером и перехватывает все запросы.
        //Т.е все запросы и ответы проходят через Proxy, следовательно, он может делать все что угодно с вопросами и ответами - их монжо
        //ПРОТОКОЛИРОВАТЬ, ИЗУЧАТЬ И даже, при необходимости ИЗМЕНЯТЬ.
        //Наиболее популярным инструментом такого рода явялется браузер BrowserMob-proxy. Он написан на Java, именно по этому его удобнее всего использовать
        //при написании тестов на этом языке. НО , у этого инструмента есть обертки и для других ЯП (Python, C#, Ruby, JavaScript),
        //но там возможности этого инструмента ниже(нельзя модифицировать отправляемые запросы).

        private IWebDriver driver;
        private WebDriverWait wait;
        private Proxy proxy;
        private Client client;
        private Server server;

        //Час возился пока подключил AutomatedTester.BrowserMob к этому проекту. Нужно было: 
        //1.скачать все из https://github.com/AutomatedTester/AutomatedTester.BrowserMob
        //2. Запустить его(это solution)
        //3. Сбилдить этот solution и в директории X:\AutomatedTester.BrowserMob-master\AutomatedTester.BrowserMob\bin\Debug появился файл AutomatedTester.BrowserMob.dll, который я и подключил.
        //https://www.adathedev.co.uk/2012/02/automating-web-performance-stats.html


        [SetUp]
        public void SetUp()
        {
            server = new Server(@"X:\browsermob-proxy-2.1.4\bin\browsermob-proxy.bat");
            server.Start();

            client = server.CreateProxy();
            client.NewHar("google");
            //это ключевая строчка. Именно сюда браузер будет послать все запросы, а оттуда они уже будут пересылаться на сервер.
            var seleniumProxy = new Proxy { HttpProxy = client.SeleniumProxy };
            var options = new ChromeOptions();
            options.Proxy = seleniumProxy;
            //options.AcceptInsecureCertificates = true;
            driver = new ChromeDriver(options);
        }

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
            client.Close();
            server.Stop();
        }

        [Test]
        public void TestProxy()
        {
            driver.Navigate().GoToUrl("https://selenium2.ru/");
            // Get the performance stats
            HarResult harData = client.GetHar();

            var allRequests = harData.Log.Entries;//cписок содержит инфо о всех перехваченных запросах.
            //Почему-то перехватываются только HTTP запросы, а HTTPS - нет.
            allRequests.ToList().ForEach(l => Console.WriteLine(string.Concat(l.Response.Status, ":", l.Request.Url)));
           
            //Не обязательно выводить информацию на консоль! Можно сделать какие - угодно проверки, например можно проверять, что 
            //нету ответов со статусами 4хх или 5хх серий, можно собирать информацию о времени загрузки этих ресуров,
            //можно анализировать заголовки запросов и ответов, и даже можна анализировать текст ответов(нужно включить опцию, чтобы текст сохранался
            //т.к по умолчанию он не сохраняется.)
        }
        //Не обязательно ограничиваться только AutomatedTester.BrowserMob инструментом. Есть и другие инструменты, которые тоже прекрасно работают,
        //но они не так оптимизированы для работы с Selenium, но использовать их можно.

        //ВАЖНО!!! Указывать настройки прокти при инициализации браузера можно и нужно не только для того, чтобы перехватывать трафик.
        //Иногда нельзя тестировать целевое приложение потому, что оно находится в сети интернет, а я в локальной сети и доступ 
        //к интернету закрыт прокси сервером. В этом случаи нужно будет при инициализации дравера воспользоваться options.Proxy = ...,
        // а в КАЧЕСТВЕ ЗНАЧЕНИЕЯ нужно будет передать обьект типа Proxy, который создан самостоятельно:
        //Proxy proxy = new Proxy();
        //proxy.HttpProxy = "myproxy:8888"; или
        //proxy.ProxyAutoConfigUrl = "адрес скрипта, который содержит информацию о прокси сервере". все это можно выяснить у систамного администратора.

        //А если нужно одновременно работать с сайтаи, которые находятся за прокси сервером и в локальной сети, тогда нужно некоторые сайты 
        //добавить в исключения при помощи свойства 
        //Proxy proxy = new Proxy();
        //proxy.NoProxy

        //Selenium не умеет загружать файлы из сервера. Если появится диалог, предлагающий выбрать место для сохранения файла, то бля Selenium`a браузер потерян.
        //Чтобы избежать такой проблемы - нужно ВНУТРИ ПРОКСИ СЕРВЕРА НАСТРОИТЬ СПЕЦИАЛЬНЫЙ ФИЛЬТР, который видит запрос, содержащий файл, перехватывает
        //этот запрос, файл сохраняет на диск, и всесто этого. отправляет в браузер специальную страничку, которая содержит путь с сохраненному файлу.
        //Можно с этой страницы прочитать адрес загруженного файла и проанализировать его.

        //Есть ещё один диалог, с которым Selenium тоже не умеет работать. Это диалог авторизации(в видео есть пример http://selenium2.ru/articles/106-selenium-i-browsermobproxy-vmeste-veselee.html).
        //С ним помагает справится Proxy. Proxy будет от имени браузера выполнять авторизацию и браузер уже будет попадать в авторизированную сессию.



        //        Automating Web Performance Stats Collection in .NET
        //You have a web application.You're a .NET developer. Maybe you already have some automated UI testing in place via Selenium, or maybe you don't.What you want to do is automate the collection of some performance metrics about your application.

        //Q.How would you go about doing that in .NET?
        //A. Use the following recipe for success.
        //Ingredients
        //BrowserMob Proxy by Webmetrics, which (quote) is:
        //A free utility to help web developers watch and manipulate network traffic from their web applications
        //Selenium, which (quote):
        //automates browsers. That's it.
        //BrowserMob Proxy .NET Library, a.NET library to provide a simple way to work with BrowserMob Proxy and Selenium, written by David Burns (blog | twitter) and myself(you're already on my blog | twitter).
        //Preparation
        //Download the BrowserMob Proxy from GitHub
        //Download the BrowserMob Proxy.NET Library from GitHub (binaries, or get the source and build yourself)
        //Reference Selenium in your.NET project(available via nuget, or from seleniumhq)
        //Reference the BrowserMob Proxy.NET Library in your.NET project
        //Make and Bake
        //Example:
        //using AutomatedTester.BrowserMob.HAR;
        //using OpenQA.Selenium;
        //using OpenQA.Selenium.Firefox;

        //namespace AutomatedTester.BrowserMob.Example
        //    {
        //        public class ExampleClass
        //        {
        //            public void ExampleUse()
        //            {
        //                // Supply the path to the Browsermob Proxy batch file
        //                Server server = new Server(@"C:\BrowserMobProxy\bin\browsermob-proxy.bat");
        //                server.Start();

        //                Client client = server.CreateProxy();
        //                client.NewHar("google");

        //                var seleniumProxy = new Proxy { HttpProxy = client.SeleniumProxy };
        //                var profile = new FirefoxProfile();

        //                profile.SetProxyPreferences(seleniumProxy);
        //                // Navigate to the page to retrieve performance stats for
        //                IWebDriver driver = new FirefoxDriver(profile);
        //                driver.Navigate().GoToUrl("http://www.google.co.uk");

        //                // Get the performance stats
        //                HarResult harData = client.GetHar();

        //                // Do whatever you want with the metrics here. Easy to persist 
        //                // out to a data store for ongoing metrics over time.

        //                driver.Quit();
        //                client.Close();
        //                server.Stop();
        //            }
        //        }
        //    }
        //    What's great is that if you already have some Selenium tests in place, you can add in the collection of performance metrics quickly and easily. This gives you the ability to collate performance metrics over time - a perfect way to identify problem areas to investigate and to quantify performance improvements you make. To learn more about what is in the performance data, check out these links which go into more detail about the HAR format (HTTP Archive) - this is what Webmetric's BrowserMob Proxy returns, which we expand out into a POCO structure(HarResult type).
        //HTTP Archive specification google group
        //HAR 1.2 Spec
        //BrowserMob Proxy allows you to do some pretty funky stuff, such as:
        //blacklisting / whitelisting content
        //simulate network traffic / latency
        //The.NET library wrapper we've made available supports this functionality. Hopefully, this will come in useful for those in the .NET world!
    }
}
