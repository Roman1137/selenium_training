using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_training.Lection_2
{
    class CompatibilityWithBrowsers
    {
        //Google Chrome
        //always the last version
        //driver from manufacturer
        //last version are available as well.
        // notes.txt => additional info is there. http://chromedriver.storage.googleapis.com/index.html?path=2.36/

        //IE 6-8 are not supported
        //9-11 webdriver created by Selenium team
        //The microsoft team is not going tpo develov webdriver for IE,stimulating developers to use Edge instead of IE.

        //Edge the webdriver is being developed by Microsoft team but for now,
        //not all command are included in webdrive interface compare to W3C standart

        //Firefox
        //Firefox 49+ - driver from Mozilla, geckodriver - not all commands
        //Firefox 48 is not supported 
        //Firefox 4-47 driver from selenium, old way (extention, which is not secured)
        //Extened Support Relese vesion means to release only once a year

        //Safari
        //v10  safaridriver from Apple
        //v 6-9 driver from Selenium - not all commands are included
        //v5 - not supperted

        //Opera 
        //v15+ (Chromium-base) - driver gfrom Opera- operachromiumdriver
        //v11-12 (Presto- based) - driver from Opera, operaprestodriver  - not supported
        //TESTING IN THIS BROWSER doesn't make sense since you are testing in Chrome => they are the same
    }
}
