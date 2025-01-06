using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.DataTransferObjects
{
    public class CreatePreOrderRequest
    {
        public string UserId { get; set; }
        public string FurnitureItemId { get; set; }
        public int Quantity { get; set; }
    }
}
