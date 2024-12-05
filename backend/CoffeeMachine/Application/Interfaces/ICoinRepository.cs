// Application/Interfaces/ICoinRepository.cs
using CoffeeMachine.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CoffeeMachine.Application.Interfaces
{
    public interface ICoinRepository
    {
        Task<List<Coin>> GetAllCoinsAsync();
        Task<Coin> GetCoinByValueAsync(int coinValue);
        Task UpdateCoinAsync(Coin coin);
        Task RegisterCoinsAsync(List<Coin> coinsToAdd);
    }
}
