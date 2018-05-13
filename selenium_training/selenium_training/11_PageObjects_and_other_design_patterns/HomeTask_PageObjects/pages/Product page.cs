using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._11_PageObjects_and_other_design_patterns.HomeTask_PageObjects
{
    public class ProductPage : Page
    {
        public ProductPage(IWebDriver driver) : base(driver) { }

        public void GoToCartPage()
        {
            this.LinkToCartElement.Click();
        }

        public void AddProductToCard()
        {
            wait.Until(x => AddToCartButtonElement.Displayed);
            if (this.SizeDropDownElements.Count != 0)
            {
                SelectRandomValueFromDropDown();
            }
            var cartCountElementText = CartProductCounterElement.Text;
            var countOfProductInCart = Convert.ToInt32(cartCountElementText);
            this.AddToCartButtonElement.Click();
            wait.Until(x => this.CartProductCounterElement.Text == (countOfProductInCart + 1).ToString());
        }

        private void SelectRandomValueFromDropDown()
        {
            var sizeDropDown = new SelectElement(this.SizeDropDownElements.First());
            var optionsToSelect = sizeDropDown.Options;
            sizeDropDown.SelectByText(optionsToSelect.Last().Text);
        }

        public string AddToCartButtonLocator => "[name=add_cart_product]";
        public string CartProductCounterLocator => "#cart .quantity";
        public string LinkToCartLocator => "#cart .link";
        public string SizeDropDownLocator => "[name='options[Size]']";
        public IWebElement AddToCartButtonElement => driver.FindElement(By.CssSelector(AddToCartButtonLocator));
        public IWebElement CartProductCounterElement => driver.FindElement(By.CssSelector(CartProductCounterLocator));
        public IWebElement LinkToCartElement => driver.FindElement(By.CssSelector(LinkToCartLocator));
        public IList<IWebElement> SizeDropDownElements => driver.FindElements(By.CssSelector(SizeDropDownLocator));
    }
}
