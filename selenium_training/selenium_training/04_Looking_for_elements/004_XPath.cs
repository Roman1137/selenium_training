using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_training.Lection_4
{
    class XPath
    {
        //Структура XPath запроса
        //   //ul[@id='name']/li[contains(@class,'active')]
        // серия прыжков по DOM`y
        // критерий но основе тега и атрибутов
        //[checked]    //*[@checked]   - // означает, что нужно искать по всему DOM`у;* - это вместо тега. нужно указывать или ее или сам тег
        //[name=email]  //*[@name='email']
        //[title*=name]  //*[contains(@title,'name')]
        //[src^=http]  //*[starts-with(@src,'http')]
        //[src$=pdf]  -- нету такого в XPath

        //В XPath нужно обязательно указывать как будешь двигаться по дереву:
        // // означает, что элемент будет искаться везде.
        // * - это ЛЮБОЙ ТЭГ
        // @ ставиться всегда перед названием атрибута
        // '' кавычки обязательны, а в CSS - нет. //*[@name='email']

        // "label"   "//label"
        // ".error"   "//*[contains(@class,error)]"
        // "label.error"   "//label[contains(@class,error)]"
        // "label.error.fatal"   "//label[contains(@class,error) and contains(@class,'fatal')]"
        // "label.error[for=email]"   "//label[contains(@class,error) and @for='email']"
        //почему conatins? потому, что у элемента может быть несколько классов. 
        //Например:class=span3 block-region .Нужно использовать @class='value' только если уверен в том, что в 
        // в '' кавысчках ты укажешь весь класс.

        //Движение по дереву.
        // //div[@id='main']//p    // это как пробел в CSS  к примеру  ul#menu li.active
        // //div[@id='main']/p   .  означает спуститься на один уровень ниже
        // //div[@id='main']/p[2]  div#main>p:nth-of-type(2)
        //В XPath НЕТ АНАЛОГОВ nth-of-child, first-child, last-child!

        //Движение в любом направлении
        //   //input[@id='search']/../input[@type='button']    /../ - означает, что мы поднимаемся вверх по элементу

        //Поиск по тексту
        //  //a[contains(.,'Edit')] - ищем элемент у которого есть текст Edit
        //  //*[.='Test'] - ищем элемент у которого есть ТОЛЬКО текст Test

        //Подзапросы
        // //form[.//input[@name='password']]  - ищем форму, внутри которой есть поле ввода с именем password
        //   .  выше означает, что запрос выполняется отноительно текущего элемента
    }
}
