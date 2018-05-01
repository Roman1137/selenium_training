using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._08_Windows_frames_dialogues
{
    [TestFixture]
    public class HomeTask
    {
        //BTW the link will trigger the new window(tab) opening if it HAS target="_blank" attribute.
        private IWebDriver driver;
        private WebDriverWait wait;
        private Random rnd;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            rnd = new Random();
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
        public void VerifyLinksAtEditCountryPage()
        {
            LoginAsAdministrator();
            GoToEditCountryPage();
            BlankLinkElements.ForEach(link =>
            {
                var windowId = OpenLinkInOtherPage(link);
                CloseWindow(windowId);
            });
        }

        private void CloseWindow(string windowId)
        {
            var currentWindowId = this.driver.CurrentWindowHandle;
            this.driver.SwitchTo().Window(windowId);
            this.driver.Close();
            this.driver.SwitchTo().Window(currentWindowId);
        }

        public string OpenLinkInOtherPage(IWebElement link, int time = 5)
        {
            var amountOfWindows = this.driver.WindowHandles;
            link.Click();
            var windowWait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(time))
            {
                Message = $"The new window was not opened in {time} seconds."
            };
            return windowWait.Until(d => d.WindowHandles.Except(amountOfWindows).First());
        }

        private void GoToEditCountryPage()
        {
            GoToCountriesPage();
            var allCountryRows = this.CountryRowElements;
            var indexOfCountryToEdit = rnd.Next(0, allCountryRows.Count - 1);
            var countryToEdit = allCountryRows[indexOfCountryToEdit];
            GetEditButtonByRow(countryToEdit).Click();
            wait.Until(d => d.FindElement(By.CssSelector(this.TittleLocator)).Text.Contains("Edit Country"));
        }

        private void GoToCountriesPage()
        {
            CountriesElement.Click();
            wait.Until(d => d.FindElement(By.CssSelector(this.TittleLocator)).Text.Contains("Countries"));
        }

        public void LoginAsAdministrator()
        {
            const string login = "admin", password = "admin";
            driver.Url = "http://localhost/litecart/admin";
            this.LoginNameElement.SendKeys(login);
            this.LoginPasswordElement.SendKeys(password);
            this.LoginButtonElement.Click();
            wait.Until(d => d.FindElement(By.CssSelector(this.LogoutButtonLocator)));
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
        public IWebElement LogoutButtonElement => this.driver.FindElement(By.CssSelector(this.LogoutButtonLocator));
        public IWebElement CountriesElement => driver.FindElements(By.CssSelector(this.DepartmentLocator)).First(x => x.Text.Contains("Countries"));

        //countries  page
        public string TittleLocator => "#content h1";
        public string CountryRowLocator => ".row";
        public string EditButtonLocator => "[title=Edit]";

        public IList<IWebElement> CountryRowElements => driver.FindElements(By.CssSelector(this.CountryRowLocator));
        public IWebElement GetEditButtonByRow(IWebElement row) => row.FindElement(By.CssSelector(this.EditButtonLocator));

        //edit country page
        public string BlankLinkLocator => "[enctype] [href^=http]";
        public List<IWebElement> BlankLinkElements => driver.FindElements(By.CssSelector(this.BlankLinkLocator)).ToList();
    }
}
