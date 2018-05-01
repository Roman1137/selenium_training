using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_5
{
    [TestFixture]
    class HomeTask_Part2
    {
        public IWebDriver driver;
        public WebDriverWait wait;

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }

        [Test]
        [TestCase("Chrome")]
        [TestCase("FireFox")]
        [TestCase("InternetExplorer")]
        public void VerifyProductPageInfo(string browser)
        {
            switch (browser)
            {
                case "Chrome":
                    driver = new ChromeDriver();
                    break;
                case "FireFox":
                    driver = new FirefoxDriver();
                    break;
                case "InternetExplorer":
                    driver = new InternetExplorerDriver();
                    break;
            }
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            const int productIndex = 0;

            GoToMainPage();
            var productInfo = GetProductInfo(productIndex);
            IsLineThroughPresent(productInfo.productRegularPriceElement).Should().BeTrue("The regular price in main page should be crossed out");
            IsElementGray(productInfo.productRegularPriceElement).Should().BeTrue("The regular price in main page should be gray");
            IsElementStrong(productInfo.productCampaignPriceElement).Should().BeTrue("The campaign price in main page should be bold");
            IsElementRed(productInfo.productCampaignPriceElement).Should().BeTrue("The campaign price in main page should be red");
            var regularPriceFontSize = GetElementFont(productInfo.productRegularPriceElement);
            var campaignPriceFontSize = GetElementFont(productInfo.productCampaignPriceElement);
            regularPriceFontSize.Should().BeLessThan(campaignPriceFontSize);
            var regularPrice = productInfo.productRegularPriceElement.Text;
            var campaignPrice = productInfo.productCampaignPriceElement.Text;

            GoToProductPage(productIndex);
            var productInfoAtPrPage = GetPoductInfoAtProductPage();
            regularPrice.Should().Be(productInfoAtPrPage.productRegularPriceElement.Text);
            campaignPrice.Should().Be(productInfoAtPrPage.productCampaignPriceElement.Text);
            IsLineThroughPresent(productInfoAtPrPage.productRegularPriceElement).Should().BeTrue("The regular price in product page should be crossed out");
            IsElementGray(productInfoAtPrPage.productRegularPriceElement).Should().BeTrue("The regular price in product page should be gray");
            IsElementStrong(productInfoAtPrPage.productCampaignPriceElement).Should().BeTrue("The campaign price in product page should be bold");
            IsElementRed(productInfoAtPrPage.productCampaignPriceElement).Should().BeTrue("The campaign price in product page should be red");
            regularPriceFontSize = GetElementFont(productInfoAtPrPage.productRegularPriceElement);
            campaignPriceFontSize = GetElementFont(productInfoAtPrPage.productCampaignPriceElement);
            regularPriceFontSize.Should().BeLessThan(campaignPriceFontSize);
        }

        public double GetElementFont(IWebElement element)
        {
            var parsedElementSize = Regex.Match(element.GetCssValue("font-size"), @"^[0-9.]+").ToString().Replace(".", ",");
            return Convert.ToDouble(parsedElementSize);
        }

        private bool IsElementRed(IWebElement element)
        {
            var color = element.GetCssValue("color");
            var parsedRGBa = ParseRGBaValue(color);
            return parsedRGBa["Green"] == "0" && parsedRGBa["Blue"] == "0";
        }

        private bool IsElementStrong(IWebElement element)
        {
            return element.GetAttribute("outerHTML").Contains("strong");
        }

        private (IWebElement productNameElement, IWebElement productRegularPriceElement, IWebElement productCampaignPriceElement) GetPoductInfoAtProductPage()
        {
            var productNameElement = ProductNamePrPageElement;
            var productRegularPriceElement = RegularPricePrPageElement;
            var productCampaignPriceElement = CampaignPricePrPageLElement;

            return (productNameElement, productRegularPriceElement, productCampaignPriceElement);
        }

        private void GoToProductPage(int index)
        {
            var productElement = ProductElements[index];
            var productName = GetProductNameElement(productElement).Text;
            productElement.Click();
            wait.Until(ExpectedConditions.TextToBePresentInElement(ProductNamePrPageElement, productName));
        }

        private (IWebElement productNameElement, IWebElement productRegularPriceElement, IWebElement productCampaignPriceElement) GetProductInfo(int indexOfProduct)
        {
            var productElement = ProductElements[indexOfProduct];
            var productNameElement = GetProductNameElement(productElement);
            var productRegularPriceElement = GetRegularPriceElement(productElement);
            var productCampaignPriceElement = GetCampaignPriceElement(productElement);

            return (productNameElement, productRegularPriceElement, productCampaignPriceElement);
        }

        private bool IsLineThroughPresent(IWebElement element)
        {
            return element.GetCssValue("text-decoration").Contains("line-through");
        }

        private bool IsElementGray(IWebElement element)
        {
            var color = element.GetCssValue("color");
            var parsedRGBa = ParseRGBaValue(color);
            return parsedRGBa["Red"] == parsedRGBa["Green"] && parsedRGBa["Green"] == parsedRGBa["Blue"];
        }

        public void GoToMainPage()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#box-most-popular"), "Most Popular"));
        }

        public Dictionary<string, string> ParseRGBaValue(string rgbaValue)
        {
            var parsedValue = rgbaValue.Split('(').Last().Replace(")", "").Replace(" ", "").Split(',');
            var parsedRGBa = new Dictionary<string, string>()
            {
                {"Red",parsedValue[0]},
                {"Green",parsedValue[1]},
                {"Blue",parsedValue[2]}
            };

            return parsedRGBa;
        }

        //main page
        public string BoxCampaignsLocator => "#box-campaigns";
        public string ProductLocator => "li.product";
        public string ProductNameLocator => ".name";
        public string RegularPriceLocator => ".regular-price";
        public string CampaignPriceLocator => ".campaign-price";

        public IWebElement BoxCampaignsElement => driver.FindElement(By.CssSelector(BoxCampaignsLocator));
        public IList<IWebElement> ProductElements => BoxCampaignsElement.FindElements(By.CssSelector(ProductLocator));
        public IWebElement GetProductNameElement(IWebElement product) =>
            product.FindElement(By.CssSelector(ProductNameLocator));
        public IWebElement GetRegularPriceElement(IWebElement product) =>
            product.FindElement(By.CssSelector(RegularPriceLocator));
        public IWebElement GetCampaignPriceElement(IWebElement product) =>
            product.FindElement(By.CssSelector(CampaignPriceLocator));

        //product page
        public string ProductNamePrPageLocator => "#box-product .title";
        public string RegularPricePrPageLocator => ".regular-price";
        public string CampaignPricePrPageLocator => ".campaign-price";

        public IWebElement ProductNamePrPageElement => driver.FindElement(By.CssSelector(ProductNamePrPageLocator));
        public IWebElement RegularPricePrPageElement => driver.FindElement(By.CssSelector(RegularPricePrPageLocator));
        public IWebElement CampaignPricePrPageLElement => driver.FindElement(By.CssSelector(CampaignPricePrPageLocator));
    }
}

