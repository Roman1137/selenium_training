using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._11_PageObjects_and_other_design_patterns.HomeTask_PageObjects
{
    public class CartPage : Page
    {
        public CartPage(IWebDriver driver) : base(driver) { }

        public void RemoverAllProductsFromCart()
        {
            wait.Until(x => this.RemoveFromCartButtonElement.Displayed);
            for (int i = 0; TableRowElement.Count != 0; i++)
            {
                RemoveProductFromCart();
            }
        }

        private void RemoveProductFromCart()
        {
            var productRows = TableRowElement;
            var removeButton = wait.Until(x => x.FindElement(By.CssSelector(RemoveFromCartButtonLocator)));
            removeButton.Click();
            foreach (var row in productRows)
            {
                wait.Until(ExpectedConditions.StalenessOf(row));
            }
        }

        public string RemoveFromCartButtonLocator => "[name=remove_cart_item]";
        public string TableRowLocator => "#order_confirmation-wrapper tr";
        public IWebElement RemoveFromCartButtonElement => driver.FindElement(By.CssSelector(RemoveFromCartButtonLocator));
        public IList<IWebElement> TableRowElement => driver.FindElements(By.CssSelector(TableRowLocator));

    }
}
