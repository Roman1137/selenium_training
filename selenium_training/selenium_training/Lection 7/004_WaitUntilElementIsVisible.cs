using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_7
{
    class WaitUntilElementIsVisible
    {
        private IWebDriver driver;
        private IWebElement element;
        private WebDriverWait wait;
        //Большинство драйверов не могут выполнить действия с элементом, если он невиден.
        //Вообще, они должны бы проверять условие ИНТЕРАКТИВНОСТИ,а не видимости, но для упрощения пока так.

        public void Method()
        {
            string locator = "q2345";
            //ждем пока элемент станет видимым
            //ВЫВОД - если метод из класс ExpectedConditions принимает ЛОКАТОР- то он вернет элемент
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(locator)));
            var listOfElements = driver.FindElements(By.CssSelector(locator));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(listOfElements));

            //ждем пока элемент станет невидимым
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(locator)));
        }

        //НУЖНО знать когда проверять видимость элемента, а когда - его присутствие на странице.
        //Бывает так, то элемент есть всегда, просто невиден.(например fancybox-wrap и fancybox-overlay
        //элементы из учебного приложения есть всегда, просто когда-то видимые, а когда-то нет).
        //ИТОГ: в этом модуле я научился проверять видимый элемент или нет
    }
}
