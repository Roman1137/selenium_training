﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace selenium_training._09_RemoteLaunch
{
    [TestFixture]
    class SeleniumGrid
    {
        //Selenium Grid - это распределенная сеть Selenium Server`ов

        //Более сложная ситуация:
        //Есть несколько разных машин, на которых мы хотим запускать тесты.(например потому, что тестов много или потому,
        //что нам нужно много разных конфигураций Win, Linux, MacOs , разные версии браузеров.)
        //В этом случае на каждой машине мы должны запустить Selenium Server, который будет выступать в роли агента.
        //НО ДЛЯ ТОГО, ТОБЫ ЗАПУСКАТЬ ТЕСТЫ НУЖНО ПОМНИТЬ АДРЕС КАЖДОЙ МАШИНЫ И ПРИ ЗАПУСКЕ НУЖНО ЯВНО УКАЗАТЬ АДРЕСС 
        //ТОГО СЕРВЕРА, НА КОТОРОМ ДОЛЖЕН ЗАПУСКАТЬСЯ БРАУЗЕР И РАСПРЕДЕЛЯТЬ ТЕСТЫ ПО ЭТИМ МАШИНАМ ТОЖЕ ПРИЙДЕТСЯ ВРУЧНУЮ.

        //Для того, чтобы упросить это, мы можем ввести в игру ещё один УЗЕЛ, который будет называться ДИСПЕТЧЕР или HUB.
        //Задача HUB`а - распределние работы между несколькими серверами Selenium. Все сервера, которые готовы запускать браузеры
        //регистрируются на HUB`е и HUB знает о том, где какая операционная система и где какие версии браузеров доступны.
        //Теперь ТЕСТЫ, для инифиализации драйверов отправляют ЗАПРОС НА ДИСПЕТЧЕР(HUB), он выбирает подходящую ПЛАТФОРМУ,
        //подходящую ВЕРСИЮ БРАУЗЕРА и перенапрявляет запрос туда. Там запускается драйвер и браузер. Все запросы будут 
        //идти по схеме КЛИЕНТСКАЯ БИБЛИОТЕКА => ДИСПЕТЧЕР(HUB) => Selenium Server => Driver => браузер.
        //ЕДИНСТВЕННОЕ, ЧТО НУЖНО УКАЗАТЬ при запуске автотестов- это АДРЕС ДИСПЕТЧЕРА(HUB`a).
        //Клиентская библиотека ВСЕГДА ОТПРАВЛЯЕТ ЗАПРОСЫ ПО ОДНОМУ АДРЕСУ - адресу ДИСПЕТЧЕРА(HUB`a) - а куда они потом идут- это
        //технические детали.

        //Selenium Server может выступать как и качестве ДИСПЕТЧЕРА, так и в качестве КОНЕЧНОГО УЗЛА- это просто разные режимы его работы.

        //ИЗ КОНСОЛИ ЗАПУСКАЕМ Selenium Server в роли ДИСПЕТЧЕРА(HUB`a)
        // java -jar selenium-server-standalone-3.11.0.jar -role hub

        //ИЗ КОНСОЛИ ЗАПУСКАЕМ Selenium Server в роли УЗЛА(NODE). (УЗЛУ НУЖНО УКАЗАТЬ АДРЕС ДИСПЕТЧЕРА. Он есть в консоли где мы запускали HUB)
        // java -jar selenium-server-standalone-3.11.0.jar -role node -hub http://192.168.0.102:4444/wd/hub
        //Стартуем узел, он установил соединение с диспетчером и теперь готов к работе.

        //Теперь запускаем второй узел из виртуальной машины

        //Посмотреть текущее состояние нашей распределнной сети какие углы(Nodes) готовы к работе и какие браузеры
        //на них доступны можно если заглянуть на консоль Selenium Hub (перейти по адерсу http://192.168.0.102:4444)

        //Для того, чтобы указать сколько браузеров может быть запущено на какой ноде нужно перезапустить ее указав 
        //capabilities: На ставить проблеы между browserName=firefox, и maxInstances=4
        //java -jar selenium-server-standalone-3.11.0.jar -role node -hub http://192.168.0.102:4444/wd/hub -capabilities browserName=firefox,maxInstances=4 -capabilities browserName=chrome,maxInstances=4 

        private IWebDriver driver;
        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            var capabilities = options.ToCapabilities() as DesiredCapabilities;
            capabilities.SetCapability("platform", "LINUX"); //VISTA - это то же самое что и WIN7 // или так capabilities.SetCapability("platform", "LINUX");
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
            this.driver.Url = "https://selenium2.ru/"; //Теперь если посмотреть в браузере Grid Console, то на той ноде, где сейчас запущен тест один 
            //значек браузера в котором он запущен - задизейблен- это означает, что он используется.
        }

        //КРОМЕ ТОГО, МОЖНО ЗАЙТИ НА АДРЕС УЗЛА: в браузере есть id : http://192.168.0.104:5555, OS : LINUX, например.
        //Эта консоль очень похожа на консоль отдельно стоящего сервера! Там можно увидеть сессии, которые запущены на этой ноде и остановить их.
        //А ТАК ЖЕ МОЖНО СДЕЛАТЬ СКРИНШОТ! В прошлой лекции когда я запускал просто Selenium Server эта опция была недоступна, а сейчас доступна.

        //Опций по управлению узлами ЗНАЧИТЕЛЬНО БОЛЬШЕ, ЧЕМ БЫЛО ПОКАЗАНО. Например, есть возможность усправления таймаутами(увеличивать или
        //уменьшать по жалению.) Так же МОЖНО ИСПОЛЬЗОВАТЬ КОНФИГУРАЦИОННЫЙ ФАЙЛ, чтобы описывать какие браузеры я хочу, чтобы были доступны
        //на узле, вместо того, чтобы это писать в опциях КОМАНДНОЙ СТРОКИ. Это все можно увидеть сделав: java -jar selenium-server-standalone-3.11.0.jar -role node -h
        //Увлекаться настройкой таймаутов не стоит - значения по умолчанию подобраны достаточно хорошо!

        //Самые главные опции: роль -role и адрес диспетчера -hub http://192.168.0.102:4444/wd/hub
        //Остановить NODE(УЗЕЛ) или HUB(ДИСПЕТЧЕТ) можно с помощью команды Ctr+C
    }
}
