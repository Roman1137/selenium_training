using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace selenium_training._11_PageObjects_and_other_design_patterns.HomeTask_PageObjects
{
    public class TestBase
    {
        public Application app;

        [SetUp]
        public void SetUp()
        {
            this.app = new Application();
        }

        [TearDown]
        public void Quit()
        {
            app.Quit();
        }
    }
}
