using DotNetCore.Furniture.Domain.Config.Interfaces;
using DotNetCore.Furniture.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Config.Implementations
{
    public partial class MongoDBLogContext : IMongoDBLogContext
    {
        public IMongoCollection<MyBankLog> Logs { get; set; }
        public IMongoCollection<User> Users { get; set; }
        public IMongoCollection<Admin> Admins { get; set; }
        public IMongoCollection<FurnitureItem> FurnitureItems { get; set; }
        public IMongoCollection<ShoppingCart> ShoppingCarts { get; set; }
        public IMongoCollection<WishlistItem> WishlistItems { get; set; }
        public IMongoCollection<Order> Orders { get; set; }
        public IMongoCollection<PreOrder> PreOrders { get; set; }
        public IMongoCollection<CustomerInquiry> CustomerInquiries { get; set; }

        public MongoDBLogContext(IMongoDbConfig config, IMongoClient mongoClient)
        {
            var client = new MongoClient(config.ConnectionString);
            var database = client.GetDatabase(config.DatabaseName);

            Logs = database.GetCollection<MyBankLog>("MyBankLog");
            Users = database.GetCollection<User>("User");
            Admins = database.GetCollection<Admin>("Admin");
            FurnitureItems = database.GetCollection<FurnitureItem>("FurnitureItem");
            ShoppingCarts = database.GetCollection<ShoppingCart>("ShoppingCartItem");
            WishlistItems = database.GetCollection<WishlistItem>("WishlistItem");
            Orders = database.GetCollection<Order>("Order");
            PreOrders = database.GetCollection<PreOrder>("PreOrder");
            CustomerInquiries = database.GetCollection<CustomerInquiry>("CustomerInquiry");
        }
    }
}
