using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_6
{
    class WorkWithComplicatedElements
    {
        public IWebDriver driver;
        public WebDriverWait wait;
        public IWebElement element;

        public void SelectFromDropDow()
        {
            var dropDown = new SelectElement(element);
            var allSelectedOption = dropDown.AllSelectedOptions;
            var selectedOption = dropDown.SelectedOption;
            var isMultiple = dropDown.IsMultiple;
            var listOfAllOption = dropDown.Options;
            var wrappedElement = dropDown.WrappedElement;

            dropDown.DeselectAll();
            dropDown.DeselectByIndex(50);
            dropDown.DeselectByText("roma");
            dropDown.DeselectByValue("12345");
            dropDown.SelectByIndex(60);
            dropDown.SelectByText("Roma");
            dropDown.SelectByValue("hgfdsa");
        }

        //Так же есть элементы, которые сложны в использовании Selenium - например календарь или слайдер или нестандартные выпадающие меню -
        //в том числе многоуровненые.
        //Чаще всего такие элементы предоставляют програмный интерыейс на языке програмирования JavaScript.
        //Вместо того, чтобы кликать мышкой кучу раз и делать сложные действия, можно выполнить операцию
        //driver.ExecuteJavaScript и обратиться к програмному интерфейсу сложных элементов.

        public void SetDatepicker(IWebDriver driver, string cssSelector, string date)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until<bool>(
                d => driver.FindElement(By.CssSelector(cssSelector)).Displayed);
            (driver as IJavaScriptExecutor).ExecuteScript(
                String.Format("$('{0}').datepicker('setDate', '{1}')", cssSelector, date));
        }
    }
}
