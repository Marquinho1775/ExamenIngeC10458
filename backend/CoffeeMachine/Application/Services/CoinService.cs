using CoffeeMachine.Application.Interfaces;
using CoffeeMachine.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeMachine.Application.Services
{
    public class CoinService
    {
        private readonly ICoinRepository _coinRepository;

        public CoinService(ICoinRepository coinRepository)
        {
            _coinRepository = coinRepository;
        }

        public async Task<List<Coin>> GetAllCoinsAsync()
        {
            return await _coinRepository.GetAllCoinsAsync();
        }

        public async Task<bool> UpdateCoinStockAsync()
        {
            return await _coinRepository.UpdateCoinStockAsync();
        }

        public async Task<List<Coin>> GetExchangeAsync(List<Coffee> coffeesToBuy, int totalToPay)
        {
            return await _coinRepository.GetExchangeAsync(coffeesToBuy, totalToPay);
        }


    }
}
