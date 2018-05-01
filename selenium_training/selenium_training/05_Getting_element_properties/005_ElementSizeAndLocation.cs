using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_5
{
    class ElementSizeAndLocation
    {
        //Пользователь так же видит размер и положение элемента на странице.
        //За это несет ответственность бразур.

        //getSize()  (в С# Size) - обьект-комбинация из двух чисел: ширина и высота измеренные в пикселях.
        //getLocation() (в С# Location) - обьект комбинация из двух чисел: координаты 
        //левого верхнего угла в пикселях относительно левого веррхнего угла СТРАНИЦЫ (а не относительно окна браузера)=>
        //т.е при скролировании положение элемента относительно страницы остается тем же самым , хотя относительно браузера -меняется.

        //команда getRect - не нашел в C#.

        //Исключения:
        //1.Иногда команды Size и Location не считают так, как хотелось бы. Есть технология CSS transformations.
        //Она предназначена для того, чтобы преобразовывать элементы(растягивать, поворачивать, скручивать).
        //Некоторые браузеры вычисляют Side и Location С УЧЕТОМ трансформации, а некоторые БЕЗ УЧЕТА.
        //В языке есть функция (Locatable element).getCoordinates().InView(), которая позволяет узнать координаты
        //элемента не только относительно окна страницы, а и отсносительно окна браузера.Она скролит к элементу,
        //делает так, чтобы он оказался в видимой части окна браузера, а потом вычисляет его координаты ОТНОСТИЛЬНО ОКНА.
        //Эту функцию используют для того, чтобы показать элементы, с которыми селениум работать не умеет=> узнав их координаты,
        //можно их передать другому инструменту. Ну и еще эта функция используется для скролла.
        //Функция  (Locatable element).getCoordinates().InView() не вошла в W3C и по этому в любой момент может исчезнуть.

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
        public void VerifyEveryDugHasSticker()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("#box-most-popular"),"Most Popular"));


            var element = driver.FindElement(By.CssSelector("[name=login]"));
            var size = element.Size;
            var location = element.Location;

        }
    }
}
