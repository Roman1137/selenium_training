using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_7
{
    [TestFixture()]
    public class HomeTask
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private static Random random;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }

        [Test]
        public void VerifyCartWorkScenario()
        {
            const int productsToAdd = 3;
            AddProductsToCart(productsToAdd);
            GoToCartPage();
            RemoverAllProductsFromCart();
        }

        public void AddProductsToCart(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GoToMainPage();
                SelectRandomProduct();
                AddProductToCard();
            }
        }

        public void RemoverAllProductsFromCart()
        {
            for (int i = 0; TableRowElement.Count != 0; i++)
            {
                RemoveProductFromCart();
            }
        }

        private void RemoveProductFromCart()
        {
            var productRows = TableRowElement;
            var removeButton = wait.Until(x=>x.FindElement(By.CssSelector(RemoveFromCartButtonLocator)));
            removeButton.Click();
            foreach (var row in productRows)
            {
                wait.Until(ExpectedConditions.StalenessOf(row));
            }
        }

        private void GoToCartPage()
        {
            this.LinkToCartElement.Click();
            wait.Until(x => this.RemoveFromCartButtonElement.Displayed);
        }

        private void AddProductToCard()
        {
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

        private void SelectRandomProduct()
        {
            var products = this.MostPopularProductElements;
            var productToSelect = products.First();
            productToSelect.Click();
            wait.Until(x => this.AddToCartButtonElement.Displayed);
        }

        public void GoToMainPage()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#box-most-popular"), "Most Popular"));
        }

        //Main page
        public string MostPopularProductLocator => "#box-most-popular li[class*=product]";
        public IList<IWebElement> MostPopularProductElements => driver.FindElements(By.CssSelector(MostPopularProductLocator));

        //Product page
        public string AddToCartButtonLocator => "[name=add_cart_product]";
        public string CartProductCounterLocator => "#cart .quantity";
        public string LinkToCartLocator => "#cart .link";
        public string SizeDropDownLocator => "[name='options[Size]']";
        public IWebElement AddToCartButtonElement => driver.FindElement(By.CssSelector(AddToCartButtonLocator));
        public IWebElement CartProductCounterElement => driver.FindElement(By.CssSelector(CartProductCounterLocator));
        public IWebElement LinkToCartElement => driver.FindElement(By.CssSelector(LinkToCartLocator));
        public IList<IWebElement> SizeDropDownElements => driver.FindElements(By.CssSelector(SizeDropDownLocator));

        //Cart page
        public string RemoveFromCartButtonLocator => "[name=remove_cart_item]";
        public string TableRowLocator => "#order_confirmation-wrapper tr";
        public IWebElement RemoveFromCartButtonElement => driver.FindElement(By.CssSelector(RemoveFromCartButtonLocator));
        public IList<IWebElement> TableRowElement => driver.FindElements(By.CssSelector(TableRowLocator));
    }
}
