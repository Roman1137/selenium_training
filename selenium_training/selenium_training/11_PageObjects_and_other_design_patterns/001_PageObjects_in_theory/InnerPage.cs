using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace selenium_training._11_PageObjects_and_other_design_patterns
{
    public class InnerPage
    {
        public IWebDriver driver;
        public void Logout()
        {
            driver.FindElement(By.LinkText("Log out")).Click();
        }
    }
}
