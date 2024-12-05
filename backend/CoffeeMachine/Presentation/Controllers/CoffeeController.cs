using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Application.Services;
using System.Threading.Tasks;
using CoffeeMachine.Presentation.DTOs;

namespace CoffeeMachine.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly CoffeeService _coffeeService;

        public CoffeeController(CoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoffees()
        {
            try
            {
                var coffees = await _coffeeService.GetAllCoffeesAsync();
                return Ok(new { Success = true, Coffees = coffees });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error interno del servidor.", Exception = ex.Message });
            }
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseCoffee([FromBody] PurchaseRequestDTO request)
        {
            try
            {
                await _coffeeService.PurchaseCoffeeAsync(request.CoffeeName, request.Quantity);
                return Ok(new { Success = true, Message = "Compra realizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
