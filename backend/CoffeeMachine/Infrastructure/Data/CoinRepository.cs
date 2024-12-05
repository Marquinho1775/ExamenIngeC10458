using CoffeeMachine.Domain.Entities;
using CoffeeMachine.Application.Interfaces;

namespace CoffeeMachine.Infrastructure.Data
{
    public class CoinRepository : ICoinRepository
    {
        private static readonly List<Coin> _coins = new List<Coin>()
        {
            new Coin(500, 20),
            new Coin(100, 30),
            new Coin(50, 50),
            new Coin(25, 25)
        };

        public Task<List<Coin>> GetAllCoinsAsync()
        {
            return Task.FromResult(_coins);
        }

        public Task UpdateCoinStockAfterExchangeAsync(List<Coin> usedCoins)
        {
            foreach (var usedCoin in usedCoins)
            {
                var coin = _coins.FirstOrDefault(c => c.CoinValue == usedCoin.CoinValue);
                if (coin != null)
                {
                    coin.CoinStock -= usedCoin.CoinStock;
                }
            }

            return Task.CompletedTask;
        }

        public Task RegisterCoinsAsync(List<Coin> coinsToAdd)
        {
            foreach (var coinToAdd in coinsToAdd)
            {
                var existingCoin = _coins.FirstOrDefault(c => c.CoinValue == coinToAdd.CoinValue);
                if (existingCoin != null)
                {
                    existingCoin.CoinStock += coinToAdd.CoinStock;
                }
                else
                {
                    _coins.Add(new Coin(coinToAdd.CoinValue, coinToAdd.CoinStock));
                }
            }

            return Task.CompletedTask;
        }

        public async Task<List<Coin>> CalculateExchangeAsync(int changeToGive)
        {
            var exchange = new List<Coin>();
            var remainingChange = changeToGive;

            foreach (var coin in _coins.OrderByDescending(c => c.CoinValue))
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
                throw new Exception("No hay suficiente cambio disponible.");
            }

            await UpdateCoinStockAfterExchangeAsync(exchange);

            return exchange;
        }


    }
}
