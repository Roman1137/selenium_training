using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_6
{
    [TestFixture]
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
            random = new Random();
        }

        [Test]
        public void VerifyCustomerCreation()
        {
            GoToCreateAccountPage();
            VerifyCaptchaIsNotDisplayed();
            var credentials = FillAllFileds();
            SubmitAccountCreation();
            Logout();
            LoginAsCustomer(credentials);
        }

        private void LoginAsCustomer((string email, string password, string firstName, string lastName) credentials)
        {
            this.LoginEmailFieldElement.SendKeys(credentials.email);
            this.LoginPasswordFieldElement.SendKeys(credentials.password);
            this.LoginButtonElement.Click(); 
            WaitUntilSuccessMessageIsPresent($"You are now logged in as {credentials.firstName} {credentials.lastName}");
        }

        private void Logout()
        {
            this.LogoutButtonLinkElement.Click();
            WaitUntilSuccessMessageIsPresent("You are now logged out");
        }

        public void SubmitAccountCreation()
        {
            this.CreateAccountButtonElement.Click();
            WaitUntilSuccessMessageIsPresent("Your customer account has been created");
        }

        private (string email, string password,string firstName,string lastName) FillAllFileds()
        {
            this.TaxIdFormElement.SendKeys(GetRandomNumberValue(5));
            this.CompanyFormElement.SendKeys(GetRandomSymbolValue(10));
            var firstName = GetRandomSymbolValue(10);
            this.FirstNameFormElement.SendKeys(firstName);
            var lastName = GetRandomSymbolValue(10);
            this.LastNameFormElement.SendKeys(lastName);
            this.Address1FormElement.SendKeys(GetRandomSymbolValue(10));
            this.Address2FormElement.SendKeys(GetRandomSymbolValue(10));
            this.PostCodeFormElement.SendKeys(GetRandomNumberValue(5));
            this.CityFormElement.SendKeys(GetRandomSymbolValue(10));
            //driver.ExecuteJavaScript("arguments[0].selectedIndex = 3; arguments[0].dispatchEvent(new Event('change'))", this.CountryDropDownElement);
            driver.ExecuteJavaScript("arguments[0].style.opacity = 1",this.CountryDropDownElement);
            new SelectElement(CountryDropDownElement).SelectByText("United States");
            const string zoneToSelect = "Arkansas";
            wait.Until(ExpectedConditions.TextToBePresentInElement(ZoneDropDownElement, zoneToSelect));
            new SelectElement(this.ZoneDropDownElement).SelectByText(zoneToSelect);
            var email = string.Concat(GetRandomSymbolValue(10), "@test.com");
            this.EmailForm.SendKeys(email);
            this.PhoneFormElement.SendKeys(string.Concat("+1",GetRandomNumberValue(8)));
            var password = GetRandomNumberValue(10);
            this.PasswordFormElement.SendKeys(password);
            this.ConfirmedPasswordFormElement.SendKeys(password);

            return (email, password, firstName,lastName);
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

        public void GoToSecurityPage()
        {
            this.SettingsElement.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(this.SecurityElement));
            this.SecurityElement.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(this.CaptchaRowElement));
        }

        public void WaitUntilSuccessMessageIsPresent(string message)
        {
            wait.Until(ExpectedConditions.TextToBePresentInElement(this.SuccessSignElement,
                message));
        }

        public void VerifyCaptchaIsNotDisplayed()
        {
            if (IsCaptchaDisplayed())
            {
                OpenPageInNewTab();
                LoginAsAdministrator();
                GoToSecurityPage();
                SelectFalseValue();
                CloseTabLastTab();
                GetBackToMainWindow();
                WaitUntilCaptchaIsNotDisplayed();
            }
        }

        public void WaitUntilCaptchaIsNotDisplayed()
        {
            this.driver.Navigate().Refresh();
            wait.Until(x => x.FindElements(By.CssSelector(this.CaptchaFieldLocator)).Count == 0);
        }

        public void OpenPageInNewTab()
        {
            driver.ExecuteJavaScript("window.open();");
            var newTabInstance = driver.WindowHandles[driver.WindowHandles.Count - 1];
            driver.SwitchTo().Window(newTabInstance);
        }

        public void CloseTabLastTab()
        {
            this.driver.Manage().Cookies.DeleteAllCookies();
            driver.SwitchTo().Window(driver.WindowHandles[driver.WindowHandles.Count - 1]).Close();
        }

        public void GetBackToMainWindow()
        {
            driver.SwitchTo().Window(driver.WindowHandles[0]);
        }

        public void SelectFalseValue()
        {
            if (this.CaptchaRowElement.Text.Contains("True"))
            {
                this.PencilElement.Click();
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(this.SaveButtonLocator)));
                this.FalseCaptchaElement.Click();
                this.SaveButtonElement.Click();
                WaitUntilSuccessMessageIsPresent("Changes were successfully saved");
            }
        }

        public void GoToMainPage()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#box-most-popular"), "Most Popular"));
        }

        public void GoToCreateAccountPage()
        {
            GoToMainPage();
            this.NewCustomersLinkElement.Click();
            wait.Until(ExpectedConditions.TextToBePresentInElement(this.CreateAccountFromElement, "Create Account"));
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

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
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
        public string SubDepartmentLocator => "[id^=doc]";
        public IWebElement LogoutButtonElement => this.driver.FindElement(By.CssSelector(this.LogoutButtonLocator));
        public IWebElement SettingsElement => driver.FindElements(By.CssSelector(this.DepartmentLocator)).First(x => x.Text.Contains("Settings"));
        public IWebElement SecurityElement => SettingsElement
            .FindElements(By.CssSelector(this.SubDepartmentLocator)).First(x => x.Text == "Security");

        //security page
        public string RowLocator => ".row";
        public string PencilLocator => ".fa-pencil";
        public string FalseCaptchaLocator => "[name=value][value='0']";
        public string SaveButtonLocator => "[name=save]";
        public string SuccessSignLocator => ".success";

        public IWebElement CaptchaRowElement => this.driver
            .FindElements(By.CssSelector(this.RowLocator)).First(x => x.Text.Contains("CAPTCHA"));

        public IWebElement PencilElement => CaptchaRowElement.FindElement(By.CssSelector(this.PencilLocator));
        public IWebElement FalseCaptchaElement => CaptchaRowElement.FindElement(By.CssSelector(this.FalseCaptchaLocator));
        public IWebElement SaveButtonElement => CaptchaRowElement.FindElement(By.CssSelector(this.SaveButtonLocator));
        public IWebElement SuccessSignElement => this.driver.FindElement(By.CssSelector(this.SuccessSignLocator));

        //main page
        public string LoginFormLocator => "[name=login_form]";
        public string LinkLocator => "a[href]";
        public string LoginEmailFieldLocator => "[name=email]";
        public string LoginPasswordFieldLocator => "[name=password]";
        public IWebElement LoginFormElement => this.driver.FindElement(By.CssSelector(this.LoginFormLocator));
        public IWebElement NewCustomersLinkElement => LoginFormElement.FindElements(By.CssSelector(this.LinkLocator))
            .First(x => x.Text.Contains("New customers click here"));
        public IWebElement LogoutButtonLinkElement => this.driver.FindElements(By.CssSelector(this.LinkLocator))
            .First(x => x.Text.Contains("Logout"));
        public IWebElement LoginEmailFieldElement => LoginFormElement.FindElement(By.CssSelector(this.LoginEmailFieldLocator));
        public IWebElement LoginPasswordFieldElement => LoginFormElement.FindElement(By.CssSelector(this.LoginPasswordFieldLocator));

        //create account page
        public string CaptchaFieldLocator => "[name=captcha]";
        public string CreateAccountFormLocator => "#create-account";
        public string TaxIdFormLocator => "[name=tax_id]";
        public string CompanyFormLocator => "[name=company]";
        public string FirstNameFormLocator => "[name=firstname]";
        public string LastNameFormLocator => "[name=lastname]";
        public string Address1FormLocator => "[name=address1]";
        public string Address2FormLocator => "[name=address2]";
        public string PostCodeFormLocator => "[name=postcode]";
        public string CityFormLocator => "[name=city]";
        public string CountryDropDownLocator => "[name=country_code]";
        public string ZoneDropDownLocator => "select[name=zone_code]";
        public string EmailFormLocator => "[name=email]";
        public string PhoneFormLocator => "[name=phone]";
        public string PasswordFormLocator => "[name=password]";
        public string ConfirmedPasswordFormLocator => "[name=confirmed_password]";
        public string CreateAccountButtonLocator => "[name=create_account]";


        public bool IsCaptchaDisplayed() => this.driver.FindElements(By.CssSelector(CaptchaFieldLocator)).Count != 0;
        public IWebElement CreateAccountFromElement => this.driver.FindElement(By.CssSelector(this.CreateAccountFormLocator));
        public IWebElement TaxIdFormElement => this.driver.FindElement(By.CssSelector(this.TaxIdFormLocator));
        public IWebElement CompanyFormElement => this.driver.FindElement(By.CssSelector(this.CompanyFormLocator));
        public IWebElement FirstNameFormElement => this.driver.FindElement(By.CssSelector(this.FirstNameFormLocator));
        public IWebElement LastNameFormElement => this.driver.FindElement(By.CssSelector(this.LastNameFormLocator));
        public IWebElement Address1FormElement => this.driver.FindElement(By.CssSelector(this.Address1FormLocator));
        public IWebElement Address2FormElement => this.driver.FindElement(By.CssSelector(this.Address2FormLocator));
        public IWebElement PostCodeFormElement => this.driver.FindElement(By.CssSelector(this.PostCodeFormLocator));
        public IWebElement CityFormElement => this.driver.FindElement(By.CssSelector(this.CityFormLocator));
        public IWebElement CountryDropDownElement => this.driver.FindElement(By.CssSelector(this.CountryDropDownLocator));
        public IWebElement ZoneDropDownElement => this.driver.FindElement(By.CssSelector(this.ZoneDropDownLocator));
        public IWebElement EmailForm => this.driver.FindElement(By.CssSelector(this.EmailFormLocator));
        public IWebElement PhoneFormElement => this.driver.FindElement(By.CssSelector(this.PhoneFormLocator));
        public IWebElement PasswordFormElement => this.driver.FindElement(By.CssSelector(this.PasswordFormLocator));
        public IWebElement ConfirmedPasswordFormElement => this.driver.FindElement(By.CssSelector(this.ConfirmedPasswordFormLocator));
        public IWebElement CreateAccountButtonElement => this.driver.FindElement(By.CssSelector(this.CreateAccountButtonLocator));
    }
}
