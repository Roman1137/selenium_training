using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_7
{
    class WaitForElementToBePresent
    {
        private IWebDriver driver;
        //Неявное ожидание.
        //driver.Manage().Timeouts().ImplicitWait= TimeSpan.FromSeconds(5);
        //чтобы отключить нужно выполнить driver.Manage().Timeouts().ImplicitWait= TimeSpan.FromSeconds(0);

        //FindElement ждёт пока элемент появится
        //FindElements ждет пока появится ХОТЯ БЫ ОДИН элемент - т.е будет возвращать список как только появится первый элемент
        //Если по истечению таймаута ничего найдено не будет FindElement - NoSuchElementException, FindElements - пустой список.

        public void Method(string locator)
        {
            for (int count = 0; ; count++)
            {
                if (count >= 30)
                    throw new TimeoutException();
                try
                {
                    driver.FindElement(By.CssSelector(locator));
                    break;
                }
                catch (NoSuchElementException e)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void SetUp(string locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //можно вернуть элементы
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector(locator)));
            IList<IWebElement> element2 = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(locator)));
        }

        //Явные и неявные ожидания.
        //Явные                                        Неявные
        //На стороне клиента   интервал 500мс          На стороне браузера(драйвера) интервал 100мс
        //Ждать чего угодно                            Ожидание появления в DOM
        //Надо писать явно                             Работают автоматически
        //TimeoutException                             NoSuchElementException
        //Много сетевых запросов                       Один сетевой запрос
        //ответы на эти запросы могут быть долгими т.к
        //шлются скриншоты и могут долго идти
        //если тесты ранятся где-то в "облаках"
    }
}
