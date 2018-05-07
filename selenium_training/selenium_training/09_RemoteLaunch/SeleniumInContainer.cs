using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace selenium_training._09_RemoteLaunch
{
    [TestFixture]
    class SeleniumInContainer
    {
        //При развертывании Selenium Grid у нас есть ОДНА ПРОСТАЯ ЗАДАЧА - запустить сервер и 
        //МНОЖЕСТВО СЛОЖНЫХ ЗАДАЧ - сконфигурировать и запустить узлы.
        //Иногда нужно много разных узлов, а иногда - много одинаковых улов и возникает проблема как их быстро клонировать.
        //На помощ для решения этих задач могут прийти контейнерные технологии.

        //По сути - каждый контейнер представляет собой виртуальную машину, НО КОНТЕЙНЕРЫ - это МАЛЕНЬКИЕ, ЛЕГКОВЕСНЫЕ виртуальные
        //машины так что, НА ОДНОЙ МАШИНЕ ИХ МОЖНО ЗАПУСКАТЬ ДЕСЯТКАМИ. При этим обезпечивается достаточно хорошая 
        //изоляция контейнера друг от друга, так что мы можем в каждом из них запускать отдельный браузер, и не будем 
        //безпокоиться, что они будут мешать друг-другу или перекрывать друг-друга. Каждый их них будет работать,
        //буду-то он единственный в операционной системе.

        //В рамках проекта Selenium разработан уже готовый набор контейнеров, который используют технологию Docker.
        //Официальная документация у Docker - просто замечательная.
        //https://github.com/SeleniumHQ/docker-selenium

        //В КАЖДОМ ИЗ КОНТЕЙНЕРОВ запускается SELENIUM SERVER в том или ином режиме и какой-то ОДИН БРАУЗЕР.
        //Т.е если нужно будет запускать тесты в большом количестве браузеров - значит потребуется много контейнеров.
        //КОНТЕЙНЕРА:
        //selenium/standalone... - представляет собой сервер
        //selenium/node-... -  представляет собой конечный узел
        //selenium/hub: Image for running a Grid Hub - представляет собой диспетчер

        //selenium/base: Base image which includes Java runtime and Selenium Server JAR file
        //selenium/hub: Image for running a Grid Hub
        //selenium/node-base: Base image for Grid Nodes which includes a virtual desktop environment
        //selenium/node-chrome: Grid Node with Chrome installed, needs to be connected to a Grid Hub
        //selenium/node-firefox: Grid Node with Firefox installed, needs to be connected to a Grid Hub
        //selenium/node-chrome-debug: Grid Node with Chrome installed and runs a VNC server, needs to be connected to a Grid Hub
        //selenium/node-firefox-debug: Grid Node with Firefox installed and runs a VNC server, needs to be connected to a Grid Hub
        //selenium/standalone-chrome: Selenium Standalone with Chrome installed
        //selenium/standalone-firefox: Selenium Standalone with Firefox installed
        //selenium/standalone-chrome-debug: Selenium Standalone with Chrome installed and runs a VNC server
        //selenium/standalone-firefox-debug: Selenium Standalone with Firefox installed and runs a VNC server

        //В СИЛУ ОГРАНИЧЕНИЯ ТЕХНОЛОГИИ DOCKER, на донный момент доступны узлы с БРАУЗЕРАМИ CHROME И FIREFOX
        //Установка Docker
        //https://docs.docker.com/install/linux/docker-ce/ubuntu/#install-docker-ce
        //https://www.youtube.com/watch?v=EDaN2293rQI

        private IWebDriver driver;
        [SetUp]
        public void SetUp()
        {
            var options = new FirefoxOptions();
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
            //sudo docker run -d -p 4444:4444 -v /dev/shm:/dev/shm selenium/standalone-firefox:3.11.0-dysprosium
            //Используется версия Selenium 3.11.0, опция -p описывает проброс портов, И ЭТО ОЗНАЧАЕТ, ЧТО
            //ПОРТ 4444 основной машины будет перенаправлятся на порт 4444 запущеного контейнера.
            //Т.ЕЕ я снаружи отправляю запрос на адрес "http://192.168.0.104 и если я отправлю его на порт 4444
            //то он АТОМАТИЧЕСКИ БУДЕТ ПЕРЕНАПРАВЛЕН ВНУТРЬ КОНТЕЙНЕРА.
            this.driver.Url = "https://selenium2.ru/";
        }

        //После того, как тест выполнился нужно остановить контейнер: 213124335235235 - это первые n символов кода, который появился после запуска Docker
        //sudo docker stop 213124335235235
        //И сразу же удалим его из списка т.к история нас не интересует
        //sudo docker rm 213124335235235

        //Using docker networking
        //    With this option, the hub and nodes will be created in the same network and they will recognize each other by their container name.A docker network needs to be created as a first step.

        //$ docker network create grid
        //$ docker run -d -p 4444:4444 --net grid --name selenium-hub selenium/hub:3.11.0-dysprosium
        //$ docker run -d --net grid -e HUB_HOST = selenium - hub - v / dev / shm:/dev/shm selenium/node-chrome:3.11.0-dysprosium
        //$ docker run -d --net grid -e HUB_HOST = selenium - hub - v / dev / shm:/dev/shm selenium/node-firefox:3.11.0-dysprosium

        //        When you are done using the grid AND THE CONTAINVER HAVE EXITED (НУЖНО САМОМУ ПОСТОПАТЬ КАЖДЫЙ КОНТЕЙНЕР ОТДЕЛЬНО), the network can be removed with the following command:

        //# Remove all unused networks
        //        $ docker network prune
        //# OR 
        //# Removes the grid network
        //        $ docker network rm grid

        //СИДЕЛ ПОЛЧАСА ПОКА ПОЛУЧИЛОСЬ: запускаем диспетчер на локальной машине, а контенер на виртуальной Linux
        //1. java -jar selenium-server-standalone-3.11.0 - role hub 
        //2. НУЖНО УКАЗАТЬ АДРЕС HUB а так же НУЖНО УКАЗАТЬ ОБРАТНЫЙ АДРЕС ЭТОГО УЗЛА, ЧТОБЫ ДИСПЕТЧЕР ЗНАЛ КУДА НУЖНО ПЕРЕНАПРАВЛЯТЬ ЗАПРОСЫ.
        //Эти настройки устанавливаются в помощью переменных окружения.
        //-e HUB_PORT_4444_TCP_ADDR="192.168.0.102" - это АДРЕС ДИСПЕТЧЕРА, HUB_PORT_4444_TCP_PORT=4444 - порт, -e SE_OPTS="-host 192.168.0.104" - ОБРАТНЫЙ АДРЕС ЭТОГО УЗЛА, ДИСПЕТЧЕР БУДЕТ ОТПРАВЛЯТЬ ОБРАТНЫЕ ЗАПРОСЫ НА ПОРТ 5555 ПО ЭТОМУ НУЖНО ЭТОТ ПОРТ ПРОБРОСИТЬ (-p 5555:5555)
        //$ sudo docker run -d -e HUB_PORT_4444_TCP_ADDR="192.168.0.102" -e HUB_PORT_4444_TCP_PORT=4444 -e SE_OPTS="-host 192.168.0.104" -p 5555:5555 selenium/node-firefox:3.11.0-dysprosium
        // меняем 104 на 102 тут new RemoteWebDriver(new Uri("http://192.168.0.104:4444/wd/hub")

        //МОЖНО НА БАЗЕ СУЩЕСТВУЮЩИХ КОНТЕЙНЕРОВ СДЕЛАТЬ СВОИ СОБСТВЕННЫЕ, которые будут содержать те версии браузеров, которые мне нужны.
        //Сборка контейнера из конфигурационных файлов выполняется несложно, это ВСЕ ОПИСАНО В ДОКУМЕНТАЦИИ.

        //+ К этому же диспетчеру можно присоеденить несколько узлов которые будут запускать браузер IE, но там уже
        //использовать контейнер не получится, так что прийдется использовать более тяжеловесные виртуальные машины
        //или даже физические машины.
    }
}
