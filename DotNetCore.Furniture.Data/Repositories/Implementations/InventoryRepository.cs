using DotNetCore.Furniture.Data.Repositories.Interfaces;
using DotNetCore.Furniture.Domain.Config.Interfaces;
using DotNetCore.Furniture.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Data.Repositories.Implementations
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IMongoDBLogContext DbContext;

        public InventoryRepository(IMongoDBLogContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<bool> UpdateStockLevelAsync(string furnitureItemId, int newStockLevel)
        {
            var filter = Builders<FurnitureItem>.Filter.Eq(f => f.Id, furnitureItemId);
            var update = Builders<FurnitureItem>.Update.Set(f => f.StockQuantity, newStockLevel);
            var result = await DbContext.FurnitureItems.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<IEnumerable<FurnitureItem>> GetCurrentStockLevelsAsync()
        {
            return await DbContext.FurnitureItems.Find(FilterDefinition<FurnitureItem>.Empty).ToListAsync();
        }
    }
}
