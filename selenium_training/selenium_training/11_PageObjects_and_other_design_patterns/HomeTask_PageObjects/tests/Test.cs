using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._11_PageObjects_and_other_design_patterns.HomeTask_PageObjects
{
    [TestFixture]
    public class Test : TestBase
    {  //вот тут выбирвается метод/свойство, и те значение, котороые возвращаются им будет передавно в тестовый метод
        [Test, TestCaseSource(typeof(DataProvider), nameof(DataProvider.AmountOfProducts))]
        public void VerifyCartWorkScenario(ProductModel model)
        {
            for (int i = 0; i < model.Amount; i++)
            {
                app.mainPage.GoToMainPage();
                app.mainPage.SelectRandomProduct();
                app.productPage.AddProductToCard();
            }
            app.productPage.GoToCartPage();
            app.cartPage.RemoverAllProductsFromCart();
        }
    }
}
