using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.DataTransferObjects
{
    public class StockLevelRequest
    {
        public string FurnitureItemId { get; set; }
        public int NewStockLevel { get; set; }
    }
}
