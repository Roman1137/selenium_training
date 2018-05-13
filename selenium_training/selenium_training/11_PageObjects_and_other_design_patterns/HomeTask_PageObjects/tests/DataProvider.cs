using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_training._11_PageObjects_and_other_design_patterns.HomeTask_PageObjects
{
    internal class DataProvider
    {
        public static IEnumerable AmountOfProducts
        {
            get { yield return new ProductModel{ Amount = 3 }; }
        }
    }
}
