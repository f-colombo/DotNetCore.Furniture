using DotNetCore.Furniture.Domain.Common.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Service.Services.Interfaces
{
    public interface IEmailService
    {
        Task<NewResult<string>> SendActivationEmail(string emailAddress, string verificationCode);
    }
}
