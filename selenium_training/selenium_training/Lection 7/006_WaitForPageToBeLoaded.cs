using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_7
{
    class WaitForPageToBeLoaded
    {
        private IWebDriver driver;
        //Что означает "окончание загрузки страницы"? -- http://barancev.github.io/page-loading-complete/
        //В процессе обработки страницы браузер меняет специальное свойство document.readyState, которое как раз
        //и содержит информацию о текущем этапе загрузки:
        //- LOADING означает, что страница ещё загружается
        //- INTERACTIVE означает, что основное содержимое страницы загрузилось и отрисовалось, пользователь
        //уже может с ней взаимодействовать, но ещё продолжается загрузка дополнительных ресурсов,
        //- COMPLETE означает, что все дополнительные ресурсы тоже загружены.
        //Selenium использует именно свойство document.readyState для определения загрузки страницы.
        //По - умолчанию Selenium ждет пока свойство document.readyState будет COMPLETE(но это можно поменять)
        //https://www.reg.ru/choose/domain/?domains=software - пример где после document.readyState=COMPLETE все еще идут запросы.
        //ВЫВОД - недостаточно ждать конца загрузки страницы -> нужно ждать пока появится элемент с которым я буду работать.


        //Как Selenium ожидает окончания загрузки страницы? -- http://barancev.github.io/how-selenium-waits-for-page-to-load/
        //Когда Selenium ждет? Selenium ждет не ПОСЛЕ команды, а ПЕРЕД командой.
        //Другими словами перед выполнением каждой команды Selenium ждет пока свойство document.readyState будет COMPLETE.

        //Что делать, если страница загружается слишком долго? -- http://barancev.github.io/slow-loading-pages/
        //Пример с картинкой на 7мб - страница грузится доооолго. Т.е свойство document.readyState никак не может стать
        //COMPLETE - оно продолжает оставаться в состоянии INTERACTIVE.
        //В таком случае есть два способа подсказать Selenium, чтобы он не ждал такой длинной загрузки.
        //ПЛОХОЙ СПОСОБ
        public void SetUp()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(1);
            //Ну, раз уж мы отобрали у Selenium и взяли на себя ответственность за ожидание 
            //загрузки страницы, надо брать ответственность и за “выгрузку” страницы тоже. То есть 
            //перед ожиданием появления элемента, который должен найтись на следующей странице, нужно 
            //сначала подождать, пока исчезнет элемент, находящийся на текущей странице. Например, 
            //исчезнет та самая кнопка, по которой кликали
            // открываем сайт, но ждём недолго
            try
            {
                driver.Url = "http://www.sazehgostar.com/SitePages/HomePage.aspx";
            }
            catch (TimeoutException ignore)
            {
            }
            // ждём появления кнопки на "недозагруженной" странице
            IWebElement button = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("en")));
            // кликаем
            try
            {
                button.Click();
            }
            catch (TimeoutException ignore)
            {
            }
            // ждём исчезновения кнопки, то есть "выгрузки" страницы
            wait.Until(ExpectedConditions.StalenessOf(button));
            // ждём загрузки следующей страницы
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("en")));
        }

        //Есть другой - более чесный способ, который называется ИЗМЕНЕНИЕ СТРАТЕГИИ ОЖИДАНИЯ ЗАГРУЗКИ.
        //для этого нужно использовать capability с названием pageloadStrategy
        //Значения этого capability:
        //1. normal - установлено по умолчанию - ждать, пока свойство document.readyState приймет значение COMPLETE
        //2. eager - ждать, пока свойство  document.readyState приймет значение INTERACTIVE
        //3. none - вообще не ждать.
        //То же самое - нужно брать ответственность за ВЫГРУЗКУ страницы. Находим элемент, запоминаем его,
        //используем его и ждем пока он исчезнет (StalenessOf), а уже после этого начинаем ждать какой-то
        //элемент на новой странице.
        public void SetUp2()
        {
            // инициализируем драйвер
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.PageLoadStrategy, "eager");
            var driver = new FirefoxDriver(capabilities);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            // открываем сайт
            driver.Url = ("http://www.sazehgostar.com/SitePages/HomePage.aspx");
            // ждём появления кнопки на "недозагруженной" странице
            var button = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("en")));
            // кликаем
            button.Click();
            // ждём исчезновения кнопки, то есть "выгрузки" страницы
            wait.Until(ExpectedConditions.StalenessOf(button));
            // ждём загрузки следующей страницы
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("menu")));
        }
        //Capabilities нельзя менять по ходу рана теста. Если выставил eager , а хочу normal - нужно перезапустить браузер.
    }
}
