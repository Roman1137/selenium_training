using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_training.Lection_3
{
    public class CSSselectors
    {
        // ul# menu li.active   - проблел - это прыжек

        //1. CSS можно искать используя $$ либо $
        // $$ возвращает все элементы, а $ - первый

        //ОЧЕНЬ ВАЖНО!!! В скобких [] нужно указывать точное значение! 
        //например, если class = span3 block-region  мы укажем [class=span3], то оно НИЧЕГО НЕ НАЙДЕТ!
        //нужно указать .span3.block-region и только тогда найдет

        //Проверка значения атрибута
        // ["checked"]  в CRM сработало $$(":checked")   где checked , это атрибут, а не значение атрибута.

        //Частичное совпадение с заданным текстом
        //$$("[class*=hea]")  значение атрибута class СОДЕРЖИТ текст hea ('такие скобки') можно не указывать в CSS.
        //$$("[class^=rt]")  значение атрибута class НАЧИНАЕТСЯ с текста rt
        //$$("[class$=er]")  значение атрибута class ЗАКАНЧИВАЕТСЯ текстом er

        //Комбинация условий
        //label.error.fatal - ищем элемент с тэгом label и классами error и fatal одновременно.
        //label.error[name=roma] и так можно.
        //$$(".link[data-test=footer-tomorrow]")

        //Отрицание условий
        //label:not(.error) - ищем элемент с тегом label НО БЕЗ класса error
        //div:not([class$=task]) - ищем элемент с тегом div но у которых нету класса, заканчивающегося на task
        //a:not([href^=http]) ищем элемент с тэгом a но у которых нету атрибута href, начинающегося с http

        //Движение по дереву
        //!!!ОЧЕНЬ ВАЖНО!!! Поиск работает не так что мы нашли первый элемент п первой части локатора,
        //а потом ищем второй элемент по второй части локатора. 
        //CSS ЗАДАЕТ НЕ АЛГОРИТМ ПОИСКА, А КРИТЕРИЙ!

        //div#main p  - p находится внутри div#main
        //div#main > p  - p находится непосредственно (на 1 уровень ниже) внутри div#main
        //div#main + p  - p находится сразу за элементом div#main (они равноправные)
        //div#main ~ p  - p находится за элементом div#main(они равноправны как в примере выше) НО между ними могут быть ещё элементы

        //div#main p:first-child
        //div#main p:last-child
        //div#main p:nth-child(2)

        //div#main p:nth-of-type(2)
        //Разница между nth-child nth-of-type то, что для nth-child главное, элемент, который мы ищем был (n-ый) в списке ВСЕХ child`ов
        //а для nth-of-type то, что элемент, который мы ищем будет выбираться только из элементов определленного типа => div#main p:nth-of-type(2) => типа p
    }
}
