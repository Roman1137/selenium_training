using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._11_PageObjects_and_other_design_patterns.HomeTask_PageObjects
{
    public class Application
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private static Random random;

        public CartPage cartPage;
        public MainPage mainPage;
        public ProductPage productPage;

        public Application()
        {
            this.driver = new ChromeDriver();
            this.cartPage = new CartPage(driver);
            this.mainPage = new MainPage(driver);
            this.productPage = new ProductPage(driver);
        }

        public void Quit()
        {
            this.driver.Close();
            this.driver.Quit();
            this.driver.Dispose();
            this.driver = null;
        }
    }
}
