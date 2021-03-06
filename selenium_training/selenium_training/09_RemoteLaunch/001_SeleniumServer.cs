﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._09_RemoteLaunch
{
    [TestFixture]
    class SeleniumServer
    {
        //Всмомагательные исполняемый файлы(драйверы) взаимодействуют с браузерами по внутренним протоколам. У каждого браузера этот
        //протокол свой. Это взаимодействие чаще всего проиходит по сети, НО В ПРЕДЕЛАХ ОДНОЙ МАШИНЫ.
        //А от взаимодействие между клиентской библиотекой и драйвером, которое происходит по протоколу , описанному в W3C Webdriver 
        //можно реализовать удаленно. Клиентская библиотека работает на одной машине, а ДРАЙВЕР И БРАУЗЕР запускаются на другой машине.
        //Для того, чтобы это все работало нужен АГЕНТ на машине, где будут запускатся тесты, он будет слушать на каком-то сетевом
        //порту, и по команде запускать драйвер и браузер. Этот агент получил название Selenium Server.
        //Он работает на машине и ждет. В какой-то момент от КЛИЕНТСКОЙ БИБЛИОТЕКИ ему приходит запрос на инициализацию драйвера.
        //Тогда он запускает драйвер => драйвер запускает браузер и когда все ок Selenium Server отправляет обратно в КЛИЕНТСКУЮ БИБЛИОТЕКУ
        //информацию о том, что все сессия работы с драйвером запущена, можно в ней выполнять какие-небудь команды. 
        //ПОСЛЕ ЭТОГО ВСЕ РАВНО, ДАЛЬНЕЙШЕЕ ВЗАИМОДЕЙСТВИЕ КЛИЕНТСКОЙ БИБЛИОТЕКИ И ДРАЙВЕРА ПРОИСХОДИТ ЧЕРЕЗ SELENIUM SERVER.
        //Т.Е клиентская библиотека отправляет все команды "АГЕНТУ"(Selenium Server), а он уже пересылает все команды драйверу.
        //Так более безопасно т.к каждый раз когда запускается браузер он испоьзует новый порт и злоумышленник может просканировать, увидеть открытые
        //порты и начать управлять браузером. Эти порты выделяются случайно и защитить их достаточно сложно.
        //ДРУГОЕ ДЕЛО Selenium Server - ОН ВСЕГДА ЗАНИМАЕТ ОДИН И ТОТ ЖЕ ПОРТ, и можно разрешить соединение на этот порт только с каких-то
        //определенных машин- откуда мы будем запускать тесты(где находится клиентская библиотека) а с других машин - запретить.

        //ЗАЧЕМ НУЖЕН УДАЛЕННЫЙ ЗАПУСК??
        //1. Мы можем использовать разные машины, разные операциооные системы и разные версии браузеров. 
        //- в часности разные версии Internet Explorer.(Других браузеров это не касается - можно иметь много браузеров разных версий
        //на одной машине, но с Internet Explorer - нельзя установить несколько версий на одну и ту же машину).
        //2. Два браузера на разных машинах одновреммено - чтобы оба браузера были на переднем плане. То, что на одной машине несколько
        //браузеров работают одновременно означает, что нескоторые браузеры находятся на заднем плане, а это может влиять на результат тестов.
        //3. Много браузеров - не хватает мощности. До 5 браузеров на одной машине будут нормально работать, но больше - это уже перебор. Нужно 
        //несколько таких машин.

        //Selenium Server ВЫСТУПАЕТ В РОЛИ УДАЛЕННОГО АГЕНТА.
        //https://www.youtube.com/watch?v=vY9QNwX_IsY - прример установки Ubuntu
        //https://www.screencast.com/t/1SwVdwjFRm - пример настроек самого Ubuntu


        //Manual steps to install geckodriver on Ubuntu:
        //1.visit https://github.com/mozilla/geckodriver/releases
        //2.download the latest version of "geckodriver-vX.XX.X-linux64.tar.gz"
        //3.unarchive the tarball(tar -xvzf geckodriver-vX.XX.X-linux64.tar.gz)
        //4.give executable permissions to geckodriver(chmod +x geckodriver)
        //5.move the geckodriver binary to /usr/local/bin or any location on your system PATH.

        //Нужно открыть терминал и написать: java -jar. selenium-server-standalone-3.0.1.jar 
        //Если добавить -h в конце , то можно увидеть подсказки.
        //Самая важная опция -port(по умолчанию Selenium Server использует порт 4444)
        //Еще одна важная опция -timeout определяет время после которого Selenium Server автоатически остановить запущенный браузер и 
        //запущенный драйвер. Если клиентская библиотека не выполнила команду Quit, то браузер так и продолжит "висеть" пока не истечет -timeout.

        //Запустил Selenium Server и ещё нужно узнать адрес машины где он запущен, для этого нужно открыть другую консоль и ввести ifconfig
        //у меня было так inet addr:192.168.0.104




        private IWebDriver driver;
        [SetUp]
        public void SetUp()
        {
            //Протокол W3C Webdriver работает ПОВЕРХ ТРАНСПОРТНОГО ПРОТОКОЛА HTTP по этому пишем http
            //capabilities https://github.com/SeleniumHQ/selenium/wiki/DesiredCapabilities
            //При использовании Seleniu Server не нужно указывать платформу. А при Selenium Grid - нужно

            //Можно или так запускать
            //driver = new RemoteWebDriver(new Uri("http://192.168.0.104:4444/wd/hub"), DesiredCapabilities.Firefox());
            //или так (Можно и firefox -  new FirefoxOptions().ToCapabilities())
            driver = new RemoteWebDriver(new Uri("http://192.168.0.104:4444/wd/hub"), new ChromeOptions().ToCapabilities());
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

        //Есть ещё один способ посмотреть что происходит на удаленной машине где работает Selenium Server
        //Для этого можно установить с ним соединение из браузера используя inet addr-
        //http://192.168.0.104:4444/ и выбрать console
        //Там есть функция Take Screenshot и можно в любой момент вермени посмотреть что происходит на удаленной машине.
        //Так же можно "закрывать сессии" при помощи "Delete Session"

        //Chrome
        // https://www.youtube.com/watch?v=i-IVuKG5abc
        //# Install ChromeDriver.
        // wget -N http://chromedriver.storage.googleapis.com/$CHROME_DRIVER_VERSION/chromedriver_linux64.zip -P ~/
        // unzip ~/chromedriver_linux64.zip -d ~/
        //rm ~/chromedriver_linux64.zip
        //sudo mv -f ~/chromedriver /usr/local/bin/chromedriver
        //sudo chown root:root /usr/local/bin/chromedriver
        //sudo chmod 0755 /usr/local/bin/chromedriver
    }
}
