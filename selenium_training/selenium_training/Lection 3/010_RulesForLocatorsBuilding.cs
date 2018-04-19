﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_training.Lection_3
{
    class RulesForLocatorsBuilding
    {
        //Самое главное требование - устойчивость к изенениям вёрстки.
        //1.Максимально точные критерии выбора.(ровно один элемент и не более)
        //2.Как можно меньше порядковых номеров. Если разработчики добавлят +1 элемент к списку, то тест будет использовать
        //не тот элемент, что нужно. Принцип FAIL FAST не работает! А сколько времени прийдется потратить, чтобы понять в чем причина...
        //3.Привязка к ближайшему уникальному элементу.
        //4.Минимум прыжков по DOM
    }
}
