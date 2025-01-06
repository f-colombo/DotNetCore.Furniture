using DotNetCore.Furniture.Domain.Config.Interfaces;
using DotNetCore.Furniture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Data.Repositories.Implementations
{
    public class OrderRepository
    {
        private readonly IMongoDBLogContext DbContext;

        public OrderRepository(IMongoDBLogContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await DbContext.Orders.InsertOneAsync(order);
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await DbContext.Orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await DbContext.Orders.Find(o => o.UserId == userId).ToListAsync();
        }
    }
}
