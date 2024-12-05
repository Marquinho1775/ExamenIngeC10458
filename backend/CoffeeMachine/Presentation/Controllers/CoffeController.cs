using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Application.Services;
using System.Threading.Tasks;

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

        [HttpPut]
        public async Task<IActionResult> UpdateCoffeeStock()
        {
            try
            {
                await _coffeeService.UpdateCoffeeStockAsync();
                return Ok(new { Success = true, Message = "Stock actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error interno del servidor.", Exception = ex.Message });
            }
        }
    }
}
