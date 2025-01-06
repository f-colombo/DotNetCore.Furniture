using DotNetCore.Furniture.Domain.Common.Generics;
using DotNetCore.Furniture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Service.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<NewResult<bool>> UpdateStockLevelAsync(string furnitureItemId, int newStockLevel);

        Task<NewResult<IEnumerable<FurnitureItem>>> GetCurrentStockLevelsAsync();
    }
}
