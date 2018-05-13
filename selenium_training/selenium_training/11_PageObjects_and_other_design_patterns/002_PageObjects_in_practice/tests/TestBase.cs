using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace selenium_training._11_PageObjects_and_other_design_patterns._002_PageObjects_in_practice
{
    public class TestBase
    {
        public Application app;

        [SetUp]
        public void Start()
        {
            this.app = new Application();
        }

        [TearDown]
        public void Stop()
        {
            app.Quit();
            app = null;
        }
    }
}
