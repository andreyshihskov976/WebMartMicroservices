using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMart.Microservices.DTOs.SubCategory
{
    public class SubCategoryReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid CategoryId { get; set; }
    }
}