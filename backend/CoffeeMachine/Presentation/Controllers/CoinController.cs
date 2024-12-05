// Presentation/Controllers/CoinController.cs
using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Application.Services;
using CoffeeMachine.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeMachine.Presentation.DTOs;

namespace CoffeeMachine.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly CoinService _coinService;

        public CoinController(CoinService coinService)
        {
            _coinService = coinService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCoins()
        {
            try
            {
                var coins = await _coinService.GetAllCoinsAsync();
                return Ok(new { Success = true, Coins = coins });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error interno del servidor.", Details = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCoins([FromBody] List<Coin> coinsToAdd)
        {
            try
            {
                await _coinService.RegisterCoinsAsync(coinsToAdd);
                return Ok(new { Success = true, Message = "Monedas registradas correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error al registrar las monedas.", Details = ex.Message });
            }
        }

        [HttpPost("calculate-exchange")]
        public async Task<IActionResult> CalculateExchange([FromBody] ChangeRequestDTO request)
        {
            try
            {
                var exchange = await _coinService.CalculateExchangeAsync(request.ChangeToGive);
                return Ok(new { Success = true, Message = $"Cambio calculado para {request.ChangeToGive} colones.", Exchange = exchange });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("can-give-exact-change/{changeToGive}")]
        public IActionResult CanGiveExactChange(int changeToGive)
        {
            try
            {
                var canGiveChange = _coinService.CanGiveExactChange(changeToGive);
                return Ok(new { Success = true, CanGiveExactChange = canGiveChange });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error interno del servidor.", Details = ex.Message });
            }
        }
    }
}
