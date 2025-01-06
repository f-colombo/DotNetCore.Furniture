using DotNetCore.Furniture.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Entities
{
    public class PreOrder
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FurnitureItemId { get; set; }
        public DateTime PreOrderDate { get; set; }
        public PreOrderStatus Status { get; set; }
        public int Quantity { get; set; }
    }
}
