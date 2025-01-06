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
    public class UserRepository : IUserRepository
    {
        // private readonly IMySqlDbContext mySqlDbContext;
        private readonly IMongoDBLogContext dbContext;
        public UserRepository(/*IMySqlDbContext mySqlDbContext*/ IMongoDBLogContext dbContext)
        {
            // this.mySqlDbContext = mySqlDbContext;
            this.dbContext = dbContext;
        }

        public async Task<bool> EmailExists(string emailAddress)
        {
            var filter = Builders<User>.Filter.Eq(u => u.EmailAddress, emailAddress);
            var user = await dbContext.Users.Find(filter).FirstOrDefaultAsync();
            return user != null;
        }

        public async Task<Admin> GetAdminByEmail(string emailAddress)
        {
            try
            {
                return await dbContext.Admins.Find(u => u.EmailAddress == emailAddress).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Admin> GetAdminByLoginId(string loginId)
        {
            try
            {
                return await dbContext.Admins.Find(u => u.AdminLoginId == loginId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> GetUserByEmail(string emailAddress)
        {
            try
            {
                return await dbContext.Users.Find(u => u.EmailAddress == emailAddress).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<User> GetUserById(string userId)
        {
            try
            {
                return await dbContext.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Admin> RegisterAdmin(Admin admin)
        {
            try
            {
                await dbContext.Admins.InsertOneAsync(admin);
                return admin;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> RegisterUser(User user)
        {
            try
            {
                await dbContext.Users.InsertOneAsync(user);
                return user;
                // await mySqlDbContext.SaveChangesAsync();
                //return user;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task ResetPassword(string emailAddress, string newPassword)
        {
            var filter = Builders<User>.Filter.Eq(u => u.EmailAddress, emailAddress);
            var update = Builders<User>.Update.Set(u => u.Password, newPassword);
            await dbContext.Users.UpdateOneAsync(filter, update);
        }

        public async Task UpdateAdminActivationStatus(string emailAddress, bool isActivated)
        {
            var filter = Builders<Admin>.Filter.Eq(u => u.EmailAddress, emailAddress);
            var update = Builders<Admin>.Update.Set(u => u.isActivated, isActivated);
            await dbContext.Admins.UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserActivationStatus(string emailAddress, bool isActivated)
        {
            var filter = Builders<User>.Filter.Eq(u => u.EmailAddress, emailAddress);
            var update = Builders<User>.Update.Set(u => u.isActivated, isActivated);
            await dbContext.Users.UpdateOneAsync(filter, update);
        }
    }
}
