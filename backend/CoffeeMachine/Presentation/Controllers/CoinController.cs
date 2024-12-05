using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Application.Services;
using CoffeeMachine.Domain.Entities;
using CoffeeMachine.Application.Interfaces;

namespace CoffeeMachine.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly CoinService _coinservice; 
        public CoinController(CoinService coinservice)
        {
            _coinservice = coinservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoins()
        {
            try
            {
                var coins = await _coinservice.GetAllCoinsAsync();
                return Ok(new { Success = true, Coins = coins });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error interno del servidor." });
            }
        }
    }
}
