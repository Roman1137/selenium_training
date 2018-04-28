using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace selenium_training.Lection_6
{
    class _004_AdvancedInteractionsAPI
    {
        //do what I mean
        //обычные click и sendKeys

        //do what I say
        //Advanced Interaction API

        //В чем разнница? 
        //1.Например, когда мы используем обычный sendKeys то, она неявно при необходимости может выполнить
        //клик по элементу, если это необходимо.
        //При использовании Advanced Interaction API никакого дополнительного клика не будет.(Там тоже есть Clich и SendKeys, но они ДРУГИЕ)
        //2.Когда мы выполняем клик, то имеется ввиду, что мы нажали мышку и отпустили.
        //Через Advanced Interaction API мышку можно не отпускать.

        //Основная часть интерфейса Advanced Interaction API
        //Click == ClickAndHold + Release
        //SendKeys == Целая серия команд KeyDown, KeyUp
        //ClickAndHold - позволяет зажать кнопку мыши и не отпускать
        //MoveToElement/MoveByOffSet - перемещает мышку куда- нибудь либо на другой элемент, либо на указанное растояния в пикселях
        //Release - позволяет отпустить КНОПКУ МЫШИ
        //KeyDown - нажать кливишу 
        //KeyUp -  отпустить клавишу

        public IWebDriver driver;
        public WebDriverWait wait;
        public IWebElement drag;
        public IWebElement drop;

        //вот так выглятит цепочка которая выполняет перетаскивание элемента с зажатой клавишей Ctr
        public void Method()
        {
            new Actions(this.driver)
                .MoveToElement(this.drag)
                .KeyDown(Keys.Control)
                .ClickAndHold()
                .MoveToElement(this.drop)
                .Release()
                .KeyUp(Keys.Control)
                .Perform();

            new Actions(this.driver)
                .DragAndDrop(this.drag, this.drop); //то, что выше, но меньше кода
        }

        //В классе Actions встречаются методы двух типов: с параметрами и без параметров.
        //Базовыми методами явялются те, которые не принимаю в качестве параметра weblement. Они выполняют
        //действие в том положении, где находится СЕЙЧАС мышка.
        //А методы, которые принимают webelemnt сначала выполняют перемещение курсора мышки в центр элемента, а потом уже 
        //выполняют действие (метод)

        private IWebElement element;
        private IWebElement element2;
        private IWebElement element3;
        public void Method2()
        {
            new Actions(this.driver).ContextClick(); //- клик ПАРАВОЙ кнопкой мышки.
            new Actions(this.driver).DoubleClick(); //- двойной клик
            new Actions(this.driver).Build(); // - с помощь этой команды МОЖНО СОЗДАВАТЬ СВОИ ЦЕПОЧКИ. а потом уже использовать их с помощью perform
            new Actions(this.driver).KeyDown(element, Keys.Control); //- сначала перемещает курсор в центр element а потом нажимает Control
            new Actions(this.driver).MoveByOffset(15, 25);//перемещение мышки от текущего местоположения курсора
            //команда Pause() - сейчас ее нет, но ее добавлят опять т.к она нужна будет для формирование цепочки действий
            //которая будет выполняться не на строноне клиентской библиотеки а отправлятся в драйвер. 
            //Команда  Pause() играет достаточно важную роль при выстраивании этих цепочек
        }

        public void Method3()
        {
            //Вот так можно попросить селениум кликнуть в левый верхний уго элемента.
            new Actions(this.driver)
                .MoveToElement(element,1,1)
                .Click()
                .Perform();
            //клик по координатам 5, 5 относительно центра элемента
            new Actions(this.driver)
                .MoveToElement(this.element)
                .MoveByOffset(5,5)
                .Click()
                .Perform();
            //работа С ВСПЛЫВАЮЩИМ МЕНЮ
            new Actions(this.driver)
                .MoveToElement(element)
                .MoveToElement(element2)
                .MoveToElement(element3)
                .Click()
                .Perform();
            //drag and drop
            new Actions(this.driver)
                .DragAndDrop(this.drag,this.drop)
                .Perform();
        }

        //В Firefox geckofriver сложные действия пока не реализованы совсем!
    }
}
