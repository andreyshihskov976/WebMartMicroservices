using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMart.Microservices.Extensions.DTOs.TakenProduct
{
    public class TakenProductCreateDto
    {
        public Guid ProductId { get; set; }
        public Guid BasketId { get; set; }
    }
}