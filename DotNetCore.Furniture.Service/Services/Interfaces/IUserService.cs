using DotNetCore.Furniture.Domain.Common.Generics;
using DotNetCore.Furniture.Domain.DataTransferObjects;
using DotNetCore.Furniture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<NewResult<User>> RegisterUser(User user);
        Task<NewResult<User>> ActivateAccount(string emailAddress, string activationCode);
        Task<NewLoginResult<User>> UserLogin(LoginRequest loginRequest);
        Task<NewLoginResult<Admin>> AdminLogin(AdminLoginRequest loginRequest);
        Task<NewResult<string>> ResetPassword(string emailAddress, string verificationCode, string newPassword);
        Task<NewResult<string>> InitiatePasswordReset(string emailAddress);
        Task<NewResult<Admin>> RegisterAdmin(Admin admin);
        Task<NewResult<Admin>> ActivateAdminAccount(string emailAddress, string activationCode);
        Task<NewResult<string>> ResendVerificationCode(string emailAddress);
    }
}
