using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_6
{
    [TestFixture]
    public class HomeTask2
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
            random = new Random();
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
        public void VerifyAddingNewProduct()
        {
            LoginAsAdministrator();
            GoToAddNewProductPage();
            var productName = FillFieldsInGeneralTab();
            GoToTabByName("Information");
            FillFieldsInInfoTab();
            GoToTabByName("Prices");
            FillFieldsInPricesTab();
            SubmitAddingNewProduct();
            IsProductPresentInCatalogPage(productName).Should().BeTrue("The product should be displayed at Catalog page after being added");
        }

        public bool IsProductPresentInCatalogPage(string productName)
        {
            var productRows = this.ProductRowElements;
            return productRows.Any(pr => pr.Text.Contains(productName));
        }

        public void WaitUntilSuccessMessageIsPresent(string message)
        {
            wait.Until(ExpectedConditions.TextToBePresentInElement(this.SuccessSignElement,
                message));
        }

        private void SubmitAddingNewProduct()
        {
            this.SaveButtonElement.Click();
            WaitUntilSuccessMessageIsPresent("Changes were successfully saved");
        }

        public void FillFieldsInPricesTab()
        {
            var price = GetRandomNumberValue(5);
            var convertedPrice = Convert.ToInt32(price);
            ClearAndSendKeys(this.PurchasePriceInputElement, price);
            new SelectElement(this.PurchasePriceDropDownElement).SelectByText("US Dollars");
            ClearAndSendKeys(this.PriceTaxUsdInputElement,(convertedPrice * 0.21).ToString());
            ClearAndSendKeys(this.PriceTaxEurInputElement, (convertedPrice * 0.16).ToString());
        }

        public void FillFieldsInInfoTab()
        {
            new SelectElement(this.ManufacturerDropDownElement).SelectByText("ACME Corp.");
            this.KeywordsFieldElement.SendKeys(string.Concat(GetRandomSymbolValue(5), ", ", GetRandomSymbolValue(5)));
            this.ShortDescriptionFieldElement.SendKeys(GetRandomSymbolValue(10));
            this.DescriptionElement.SendKeys(GetRandomSymbolValue(30));
            this.HeadTitleElement.SendKeys(GetRandomSymbolValue(15));
            this.MetaDescriptionElement.SendKeys(GetRandomSymbolValue(10));
        }

        public void ClearAndSendKeys(IWebElement element,string text)
        {
            element.Clear();
            element.SendKeys(text);
        }

        public string FillFieldsInGeneralTab()
        {
            this.EnabledStatusRadioButtonElement.Click();
            var productName = GetRandomSymbolValue(10);
            this.NameFiledElement.SendKeys(productName);
            this.CodeFieldElement.SendKeys(GetRandomNumberValue(5));
            this.UnisexCheckboxElement.Click();
            ClearAndSendKeys(this.QuantityFieldElement, GetRandomNumberValue(2));
            this.FileInputElement.SendKeys(TestContext.CurrentContext.TestDirectory + @"\Lection 6\Files\ProductPicture.png");
            var dateFrom = DateTime.Today.Subtract(TimeSpan.FromDays(1)).ToString("dd/MM/yyyy");
            this.DateFromElement.SendKeys(dateFrom);
            var dateTo = DateTime.Today.Add(TimeSpan.FromDays(1)).ToString("dd/MM/yyyy");
            this.DateToElement.SendKeys(dateTo);

            return productName;
        }

        public void GoToTabByName(string tabName)
        {
            this.GetTabElementByName(tabName).Click();
            WaitTheTabToBeActive(tabName);
        }

        public void GoToAddNewProductPage()
        {
            CatalogElement.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(this.AddNewProductButtonElement));
            this.AddNewProductButtonElement.Click();
            WaitTheTabToBeActive("General");
        }

        public void WaitTheTabToBeActive(string tabName)
        {
            wait.Until(x =>
                x.FindElements(By.CssSelector(this.TabLocator)).First(c => c.Text == tabName).GetAttribute("class")
                    .Contains("active"));
        }

        public void LoginAsAdministrator()
        {
            const string login = "admin", password = "admin";
            driver.Url = "http://localhost/litecart/admin";
            this.LoginNameElement.SendKeys(login);
            this.LoginPasswordElement.SendKeys(password);
            this.LoginButtonElement.Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(this.LogoutButtonLocator)));
        }

        private static string GetRandomSymbolValue(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        private static string GetRandomNumberValue(int length)
        {
            const string pool = "0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        //admin page
        public string LoginNameLocator => "[name='username']";
        public string LoginPasswordLocator => "[name='password']";
        public string LoginButtonLocator => "[name='login']";
        public IWebElement LoginNameElement => this.driver.FindElement(By.CssSelector(LoginNameLocator));
        public IWebElement LoginPasswordElement => this.driver.FindElement(By.CssSelector(LoginPasswordLocator));
        public IWebElement LoginButtonElement => this.driver.FindElement(By.CssSelector(LoginButtonLocator));

        //main admin page
        public string LogoutButtonLocator => "[title='Logout']";
        public string DepartmentLocator => "#app-";
        public string SuccessSignLocator => ".success";
        public string ProductRowLocator => "tbody .row";
        public IWebElement SuccessSignElement => this.driver.FindElement(By.CssSelector(this.SuccessSignLocator));
        public IWebElement CatalogElement => driver.FindElements(By.CssSelector(this.DepartmentLocator)).First(x => x.Text.Contains("Catalog"));

        public IList<IWebElement> ProductRowElements =>this.driver.FindElements(By.CssSelector(this.ProductRowLocator));

        //add new product page
        public string ButtonLocator => ".button";
        public string SaveButtonLocator => "[name=save]";
        public string TabLocator => "ul.index li";
        public IWebElement AddNewProductButtonElement => driver.FindElements(By.CssSelector(this.ButtonLocator))
            .First(x => x.Text.Contains("Add New Product"));
        public IWebElement SaveButtonElement => driver.FindElement(By.CssSelector(this.SaveButtonLocator));

        public IWebElement GetTabElementByName(string name) => driver.FindElements(By.CssSelector(this.TabLocator)).First(x => x.Text == name);

        //'General' page
        public string EnabledStatusRadioButtonLocator => "[name=status][value='1']";
        public string NameFiledLocator => "[name='name[en]']";
        public string CodeFieldLocator => "[name=code]";
        public string UnisexCheckboxLocator => "td [value='1-3']";
        public string QuantityFieldLocator => "[name=quantity]";
        public string FileInputLocator => "[type=file]";
        public string DateFromLocator => "[name=date_valid_from]";
        public string DateToLocator => "[name=date_valid_to]";

        public IWebElement EnabledStatusRadioButtonElement => driver.FindElement(By.CssSelector(this.EnabledStatusRadioButtonLocator));
        public IWebElement NameFiledElement => driver.FindElement(By.CssSelector(this.NameFiledLocator));
        public IWebElement CodeFieldElement => driver.FindElement(By.CssSelector(this.CodeFieldLocator));
        public IWebElement UnisexCheckboxElement => driver.FindElement(By.CssSelector(this.UnisexCheckboxLocator));
        public IWebElement QuantityFieldElement => driver.FindElement(By.CssSelector(this.QuantityFieldLocator));
        public IWebElement FileInputElement => driver.FindElement(By.CssSelector(this.FileInputLocator));
        public IWebElement DateFromElement => driver.FindElement(By.CssSelector(this.DateFromLocator));
        public IWebElement DateToElement => driver.FindElement(By.CssSelector(this.DateToLocator));

        //'Information' page
        public string ManufacturerDropDownLocator => "[name=manufacturer_id]";
        public string KeywordsFieldLocator => "[name=keywords]";
        public string ShortDescriptionFieldLocator => "[name='short_description[en]']";
        public string DescriptionLocator => ".trumbowyg-editor";
        public string HeadTitleLocator => "[name='head_title[en]']";
        public string MetaDescriptionLocator => "[name='meta_description[en]']";

        public IWebElement ManufacturerDropDownElement => driver.FindElement(By.CssSelector(this.ManufacturerDropDownLocator));
        public IWebElement KeywordsFieldElement => driver.FindElement(By.CssSelector(this.KeywordsFieldLocator));
        public IWebElement ShortDescriptionFieldElement => driver.FindElement(By.CssSelector(this.ShortDescriptionFieldLocator));
        public IWebElement DescriptionElement => driver.FindElement(By.CssSelector(this.DescriptionLocator));
        public IWebElement HeadTitleElement => driver.FindElement(By.CssSelector(this.HeadTitleLocator));
        public IWebElement MetaDescriptionElement => driver.FindElement(By.CssSelector(this.MetaDescriptionLocator));

        //'Prices' page
        public string PurchasePriceInputLocator => "[name=purchase_price]";
        public string PurchasePriceDropDownLocator => "[name=purchase_price_currency_code]";
        public string PriceTaxUsdInputLocator => "[name='gross_prices[USD]']";
        public string PriceTaxEurInputLocator => "[name='gross_prices[EUR]']";

        public IWebElement PurchasePriceInputElement => driver.FindElement(By.CssSelector(this.PurchasePriceInputLocator));
        public IWebElement PurchasePriceDropDownElement => driver.FindElement(By.CssSelector(this.PurchasePriceDropDownLocator));
        public IWebElement PriceTaxUsdInputElement => driver.FindElement(By.CssSelector(this.PriceTaxUsdInputLocator));
        public IWebElement PriceTaxEurInputElement => driver.FindElement(By.CssSelector(this.PriceTaxEurInputLocator));
    }
}
