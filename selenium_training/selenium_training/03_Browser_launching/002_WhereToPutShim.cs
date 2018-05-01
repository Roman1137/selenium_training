using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_2
{
    [TestFixture]
    class WhereToPutShim
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        //[SetUp]
        //public void SetUp()
        //{
        //    driver = new InternetExplorerDriver();
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //}

        //[SetUp]
        //public void SetUp()
        //{
        //    driver = new FirefoxDriver();
        //    driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //}

        [Test]
        public void Test()
        {
            const string text = "selenium", textBoxlocator = "lst-ib";
            driver.Url = "https://www.google.com.ua";
            driver.FindElement(By.Id(textBoxlocator)).SendKeys(text + Keys.Enter);
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id(textBoxlocator), text));
        }

        [TearDown]
        public void Quit()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
            driver = null;
        }

        //Where to put?
        //The BEST WAY!
        //1): Put in only in ONE place and update in ONE place if neccessary
        //Put to PATH!(PATH — это системная переменная, содержащая список (через точку с запятой) директорий, в которых ОС будет искать исполняемый файл при вызове команды из консоли. )
        //Open Command Line and print echo %PATH% or echo $PATH 
        //There is the list of pathes where Selenium will foolk for the executing file(driver).
        //The simpliest way is to user one of there => C:Windows\System32.
        // I've created my  owr directory and included it to PATH using this instruction 
        //https://www.computerhope.com/issues/ch000549.htm or this http://nicothin.pro/page/windows-path  => C:\Tools
        //The MOST IMPORTANT thing is not to forget reload console(to verify that derictiry was included) and to reload Visual Studio.
        // Advantage :
        //Put in only in ONE place and update in ONE place if neccessary


        //2): 
        //Project directory BUT it this doing this was doesn't always work.
        //Why? Because it depends on the programming langiage Selenium realization where the drived will be looked for:
        // C#, Java => first in project dirictory and after that in PATH
        // Other don't look at the project directory and start searching for driver from PATH.
        //If the project directory is present in PATH then il will be looked for , otherwise - i will not.
        //Disadvantage :
        //1.if you put it to the many places because you have a many projects and put these file into every project's direcory 
        //=> you will have to upload every file manually if it is needed.
        //2. if you put to the project directory filed for Windows, MacOS, Linux BUT they have the name => it will be disaster.
        //you'd better tune environtment properly.

        //3:
        //It is possible to put the driver to the folder you want and specifu the path to the folder.
        //Substitude this driver = new ChromeDriver(); with driver = new ChromeDriver(@"C:\TestFolder");
        //Disadvantage :
        // if the file is moved to another folder => Selenium will not find him the be browsed will not be opened.
    }
}
