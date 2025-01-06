using DotNetCore.Furniture.Data.Repositories.Interfaces;
using DotNetCore.Furniture.Domain.Common.Generics;
using DotNetCore.Furniture.Domain.Entities;
using DotNetCore.Furniture.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Service.Services.Implementations
{
    public class CustomerSupportService : ICustomerSupportService
    {
        private readonly ICustomerSupportRepository customerSupportRepository;
        public CustomerSupportService(ICustomerSupportRepository customerSupportRepository)
        {
            this.customerSupportRepository = customerSupportRepository;
        }
        public async Task<NewResult<CustomerInquiry>> GetInquiryByIdAsync(string inquiryId)
        {
            try
            {
                if (string.IsNullOrEmpty(inquiryId))
                    throw new ArgumentNullException(nameof(inquiryId), "Inquiry ID cannot be null or empty.");

                var inquiry = await customerSupportRepository.GetInquiryByIdAsync(inquiryId);
                return NewResult<CustomerInquiry>.Success(inquiry, "Inquiry retrieved successfully.");
            }
            catch (Exception ex)
            {
                return NewResult<CustomerInquiry>.Failed(null, $"Error occurred: {ex.Message}");
            }
        }

        public async Task<NewResult<IEnumerable<CustomerInquiry>>> GetUserInquiriesAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

                var inquiries = await customerSupportRepository.GetInquiriesByUserIdAsync(userId);
                return NewResult<IEnumerable<CustomerInquiry>>.Success(inquiries, "Inquiries retrieved successfully.");
            }
            catch (Exception ex)
            {
                return NewResult<IEnumerable<CustomerInquiry>>.Failed(null, $"Error occurred: {ex.Message}");
            }
        }

        public async Task<NewResult<string>> ResolveInquiryAsync(string inquiryId)
        {
            try
            {
                if (string.IsNullOrEmpty(inquiryId))
                    throw new ArgumentNullException(nameof(inquiryId), "Inquiry ID cannot be null or empty.");

                var inquiry = await customerSupportRepository.GetInquiryByIdAsync(inquiryId);
                if (inquiry == null)
                    return NewResult<string>.Failed(null, "Inquiry not found.");

                inquiry.IsResolved = true;
                var updated = await customerSupportRepository.UpdateInquiryAsync(inquiry);

                return updated ? NewResult<string>.Success(inquiryId, "Inquiry resolved successfully.")
                               : NewResult<string>.Failed(inquiryId, "Failed to resolve inquiry.");
            }
            catch (Exception ex)
            {
                return NewResult<string>.Failed(null, $"Error occurred: {ex.Message}");
            }
        }

        public async Task<NewResult<string>> SubmitInquiryAsync(string userId, string subject, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
                    throw new ArgumentNullException("Invalid inquiry data");

                var inquiry = new CustomerInquiry
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Subject = subject,
                    Message = message,
                    CreatedAt = DateTime.UtcNow,
                    IsResolved = false
                };

                await customerSupportRepository.AddInquiryAsync(inquiry);
                return NewResult<string>.Success(inquiry.Id, "Inquiry submitted successfully.");
            }
            catch (Exception ex)
            {
                return NewResult<string>.Failed(null, $"Error occurred: {ex.Message}");
            }
        }
    }
}
