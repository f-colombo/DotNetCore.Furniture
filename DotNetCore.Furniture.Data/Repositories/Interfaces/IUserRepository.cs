using DotNetCore.Furniture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(User user);
        Task<User> GetUserByEmail(string emailAddress);
        Task<User> GetUserById(string userId);
        Task<bool> EmailExists(string emailAddress);
        Task ResetPassword(string emailAddress, string newPassword);
        Task UpdateUserActivationStatus(string emailAddress, bool isActivated);
        Task UpdateAdminActivationStatus(string emailAddress, bool isActivated);
        Task<Admin> RegisterAdmin(Admin admin);
        Task<Admin> GetAdminByEmail(string emailAddress);
        Task<Admin> GetAdminByLoginId(string loginId);
    }
}
