using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindBlazor.Entities.CustomerModels
{
    /// <summary>
    /// Customer model - only shows the customer
    /// </summary>
    public class CompanyNameOnly
    {
        public CompanyNameOnly() { }

        public CompanyNameOnly(string customerId, string companyName) {
            CustomerId = customerId;
            CompanyName = companyName;
        }

        public string CustomerId { get; set; }

        public string CompanyName { get; set; }
    }
}
