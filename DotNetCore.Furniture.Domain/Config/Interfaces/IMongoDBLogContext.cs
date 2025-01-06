using DotNetCore.Furniture.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Config.Interfaces
{
    public partial interface IMongoDBLogContext
    {
        IMongoCollection<MyBankLog> Logs { get; set; }
        IMongoCollection<User> Users { get; set; }
        IMongoCollection<Admin> Admins { get; set; }
        IMongoCollection<FurnitureItem> FurnitureItems { get; set; }
        IMongoCollection<ShoppingCart> ShoppingCarts { get; set; }
        IMongoCollection<WishlistItem> WishlistItems { get; set; }
        IMongoCollection<Order> Orders { get; set; }
        IMongoCollection<PreOrder> PreOrders { get; set; }
        IMongoCollection<CustomerInquiry> CustomerInquiries { get; set; }
    }
}
