using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._11_PageObjects_and_other_design_patterns.HomeTask_PageObjects
{
    public class MainPage:Page
    {
        public MainPage(IWebDriver driver) : base(driver) { }
        public void SelectRandomProduct()
        {
            var products = this.MostPopularProductElements;
            var productToSelect = products.First();
            productToSelect.Click();
        }

        public void GoToMainPage()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector(MostPopularTitleLocator), "Most Popular"));
        }

        public string MostPopularTitleLocator => "#box-most-popular";
        public string MostPopularProductLocator => string.Concat(MostPopularTitleLocator, " li[class*=product]");
        public IList<IWebElement> MostPopularProductElements => driver.FindElements(By.CssSelector(MostPopularProductLocator));
    }
}
