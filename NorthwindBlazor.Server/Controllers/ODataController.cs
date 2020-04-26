using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using NorthwindBlazor.Entities;
using NorthwindBlazor.Server.Services;

namespace NorthwindBlazor.Server.Controllers
{
    [Route("odatatest")]
    public class ODataController : Controller
    {
        private readonly INorthwindDbService _northwindDbService;

        public ODataController(INorthwindDbService northwindDbService)
        {
            _northwindDbService = northwindDbService;
        }

        [HttpGet]
        [EnableQuery()]
        public IEnumerable<Category> Categories()
        {
            using var db = _northwindDbService.GetNorthwindDb();
            return db.Categories;
        }
    }
}
