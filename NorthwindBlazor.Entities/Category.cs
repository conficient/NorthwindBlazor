using System.Collections.Generic;

namespace NorthwindBlazor.Entities
{
    /// <summary>
    /// Northwind product categories
    /// </summary>
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public ICollection<Product> Products { get; private set; }

        /// <summary>
        /// Is the image defined for this category?
        /// </summary>
        public bool HasPicture => Picture != null;

        /// <summary>
        /// URL for picture on Server
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static string PictureUrl(int categoryId)
        {
            if (categoryId <= 0) return string.Empty;
            return $"/Pictures/Category/{categoryId}";
        }

    }
}
