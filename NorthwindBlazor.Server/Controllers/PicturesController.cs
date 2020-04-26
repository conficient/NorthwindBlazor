using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthwindBlazor.Entities;
using NorthwindBlazor.Server.Services;

namespace NorthwindBlazor.Server.Controllers
{
    [Route("[controller]")]
    public class PicturesController : Controller
    {
        private readonly INorthwindDbService _northwindDbService;

        public PicturesController(INorthwindDbService northwindDbService)
        {
            _northwindDbService = northwindDbService;
        }

        [HttpGet]
        [Route("Category/{id:int}")]
        public IActionResult Category(int id)
        {
            using var db = _northwindDbService.GetNorthwindDb();
            var pic = (from c in db.Categories
                       where c.CategoryId == id
                       select c.Picture).SingleOrDefault();
            if (pic == null)
                return NotFound(id);
            else
                return File(GetFixedImage(pic), "image/bmp");
        }


        private static byte[] GetFixedImage(byte[] databaseImage)
        {
            if (databaseImage.Length > 78)
                return databaseImage.Skip(78).ToArray();
            throw new InvalidOperationException("Images should be at least 78 bytes long");
        }
    }
}
