using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._08_Windows_frames_dialogues
{
    class Frames
    {
        private IWebDriver driver;

        //Фрейм - окно, вложенное внутрь страницы. Фреймы используются тогда, когда нужно внедрить на страницу кусочек,
        //загруженный из другого сайта. Помещение контента из другого сайта во внутры фрейта повышает безопасность т.к браузер
        //изолирует фрейм от основного сожержимого страницы. КОНТЕНТ ВНУТРИ ФРЕЙМА НЕ МОЖЕТ ПОЛУЧИТЬ ДОСТУП К ОСНОВНОМУ СОДЕРЖИМОМУ СТРАНИЦЫ.
        //КАК СЛЕДСТВИЕ - если во внутрь фрейма загрузиться вредоносный код, то он не нанесет никакого ущерба основному сайту.
        //Так, что одна из основных причин использование фрейма- повышение защищенности.

        //Каждый фрейм имеет свой собственный DOM. И когда выполняется поиск элементов на странице, то внутри фрейма Selenium не ищет.
        //Для того, чтобы поисать что-то внутри фрейма, нужно туда явно переключиться.

        //Операции с фреймаи
        public void Method()
        {
            var frameElemet = driver.FindElement(By.CssSelector(".frame"));
            driver.SwitchTo().Frame(frameElemet); //лучше всего
            var frameName = "name";
            driver.SwitchTo().Frame(frameName);
            var frameIndex = 1;
            driver.SwitchTo().Frame(frameIndex);
            //для того, чтобы "выскочить" из фрейма наружу можно использовать одну из двух команд
            driver.SwitchTo().DefaultContent(); //выбрасывает на самый вверх (на основную страницу). Эту команду хотят переименовать в SwitchToTop
            driver.SwitchTo().ParentFrame(); //выбрасывает на один уровень вверх.
        }

        [Test]
        public void TryToUseFrame()
        {
            //попрактиковался и пошагать от одного фрайма к другому и поиспользовать SwitchTo().ParentFrame() и SwitchTo().DefaultContent()
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://jsbin.com/?html,output");

            var firstFrame = driver.FindElement(By.CssSelector("iframe"));
            driver.SwitchTo().Frame(firstFrame); //проверено - работает
            //swithc to frame by name
            driver.SwitchTo().Frame("<proxy>"); //проверено - работает
            //swithc to frame by index
            driver.SwitchTo().Frame(0); //проверено - работает

            var secondFrame = driver.FindElement(By.CssSelector("#sandbox-wrapper iframe[sandbox]"));
            driver.SwitchTo().Frame(secondFrame);

            driver.SwitchTo().ParentFrame();
            driver.SwitchTo().DefaultContent();
        }

        [TearDown]
        public void Stop()
        {
            driver.Dispose();
        }
    }
}
