using CoffeeMachine.Application.Interfaces;
using CoffeeMachine.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        public async Task RegisterCoinsAsync(List<Coin> coinsToAdd)
        {
            var existingCoins = await _coinRepository.GetAllCoinsAsync();

            foreach (var coinToAdd in coinsToAdd)
            {
                var existingCoin = existingCoins.FirstOrDefault(c => c.CoinValue == coinToAdd.CoinValue);
                if (existingCoin != null)
                {
                    existingCoin.CoinStock += coinToAdd.CoinStock;
                    await _coinRepository.UpdateCoinAsync(existingCoin);
                }
                else
                {
                    existingCoins.Add(new Coin(coinToAdd.CoinValue, coinToAdd.CoinStock));
                }
            }
        }

        public async Task<List<Coin>> CalculateExchangeAsync(int changeToGive)
        {
            var coins = await _coinRepository.GetAllCoinsAsync();
            var exchange = CalculateExchange(changeToGive, coins);

            if (exchange == null)
            {
                throw new Exception("No hay suficiente cambio disponible.");
            }

            foreach (var coin in exchange)
            {
                var existingCoin = coins.First(c => c.CoinValue == coin.CoinValue);
                existingCoin.CoinStock -= coin.CoinStock;
                await _coinRepository.UpdateCoinAsync(existingCoin);
            }

            return exchange;
        }

        private List<Coin> CalculateExchange(int changeToGive, List<Coin> availableCoins)
        {
            var exchange = new List<Coin>();
            var remainingChange = changeToGive;

            foreach (var coin in availableCoins.OrderByDescending(c => c.CoinValue))
            {
                if (remainingChange <= 0)
                {
                    break;
                }

                int numCoinsNeeded = remainingChange / coin.CoinValue;
                int numCoinsAvailable = coin.CoinStock;

                int numCoinsToUse = Math.Min(numCoinsNeeded, numCoinsAvailable);

                if (numCoinsToUse > 0)
                {
                    exchange.Add(new Coin(coin.CoinValue, numCoinsToUse));
                    remainingChange -= numCoinsToUse * coin.CoinValue;
                }
            }

            if (remainingChange > 0)
            {
                return null;
            }

            return exchange;
        }

        public bool CanGiveExactChange(int changeToGive)
        {
            var coins = _coinRepository.GetAllCoinsAsync().Result;
            var exchange = CalculateExchange(changeToGive, coins);
            return exchange != null;
        }
    }
}
