using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace selenium_training._11_PageObjects_and_other_design_patterns
{
    class PageObjects_in_theory
    {
        //Marting Fowler - 2004 "Window Driver", но это не совсем то же самое, что и PageObjects
        //PageObjects очень популярен в кругах людей, использующих Selenium.
        //Краткое, но достаточно полное описание сути этого шаблона проектирования:
        //http://martinfowler.com/bliki/PageObject.html

        //Сайт книги "xUnit Test Patterns":
        //http://xunitpatterns.com/

        //1. Первая и самая главная мысль - не нужно в тестх работать напрямую с пользовательским интерфейсом.
        //При разработке тестов следует применять "ПРИНЦИП РАЗДЕЛЕНИЯ ОТВЕТСТВЕННОСТИ" - в ТЕСТАХ описывается поведение
        //спецификация, а в специальных вспомагательных функциях описано как именно это поведение реализуется
        //в пользовательском интерфейсе.
        //В ИТОГЕ - тесты оказываются высокоуровневые, т.к они описывают логику поведения, а вспомагательные
        //методы описывают конкретные локаторы элементов, как с ними взаимодействавать, куда кликать, чего ожидать
        //и прочие технические детали.
        //Главная суть - разделение ответственности, но так же возникает ещё один полезные вспомагательный эффект- 
        //один низкоуровневый метод может использоваться в разных местах в разных высокоуровневых тестовых методах.
        //Таким образом достигается более ВЫСОКИЙ УРОВЕНЬ ПОВТОРНОГО ИСПОЛЬЗОВАНИЯ.

        public IWebDriver driver;
        public string BaseUrl = "https://selenium2.com";

        //Низкоуровневый код - ничего не понятно.
        [Test]
        public void LowLevelScenario()
        {
            driver.Url = BaseUrl;
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("usernamre")).SendKeys("admin");
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("sumbit")).Click();
            driver.FindElement(By.LinkText("Log out")).Click();
        }
        //высокоуровневый код
        [Test]
        public void HighLeveScenario()
        {
            Login("admin", "admin"); //внутрь этих функций мы прячем технические детали.
            Logout(); //внутрь этих функций мы прячем технические детали.
        }
        public void Login(string name, string password)
        {
            driver.Url = BaseUrl;
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("usernamre")).SendKeys(name);
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("sumbit")).Click();
        }
        public void Logout()
        {
            driver.FindElement(By.LinkText("Log out")).Click();
        }
        //перенесем функции в отдельный класс и теперь:
        public Application app = new Application();
        [Test]
        public void HigLevelScenarionWithPageObject()
        {
            app.Login("admin","admin");
            app.Logout();
            //Если все методы впихнуть в app - то он будет очень большим
            //по этому его нужно декомпозировать - ДЛЯ КАЖДОЙ СТАНИЦЫ(КОНСТРОЛА)- СВОЙ КЛИААС..
        }
        //а теперь методы разбросаны по страницам
        [Test]
        public void HigLevelScenarionWithPageObjects()
        {
            app.loginPage.Login("admin", "admin");
            app.innerPage.Logout();
        }

        //ВАЖНО!
        //Не смотря на то, что здесь упоминается слово Page - никакого отношения к страницам эти не имеет.
        //Мы можем выделять эти вспомагательные обьекты так, как нам удобно: можно сделать один обьект на 
        //несколько страниц или наоборот - несколько обьектов на страницу.

        //Можно часто втретить разделение на на 2 слоя, а на 3 слоя - выделить 3 разных зоны ответственности.
        //Тестовые сценарии, которые описывают логику (можно считать, что это спецификация поведения системы).
        //Операции уровня бизнес логики (например метод Login)
        //Операции уровня пользовательского интерфейса. (а на этом уровне будет заполнение полей)
        //На 3м уровне так же можно (и нужнно) выделять отдельно локаторы.

        //В Java и C# есть так званые PageFactory (но они уже перенесены в другой проект)
        [FindsBy(How = How.Name, Using = "address1")]
        internal IWebElement AddressInput;
        //так можно найти много элементов
        [FindsBy(How = How.Name, Using = "address1")]
        internal IList<IWebElement> AddressInput2;
        //Эта штука ничем не отличается от 
        public IWebElement AddressInput1 => driver.FindElement(By.Name("address1"));

        //ВАЖНО:
        //Нужно выбрать реализацию PageObjects в таким стиле, как нам удобно.
        //Если сценарии простые и их мало (меньше 10) - то вообще нет смысла делить на слои
        //и отдельять бизнес логику от сценариев. 
        //Второй этап - это отделение бизнес логики от операций пользовательского интерфейса- 
        //бизнес логика - в тесте, а взаимодействие в пользовательским интерфейсом - в других классах.
        //И так далее - чем сложнее сценарии, тем больше слоев, но чем больше слоев - тем сложнее потом в них разбираться.
    }
}
