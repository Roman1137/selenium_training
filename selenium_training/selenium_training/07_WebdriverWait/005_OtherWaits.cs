using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_7
{
    class OtherWaits
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private IWebElement element;


        public void Method()
        {
            wait.Until(ExpectedConditions.TitleIs("webdriver - Поиск в Google"));
            wait.Until(ExpectedConditions.TitleContains("webdriver - Поиск в"));
            wait.Until(ExpectedConditions.UrlContains("login.php"));
            wait.Until(ExpectedConditions.UrlToBe("http://pagination.js.org/"));
            var regex = @"\d";
            wait.Until(ExpectedConditions.UrlMatches(regex));
            wait.Until(ExpectedConditions.AlertIsPresent());
            //В С# я не нашел  wait.Until(ExpectedConditions.NumberOfWindowsToBe());
            //нужно проверять то, что у элемента есть какой-то класс, вместо того, чтобы проверять стиль элемента.
            //как правило, разработчики не меняют стили напрямую, они присваивают классы.
            wait.Until(x => x.FindElement(By.CssSelector("locator")).GetAttribute("class").Contains("error"));
            wait.Until(ExpectedConditions.TextToBePresentInElement(element, "text"));
            wait.Until(ExpectedConditions.ElementToBeSelected(element, false));
            wait.Until(ExpectedConditions.ElementToBeClickable(element));
            //ElementToBeClickable - не  соответствует своему названию. Сдесь проверяется то, что
            //елемент 1.Видимый, 2.Не disabled. И конечно, здесь нет никаких проверок, что эта кнопка НЕ ЗАКРЫТА никаким другим 
            //элементом а если она прозрачная, то она будет считаться невидимой. Так, что название этого метода просто напросто врет.
            //По-настоящему ИНТЕРАКТИВНОСТЬ ОНО НЕ ПРОВЕРЯЕТ.
            //условие количества элементов с локатором
            var elements = driver.FindElements(By.CssSelector("locator"));
            wait.Until(x => elements.Count == 10);
            //но здесь мы возвращаем коллекцию элементов.
            var returnedElements = wait.Until(x =>
            {
                 var elements2 = x.FindElements(By.CssSelector("locaotr"));
                 return elements2.Count == 10 ? elements2 : null;
            });
            //хз что это...
            var wait2 = new DefaultWait<IWebElement>(element);
        }
    }
}
