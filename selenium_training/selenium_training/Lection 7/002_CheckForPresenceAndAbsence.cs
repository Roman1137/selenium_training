using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_7
{
    class CheckForPresenceAndAbsence
    {
        private IWebDriver driver;

        //Проверка НАЛИЧИЯ
        public bool IsElementPresent(IWebDriver driver, By locator)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                return driver.FindElements(locator).Count > 0;
            }
            finally
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            }
        }

        //Зависает проверка ОТСУТСТВИЯ элемента
        //Нужно сделать так
        public bool IsElementNotPresent(IWebDriver driver, By locator)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                return driver.FindElements(locator).Count == 0;
            }
            finally
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            }
        }

        //Проверяйте наличие, а не отсутствие!
        //Например: мы выполнили логин. Вместо того, чтобы проверять ОТСУТСТВИЕ формы логина, нужно проверять
        //НАЛИЧИЕ элемента, который есть на странице залогиненого пользователя. 

        //Лучше избегать манипуляций с неявными ожиданиями!
    }
}
