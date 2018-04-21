using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_5
{
    class DeterminationOfElementVisibility
    {
        //Селениум не хочет выполнять действия с невидимыми элементами. Т.к пользователь этого сделать не может - Селениум тоже отказывается.

        //Selected - проверяет выбрна ли элемент, Enabled -проверяет свойство Disable, если оно установленно, то возвращает false.
        //Но со Displayed, которое ОПРЕДЕЛЯЕТ Является ли элемент выдимым дела хуже. В DOM element properties нету свойства present,
        //по этому приходится использовать этот метод.
        //Метод звучит так => если человек может увидить элемент глазами, то он возвращает true, если нет - false.
        //В целом метод (в C# свойство) Displayed работает хорошо, но есть ряд исключений, когда человек не видит
        //элемент, а селениум говорит, что он видимый.

        //https://output.jsbin.com/saqoca/2
        //На странице есть 4 элемента, которые НЕВОЗМОЖНО увидить человеку, а селениум врет и говорит, что они видимы.
        //1.Прозачный элемент. (почти прозрачный)- если бы прозрачность была 100% - селениум сказал бы, что элемент НЕВИДИМ,
        //НО если прозрачность 98%, то человек его вряд ли увидит, а селениум говорит, что элемент видимый.
        //2.Элемент имеет такой же цвет как и фон, следовательно человек не может увидить элемент, НО
        //селениум говорит, что элемент видимый.(например белый тест на белом фоне). Селениум не сравнивает цвета при вызове метода Displayed
        //3.Видимый элемент, скрытый позади других видимых элементов. Пользователь такой элемент не сможет увидить, т.к
        //этот элемент скрыт другим элементом, а селениум ПРИ ОПРЕДЕЛЕНИИ ВИДИМОСТИ НЕ анализирует перекрытие элементов и по этому
        //считает, что элемент видимый.
        //4.Элемент сдвинут за левый/верхний край экрана. (если элемент сдвинуть за нижний/правый край экрана), то
        //селениум проскролится к нему, а если за левый/верхний край - то туда проскролировать нельзя. Некоторые браузеры это проверяют
        //но например firefox этого не проверяет.

        public IWebDriver driver;
        public WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new FirefoxDriver();
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
        public void VerifyEveryDugHasSticker()
        {
            driver.Url = "https://output.jsbin.com/saqoca/2";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("h1"),
                "Invisibles"));
            ;
            var element98PesentOfVisibility = driver.FindElement(By.CssSelector("#transparent"));
            var a = element98PesentOfVisibility.Displayed; //displayed = true but real user cannot see it.
            var aa = element98PesentOfVisibility.Text; //test is read because element is displayed

            var element100PxLeft = driver.FindElement(By.CssSelector("#shifted")); 
            var b = element100PxLeft.Displayed; //Chrome and FireFox consider this element as not dispolayed. Good
            var ba = element100PxLeft.Text; //test is not read because element is not displayed

            var elementLocatedBehindTheBox = driver.FindElement(By.CssSelector("#behind"));
            var c = elementLocatedBehindTheBox.Displayed;//displayed = true but real user cannot see it.
            var ca = elementLocatedBehindTheBox.Text; //test is read because element is displayed

            var elementWhiteTexOnWhiteFone = driver.FindElement(By.CssSelector("#white"));
            var d = elementWhiteTexOnWhiteFone.Displayed;//displayed = true but real user cannot see it.
            var da = elementWhiteTexOnWhiteFone.Text; //test is read because element is displayed
        }
    }
}
