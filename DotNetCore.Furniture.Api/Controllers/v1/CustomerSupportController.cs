using DotNetCore.Furniture.Domain.Entities;
using DotNetCore.Furniture.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Furniture.Api.Controllers.v1
{
    public class CustomerSupportController : BaseController
    {
        private readonly ICustomerSupportService customerSupportService;

        public CustomerSupportController(ICustomerSupportService customerSupportService)
        {
            this.customerSupportService = customerSupportService;
        }

        [HttpPost("api/v{version:apiVersion}/[controller]/support/submit-inquiry")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> SubmitInquiry([FromBody] CustomerInquiry request)
        {
            var response = await customerSupportService.SubmitInquiryAsync(request.UserId, request.Subject, request.Message);

            return response.ResponseCode switch
            {
                "00" => Ok(response),
                "99" => BadRequest(response),
                "77" => StatusCode(417, response), // DUPLICATE
                _ => StatusCode(500, response)
            };
        }

        [HttpGet("api/v{version:apiVersion}/[controller]/support/admin/get-inquiry-by-id")]
        [Authorize(Policy = "AdminOnly")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> GetInquiryById(string inquiryId)
        {

            var response = await customerSupportService.GetInquiryByIdAsync(inquiryId);

            return response.ResponseCode switch
            {
                "00" => Ok(response),
                "99" => BadRequest(response),
                "77" => StatusCode(417, response), // DUPLICATE
                _ => StatusCode(500, response)
            };
        }

        [HttpGet("api/v{version:apiVersion}/[controller]/support/admin/resolve-inquiry")]
        [Authorize(Policy = "AdminOnly")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> ResolveInquiry(string inquiryId)
        {

            var response = await customerSupportService.ResolveInquiryAsync(inquiryId);

            return response.ResponseCode switch
            {
                "00" => Ok(response),
                "99" => BadRequest(response),
                "77" => StatusCode(417, response), // DUPLICATE
                _ => StatusCode(500, response)
            };
        }
    }
}
