using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_6
{
    class _006_InteractionWithInvisibleElements
    {
        public IWebDriver driver;
        public WebDriverWait wait;
        public IWebElement select; //- это выпадающий список(дропдаун)
        //Selenium не умеет кликать по невидимым элементам.
        //Что делать если столкнулся с проблемой, что нужно работать с невидимыми элементами.
        //Есть 2 варианта решения проблемы.
        //1. С помощью JavaScrip выполнить клик(или другую операцию) по невидимому элементу через програмный интерфейс.
        //2. C помощью JavaScrip сделать этот элемент видымим.

        //Если opasity 0 ,(opasity- непрозрачность) - то он прозрачный полностью (т.е по мнению Selenium - невидимый)
        //Если opasity 1 -то элемент НЕ ПРОЗРАЧНЫЙ.

        //Прозрачными делают элементы для того, чтобы скрыть их настоящий дизайн и подложить под них красивую положку.

        //работаем с элементом через програмный интерфейс
        public void Method()
        {
            driver.ExecuteJavaScript("arguments[0].selectedIdex = 3", select);  //через JavaScript выбрали 3й элемент из дропдауна.
            //но на странице все так же остался выбран первый элемент(а на самом деле выбран 3й уже).
            //чтобы это было показано, нужно отправить событие
            driver.ExecuteJavaScript("arguments[0].selectedIdex = 3; argumants[0].dispatchEvent(new Event('change'))", select);
            //перерь показано, что выбран 3й элемент
        }

        //сделаем элемент выдимым и тогда будем работать с ним через selenium
        public void Method1()
        {
            driver.ExecuteJavaScript("arguments[0].style.opacity = 1", select);
        }
    }
}
