using DotNetCore.Furniture.Domain.Common.Generics;
using DotNetCore.Furniture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Service.Services.Interfaces
{
    public interface ICustomerSupportService
    {
        Task<NewResult<string>> SubmitInquiryAsync(string userId, string subject, string message);
        Task<NewResult<IEnumerable<CustomerInquiry>>> GetUserInquiriesAsync(string userId);
        Task<NewResult<CustomerInquiry>> GetInquiryByIdAsync(string inquiryId);
        Task<NewResult<string>> ResolveInquiryAsync(string inquiryId);
    }
}
