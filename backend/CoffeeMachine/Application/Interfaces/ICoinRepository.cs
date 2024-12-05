using CoffeeMachine.Domain.Entities;

namespace CoffeeMachine.Application.Interfaces
{
    public interface ICoinRepository
    {
        Task<List<Coin>> GetAllCoinsAsync();
        Task<bool> UpdateCoinStockAsync();
        Task<List<Coin>> GetExchangeAsync(List<Coffee> coffeesToBuy, int totalToPay);
    }
}
