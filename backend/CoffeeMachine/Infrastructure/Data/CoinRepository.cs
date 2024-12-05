// Infrastructure/Data/CoinRepository.cs
using CoffeeMachine.Domain.Entities;
using CoffeeMachine.Application.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

        public Task<Coin> GetCoinByValueAsync(int coinValue)
        {
            var coin = _coins.FirstOrDefault(c => c.CoinValue == coinValue);
            return Task.FromResult(coin);
        }

        public Task UpdateCoinAsync(Coin coin)
        {
            // Como es una lista en memoria, no es necesario hacer mucho más
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
    }
}
