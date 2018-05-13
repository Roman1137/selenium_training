using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatedTester.BrowserMob.HAR;
using OpenQA.Selenium;

namespace selenium_training._11_PageObjects_and_other_design_patterns._002_PageObjects_in_practice
{
    internal class AdminPanelLoginPage : Page
    {
        public AdminPanelLoginPage(IWebDriver driver) : base(driver) { }

        internal AdminPanelLoginPage Open()
        {
            driver.Url = "http://localhost/litecart/admin";
            return this;
        }

        internal bool IsOnThisPage()
        {
            return driver.FindElements(By.Id("box-login")).Count > 0;
        }

        internal AdminPanelLoginPage EnterUsername(string username)
        {
            driver.FindElement(By.Name("username")).SendKeys(username);
            return this;
        }

        internal AdminPanelLoginPage EnterPassword(string password)
        {
            driver.FindElement(By.Name("password")).SendKeys(password);
            return this;
        }

        internal void SubmitLogin()
        {
            driver.FindElement(By.Name("login")).Click();
            wait.Until(d => d.FindElement(By.Id("box-apps-menu")));
        }

    }
}
