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

        public async Task UpdateCoinStockAfterExchangeAsync(List<Coin> usedCoins)
        {
            await _coinRepository.UpdateCoinStockAfterExchangeAsync(usedCoins);
        }

        public async Task RegisterCoinsAsync(List<Coin> coinsToAdd)
        {
            await _coinRepository.RegisterCoinsAsync(coinsToAdd);
        }

        public async Task<List<Coin>> CalculateExchangeAsync(int changeToGive)
        {
            return await _coinRepository.CalculateExchangeAsync(changeToGive);
        }


    }
}
