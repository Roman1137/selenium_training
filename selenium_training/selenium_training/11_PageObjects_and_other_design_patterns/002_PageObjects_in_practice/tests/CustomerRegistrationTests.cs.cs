using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace selenium_training._11_PageObjects_and_other_design_patterns._002_PageObjects_in_practice
{
    [TestFixture]
    public class CustomerRegistrationTests : TestBase
    {
        [Test,TestCaseSource(typeof(DataProviders),nameof(DataProviders.ValidCustomers))]
        public void CanRegisterCustomer(Customer customer)
        {
            ISet<String> oldIds = app.GetCustomerIds();

            app.RegisterNewCustomer(customer);

            ISet<String> newIds = app.GetCustomerIds();

            Assert.IsTrue(newIds.IsSupersetOf(oldIds));
            Assert.IsTrue(newIds.Count == oldIds.Count + 1);
        }
    }
}
