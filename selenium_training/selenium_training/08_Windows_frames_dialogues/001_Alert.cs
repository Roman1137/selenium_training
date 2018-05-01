using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_training._08_Windows_frames_dialogues
{
    class Alert
    {
        //В предыдущих версиях Selenium была плохо настроена работа с диалогами и окнами, которые появляются во время
        //работы веб приложения. Т.к в браузеры, которые работали по старой схеме - внутрь внедлрялось расширение, написанное 
        //на JavaScript. И понятно, что из этого JavaScript кода получить доступ к окнам и диалогами было практически невозможно.
        //Сейчас ситуация получше, но полностью преодолеть проблемы с нативными окнами не удалось.

        //Иногда во время работы возникают диалоги, похожие на нативные, но они порождаются функциями Alert, Confirmation или Promt -
        //с ними Selenium умеет работать. Как правило, эти диалоги успользуются для того, чтобы запросить у пользователя какуе-то важ-
        //ную информацию или запросить подтверждение какого-то действия.
        //Но в основном, разработчики стараются не использовать такие модальные Alert`ы (если оно возникает, то блокируется все-
        //никуда нельзя нажать и ничего сделать). У поддельных диалоговых окон есть множество достоинств: во-первых: они выглядят 
        //красивее и их можно оформлять как хочется, писать любой текст и подганять под себя полностью. Появление "поддельного"
        //диалогового окна не блокирует вкладки. Для работы с "Поддельными" окнами никаких особых команд не нужно.

        private IWebDriver driver;
        private WebDriverWait wait;
        private IAlert foundAlert;

        public void Method()
        {
            IAlert alert = driver.SwitchTo().Alert();
            //если диалоговое окно появляется не мгновенно, то нужно использовать ожидание:
            IAlert newAlert = wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert newLambdaAlert = wait.Until(x => x.SwitchTo().Alert());
            //так же можно получить текст из Alert`a или ввести текст в него
            var alertText = this.foundAlert.Text;
            this.foundAlert.SendKeys("some text");
            this.foundAlert.Accept(); //Ok
            this.foundAlert.Dismiss(); //Cancel , но есть кнопки Cancel нету, 
            //то .Dismiss() - это закрытие диалога крестиком или нажатие клавишу Escape
            this.foundAlert.SetAuthenticationCredentials("Admin","Admin");

            //Иногда окна Alert появляются неожиданно. В Selenium есть capability:
            var options = new ChromeOptions();
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.AcceptAndNotify; // Accept, но потом  выбрасывает UnhanledAlertException - и это правильно
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.Default;
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify; // Dismiss, но потом  выбрасывает UnhanledAlertException - и это правильно
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore; //ничего не делает. и все сценарии "упадут"
            //или можно так, наверное
            options.AddArgument("-disable-notifications");
            options.AddAdditionalCapability("unexpectedAlertException", "dismiss");
        }
    }
}
