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
    public class CustomerSupportRepository : ICustomerSupportRepository
    {
        private readonly IMongoDBLogContext dbContext;

        public CustomerSupportRepository(IMongoDBLogContext dbContext)
        {
            dbContext = dbContext;
        }

        public async Task AddInquiryAsync(CustomerInquiry inquiry)
        {
            await dbContext.CustomerInquiries.InsertOneAsync(inquiry);
        }

        public async Task<IEnumerable<CustomerInquiry>> GetInquiriesByUserIdAsync(string userId)
        {
            var filter = Builders<CustomerInquiry>.Filter.Eq(i => i.UserId, userId);
            return await dbContext.CustomerInquiries.Find(filter).ToListAsync();
        }

        public async Task<CustomerInquiry> GetInquiryByIdAsync(string inquiryId)
        {
            var filter = Builders<CustomerInquiry>.Filter.Eq(i => i.Id, inquiryId);
            return await dbContext.CustomerInquiries.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateInquiryAsync(CustomerInquiry inquiry)
        {
            var filter = Builders<CustomerInquiry>.Filter.Eq(i => i.Id, inquiry.Id);
            var result = await dbContext.CustomerInquiries.ReplaceOneAsync(filter, inquiry);
            return result.ModifiedCount > 0;
        }
    }
}
