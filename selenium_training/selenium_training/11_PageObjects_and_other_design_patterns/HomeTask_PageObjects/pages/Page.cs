using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._11_PageObjects_and_other_design_patterns.HomeTask_PageObjects
{
    public class Page
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public Page(IWebDriver driver)
        {
            this.driver = driver;
            this.driver.Manage().Window.Maximize();
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
    }
}
