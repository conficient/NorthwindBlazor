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
        public string CustomerId { get; set; }

        public string CompanyName { get; set; }
    }
}
