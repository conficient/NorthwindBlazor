using NorthwindBlazor.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindBlazor.Entities.CustomerModels;
using Microsoft.EntityFrameworkCore;

namespace NorthwindBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        /// <summary>
        /// Get customer ID and company name list
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<CompanyNameOnly>> CompanyNames()
        {
            using(var db = Common.GetDb())
            {
                return await 
                    (from c in db.Customers
                     select new CompanyNameOnly(c.CustomerId, c.CompanyName)
                     ).ToListAsync();
            }
        }
    }
}
