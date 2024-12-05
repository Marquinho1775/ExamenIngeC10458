using CoffeeMachine.Domain.Entities;
using CoffeeMachine.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeMachine.Application.Services
{
    public class CoffeeService
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public CoffeeService(ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        public async Task<List<Coffee>> GetAllCoffeesAsync()
        {
            return await _coffeeRepository.GetAllCoffeesAsync();
        }

        public async Task UpdateCoffeeStockAsync()
        {
            await _coffeeRepository.UpdateCoffeeStockAsync();
        }
    }
}
