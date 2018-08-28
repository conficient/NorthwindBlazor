using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindBlazor.Entities.Views
{
    public class IdName
    {
        /// <summary>
        /// empty ctor - for serialization support
        /// </summary>
        public IdName() { }

        /// <summary>
        /// ctor for string if
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public IdName(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// ctor for int id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public IdName(int id, string name)
        {
            Id = id.ToString();
            Name = name;
        }

        /// <summary>
        /// object id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// object name
        /// </summary>
        public string Name { get; set; }
    }
}
