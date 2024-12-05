using CoffeeMachine.Domain.Entities;

namespace CoffeeMachine.Application.Interfaces
{
    public interface ICoinRepository
    {
        Task<List<Coin>> GetAllCoinsAsync();
        Task UpdateCoinStockAfterExchangeAsync(List<Coin> usedCoins);
        Task RegisterCoinsAsync(List<Coin> coinsToAdd);

        Task<List<Coin>> CalculateExchangeAsync(int changeToGive);
    }
}
