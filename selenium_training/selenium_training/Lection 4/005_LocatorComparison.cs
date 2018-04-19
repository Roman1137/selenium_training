using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_3
{
    class LocatorComparison
    {
        //Частные случаи CSS- селекторов
        //By.tagName("div")           By.CssSelector("div")
        //By.Id("main")               By.CssSelector("#main")
        //By.ClassName("error")       By.CssSelector(".error")
        //Суть поиск практически по любому критерию преобразуется в поиск по CSS-селектору с помощью клиентской библиотеки.

        //Смотря как сравнивать...
        //Мощность языка - Xpath (как правило, мощность является невостребованной!)
        //Краткость и понятность - CSS
        //Поддержка в браузерах - CSS  IE 6,7,8 - там XPath не работает
        //Скорость локаторов - CSS c минимальным преймеществом

        //Chrome CSS - 5.37ms   Xpath - 5.93ms
        //IE CSS - 470ms   Xpath - 248ms

    }
}
