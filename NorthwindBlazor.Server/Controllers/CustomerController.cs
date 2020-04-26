using NorthwindBlazor.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindBlazor.Entities.CustomerModels;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using NorthwindBlazor.Server.Services;

namespace NorthwindBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly INorthwindDbService _northwindDbService;

        /// <summary>
        /// Inject Northwind service
        /// </summary>
        /// <param name="northwindDbService"></param>
        public CustomerController(INorthwindDbService northwindDbService)
        {
            _northwindDbService = northwindDbService;
        }

        /// <summary>
        /// Get customer ID and company name list
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<CompanyNameOnly>> CompanyNames()
        {
            using var db = _northwindDbService.GetNorthwindDb();
            return await
                (from c in db.Customers
                 select new CompanyNameOnly(c.CustomerId, c.CompanyName)
                ).ToListAsync();
        }
    }
}
