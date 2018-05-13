using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace selenium_training._11_PageObjects_and_other_design_patterns
{
    public class Application
    {
        public IWebDriver driver;
        public string BaseUrl = "https://selenium2.com";
        public InnerPage innerPage = new InnerPage();
        public LoginPage loginPage = new LoginPage();

        public void Login(string name, string password)
        {
            driver.Url = BaseUrl;
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("usernamre")).SendKeys(name);
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("sumbit")).Click();
        }
        public void Logout()
        {
            driver.FindElement(By.LinkText("Log out")).Click();
        }
    }
}
