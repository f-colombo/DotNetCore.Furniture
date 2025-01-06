using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Entities
{
    public class ShoppingCartItem
    {
        public string FurnitureItemId { get; set; }
        public int Quantity { get; set; }
    }
}
