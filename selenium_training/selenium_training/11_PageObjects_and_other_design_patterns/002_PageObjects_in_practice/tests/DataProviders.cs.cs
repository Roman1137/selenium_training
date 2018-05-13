using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_training._11_PageObjects_and_other_design_patterns._002_PageObjects_in_practice
{
    internal class DataProviders
    {
        public static IEnumerable ValidCustomers
        {
            get
            {
                yield return new Customer()
                {
                    Firstname = "Adam",
                    Lastname = "Smith",
                    Phone = "+0123456789",
                    Address = "Hidden Place",
                    Postcode = "12345",
                    City = "New City",
                    Country = "US",
                    Zone = "KY",
                    Email = "adam" + (long)(DateTime.Now - DateTime.MinValue).TotalMilliseconds + "@smith.me",
                    Password = "qwerty"
                };
            }
        }
    }
}
