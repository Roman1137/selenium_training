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
    class _002_Locators
    {
        //IWebElement element = driver.FindElement(By.locator) 
        //находит первый элемент и выдает NoElementRefferanceException когда не находит элемент

        //IReadOnlyCollection<IWebElement> elements = driver.FindElements(By.locator);
        //находит коллекцию элементов. если нету элементов с заданным локатором, то возвращает пустую коллекцию.
        //если есть неявное ожидание, то driver.FindElements найдет только 1 элемент и вернет его.


        //driver.FindElement(By.ClassName());
        //driver.FindElement(By.CssSelector());
        //driver.FindElement(By.Id());
        //driver.FindElement(By.Name());
        //driver.FindElement(By.TagName());
        //driver.FindElement(By.XPath());

        ////эти методы немного корявые т.к ищут все линки, а потом вычисляет текст у каждой линки и сравнивает его со строкой, по этому работают долго.
        //driver.FindElement(By.PartialLinkText());
        //driver.FindElement(By.LinkText());

    }
}
