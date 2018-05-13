using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_training._11_PageObjects_and_other_design_patterns._002_PageObjects_in_practice
{
    public class Customer
    {
        public string Address { get; internal set; }
        public string City { get; internal set; }
        public string Country { get; internal set; }
        public string Email { get; internal set; }
        public string Firstname { get; internal set; }
        public string Lastname { get; internal set; }
        public string Password { get; internal set; }
        public string Phone { get; internal set; }
        public string Postcode { get; internal set; }
        public string Zone { get; internal set; }
    }
}
