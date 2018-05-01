using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace selenium_training._08_Windows_frames_dialogues
{
    class CloseQuitDispose
    {
        //Команда Close закрывает текущее окно, а если это окно последнее, то она и 
        //останавливает и браузер тоже НО НЕ ОСТАНАВЛИВАЕТ ВСПОМАГАТЕЛЬНЫЙ ФАЙЛ ДРАЙВЕРА.
        //Команда Quit закрыаает все окна , останавливает браузер и останавливает вспомагательный файл драйвера.
        
        //Перед запуском браузеров Chrome и Firefox в директории для временных файлов создается пустой профиль, который как раз таки 
        //и используется при работе браузера и после того, как Driver завершает работу, эту директорию, которая содержит профиль нужно удалить.
        //Close ЭТОГО НЕ ДЕЛАЕТ. Quit - делает.
        private IWebDriver driver;

        [Test]
        public void StopbBrowser()
        {
            driver = new ChromeDriver();

            driver.Close();
            driver.Quit();
            driver.Dispose();
        }
    }
}
