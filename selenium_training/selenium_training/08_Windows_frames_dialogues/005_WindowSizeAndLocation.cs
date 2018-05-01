using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace selenium_training._08_Windows_frames_dialogues
{
    class WindowSizeAndLocation
    {
        //https://peter.sh/experiments/chromium-command-line-switches/
        private IWebDriver driver;

        public void ExplainMethod()
        {
            var position = this.driver.Manage().Window.Position; //Возвращает положение левого верхнего угла окна
            this.driver.Manage().Window.Position = new Point(50, 50); //Позволяет установить положение левого верхнего угла окна

            var size = driver.Manage().Window.Size; //Возвращает размер ОКНА (а не видимой части страницы), 
            //не учитывается размер тулбара, скрол бара, елементов, которые обрамляют страницу.
            this.driver.Manage().Window.Size = new Size(500, 500);

            this.driver.Manage().Window.FullScreen();
            this.driver.Manage().Window.Maximize();
            this.driver.Manage().Window.Minimize();
        }

        [Test]
        public void WindowSizeAndLocationMethod()
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized ");
            this.driver = new ChromeDriver(options);
            

            var position = this.driver.Manage().Window.Position; // перетащив окно - взяв это значение еще раз - оно изменилось
            this.driver.Manage().Window.Position = new Point(500, 500); // 

            var size = driver.Manage().Window.Size; // меняешь вручную и берешь значение - оно меняется тоже), 
            //не учитывается размер тулбара, скрол бара, елементов, которые обрамляют страницу.
            this.driver.Manage().Window.Size = new Size(500, 500);

            this.driver.Manage().Window.FullScreen();  //не работает , но можно использовать то, что ниже!
            //var options = new ChromeOptions();
            //options.AddArgument("start-fullscreen");
            //this.driver = new ChromeDriver(options);
            this.driver.Manage().Window.Maximize();
            //this.driver.Manage().Window.Minimize();  // не работает

            //Что нужно проверять, меняя размер окна для проверки на разных девайсах?
            //Нужно выполнить поиск элементов и проверять их свойство: видимые ли они или нет?
        }

    }
}
