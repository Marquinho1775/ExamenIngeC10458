using CoffeeMachine.Domain.Entities;
using CoffeeMachine.Application.Interfaces;

namespace CoffeeMachine.Infrastructure.Data
{
    public class CoinRepository : ICoinRepository
    {
        private readonly List<Coin> _coins;

        public CoinRepository()
        {
            _coins = new List<Coin>
            {
                new Coin(500, 20),
                new Coin(100, 30),
                new Coin(50, 50),
                new Coin(25, 25)
            };
        }

        public Task<List<Coin>> GetAllCoinsAsync()
        {
            return Task.FromResult(_coins);
        }

        public Task<bool> UpdateCoinStockAsync()
        {
            return Task.FromResult(true);
        }

        public Task<List<Coin>> GetExchangeAsync(List<Coffee> coffeesToBuy, int totalToPay)
        {
            return Task.FromResult(new List<Coin>());
        }
    }
}
