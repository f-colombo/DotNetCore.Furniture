using DotNetCore.Furniture.Domain.DataTransferObjects;
using DotNetCore.Furniture.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Furniture.Api.Controllers.v1
{
    public class InventoryController : BaseController
    {
        private readonly IInventoryService inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        [HttpPost("api/v{version:apiVersion}/[controller]/update-stock-level")]
        [Authorize(Policy = "AdminOnly")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> UpdateStockLevel(StockLevelRequest request)
        {
            var response = await inventoryService.UpdateStockLevelAsync(request.FurnitureItemId, request.NewStockLevel);

            return response.ResponseCode switch
            {
                "00" => Ok(response),
                "99" => BadRequest(response),
                "77" => StatusCode(417, response), // DUPLICATE
                _ => StatusCode(500, response)
            };
        }

        [HttpPost("api/v{version:apiVersion}/[controller]/get-stock-level")]
        [Authorize(Policy = "AdminOnly")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> GetStockLevels()
        {

            var response = await inventoryService.GetCurrentStockLevelsAsync();

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
