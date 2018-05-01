using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_5
{
    [TestFixture]
    public class HomeTask
    {
        public IWebDriver driver;
        public WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
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
        public void VerifyCountriesSort()
        {
            Login();
            GoToTheCountriesPage();
            var contryNameColumnIndex = GetColumnIndexByName("Name");
            var zonesColumnIndex = GetColumnIndexByName("Zones");
            var allRows = rowElements;
            var listOfCountryNames = new List<string>();
            foreach (var row in allRows)
            {
                var countryNameElement = GetRowColumnElements(row)[contryNameColumnIndex];
                listOfCountryNames.Add(countryNameElement.Text);

                var zonesAmountElement = GetRowColumnElements(row)[zonesColumnIndex];
                if (Convert.ToInt32(zonesAmountElement.Text) != 0)
                {
                    var linkToEditCountryPage = GetLinkByElement(countryNameElement);
                    OpenPageInNewTab(linkToEditCountryPage, "Edit Country");
                    var zoneNameColumnIndex = GetColumnIndexByName("Name");
                    var allEditPageRows = editPageRowElements.ToList();
                    allEditPageRows.RemoveAt(allEditPageRows.Count - 1); //the last field is invalid
                    var listOfZoneNames = new List<string>();
                    foreach (var editPageRow in allEditPageRows)
                    {
                        var zoneNameElement = GetRowColumnElements(editPageRow)[zoneNameColumnIndex];
                        listOfZoneNames.Add(zoneNameElement.Text);
                    }
                    var actualZoneNamesList = listOfZoneNames;
                    listOfZoneNames.Sort(); //expected
                    Assert.AreEqual(listOfZoneNames, actualZoneNamesList);

                    CloseTabLastTab();
                    GetBackToMainWindow();
                }
            }
            var actualCountryNamesList = listOfCountryNames;
            listOfCountryNames.Sort(); //expected
            Assert.AreEqual(listOfCountryNames, actualCountryNamesList);
        }

        [Test]
        public void VerifyCountriesSortFaster()
        {
            Login();
            GoToTheCountriesPage();
            var tableText = countryTableElement.Text;
            var validRows = GetValidRows(tableText);
            var countryNameList = new List<string>();
            foreach (var row in validRows)
            {
                var splitedRow = row.Split(' ').ToList();
                if (splitedRow.Last() != "0")
                {
                    var allRows = rowElements;
                    var countryNamePart = splitedRow[2];
                    var zonesAmountPart = splitedRow.Last();
                    var rowElement = allRows.First(x=>x.Text.Contains(countryNamePart)&& x.Text.Contains(zonesAmountPart));

                    var linkToEditCountryPage = GetLinkByElement(rowElement);
                    OpenPageInNewTab(linkToEditCountryPage, "Edit Country");
                    var zoneTableText = zoneTableElement.Text;
                    var validZoneRows = GetValidRows(zoneTableText);
                    var countryNamesList = new List<string>();
                    foreach (var zoneRow in validZoneRows)
                    {
                        var splitedZoneRow = zoneRow.Split(' ').ToList();
                        var countryZoneName = GetContryNameWithoutParts(splitedZoneRow);
                        countryNamesList.Add(countryZoneName);
                    }

                    var actualCountryZoneList = countryNamesList;
                    countryNamesList.Sort(); //expected
                    Assert.AreEqual(countryNamesList, actualCountryZoneList);
                    CloseTabLastTab();
                    GetBackToMainWindow();
                }
                var countryName = GetContryNameWithoutParts(splitedRow, removeLast: true);
                countryNameList.Add(countryName);
            }
            var actualCountryList = countryNameList;
            countryNameList.Sort(); //expected
            Assert.AreEqual(countryNameList, actualCountryList);
        }

        [Test]
        public void VerifyZonesSortFaster()
        {
            Login();
            GoToGeoZonesPage();
            var contryNameColumnIndex = GetColumnIndexByName("Name");
            var allRows = rowElements;
            foreach (var row in allRows)
            {
                var countryNameElement = GetRowColumnElements(row).ToList()[contryNameColumnIndex];
                var linkToEditCountryPage = GetLinkByElement(countryNameElement);
                OpenPageInNewTab(linkToEditCountryPage, "Edit Geo Zone");
                var tableText = countryTableElement.GetAttribute("outerHTML");
                var pattern = @"<option value=""[a-zA-Z]{0,2}"" selected=""selected"">.{0,100}</option>";
                var lines = Regex.Matches(tableText, pattern);
                var countryList = new List<string>();
                foreach (var line in lines)
                {
                    var secondPattern = @">.{0,100}<";
                    var country = Regex.Match(line.ToString(), secondPattern).ToString().Replace(">","").Replace("<","");
                    countryList.Add(country);
                }
                var actualCountryList = countryList;
                countryList.Sort();//expected
                Assert.AreEqual(countryList, actualCountryList);
                CloseTabLastTab();
                GetBackToMainWindow();
            }
        }

        public int GetColumnIndexByName(string columnName)
        {
            var headerColumns = headerColumnElements.ToList();
            return headerColumns.FindIndex(x => x.Text == columnName);
        }

        public string GetContryNameWithoutParts(List<string> splitedPart,bool removeLast = false)
        {
            splitedPart.RemoveAt(0);
            splitedPart.RemoveAt(0);
            if (removeLast)
            {
                splitedPart.RemoveAt(splitedPart.Count - 1);
            }

            return string.Concat(splitedPart);
        }

        public void Login()
        {
            const string login = "admin", password = "admin";
            driver.Url = "http://localhost/litecart/admin";
            driver.FindElement(By.CssSelector("[name='username']")).SendKeys(login);
            driver.FindElement(By.CssSelector("[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("[name='login']")).Click();
            wait.Until(ExpectedConditions.ElementExists((By.CssSelector("[title='Logout']"))));
        }

        public void GoToTheCountriesPage()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#content>h1"), "Countries"));
        }

        public void GoToGeoZonesPage()
        {
            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#content>h1"), "Geo Zones"));
        }

        public void CloseTabLastTab()
        {
            driver.SwitchTo().Window(driver.WindowHandles[driver.WindowHandles.Count - 1]).Close();
        }

        public void GetBackToMainWindow()
        {
            driver.SwitchTo().Window(driver.WindowHandles[0]);
        }

        public void OpenPageInNewTab(string link, string textToWait)
        {
            driver.ExecuteJavaScript("window.open();");
            var newTabInstance = driver.WindowHandles[driver.WindowHandles.Count - 1];
            driver.SwitchTo().Window(newTabInstance);
            driver.Url = link;
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#content>h1"), textToWait));
        }

        public string GetLinkByElement(IWebElement column)
        {
            return column.FindElement(By.CssSelector(linkLocator)).GetAttribute("href");
        }

        public List<string> GetValidRows(string allRows)
        {
            var splittedRows = Regex.Split(allRows, "\r\n").ToList();
            splittedRows.RemoveAt(0);
            splittedRows.RemoveAt(splittedRows.Count - 1);
            return splittedRows;
        }

        //locators
        //test 1 scenario 1
        public string headerLocator => "#content>h1";
        public string rowLocator => ".row";
        public string rowColumnLocator => "td";
        public string headerRowLocator => "tbody>.header";
        public string headerColumnLocator => "th";
        public string editPageRowLocator => "#table-zones tr:not(.header)";
        public string linkLocator => "a[href]";
        //test 1 scenario 2
        public string countryTableLocator => ".dataTable tbody";
        public string zoneTableLocator => "#table-zones";
        //test2


        //elements
        public IList<IWebElement> rowElements => driver.FindElements(By.CssSelector(rowLocator));
        public IList<IWebElement> GetRowColumnElements(IWebElement row) =>
            row.FindElements(By.CssSelector(rowColumnLocator));
        public IWebElement headerRowElement => driver.FindElement(By.CssSelector(headerRowLocator));
        public IList<IWebElement> headerColumnElements => headerRowElement.FindElements(By.CssSelector(headerColumnLocator));
        public IList<IWebElement> editPageRowElements => driver.FindElements(By.CssSelector(editPageRowLocator));

        //scenario 2
        public IWebElement countryTableElement => driver.FindElement(By.CssSelector(countryTableLocator));
        public IWebElement zoneTableElement => driver.FindElement(By.CssSelector(zoneTableLocator));
    }
}
