using CoffeeMachine.Domain.Entities;
using CoffeeMachine.Application.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CoffeeMachine.Infrastructure.Data
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly List<Coffee> _coffees;

        public CoffeeRepository()
        {
            _coffees = new List<Coffee>
            {
                new Coffee("Americano", 950, 10),
                new Coffee("Capuchino", 1200, 8),
                new Coffee("Late", 1350, 10),
                new Coffee("Mocachino", 1500, 15)
            };
        }

        public Task<List<Coffee>> GetAllCoffeesAsync()
        {
            return Task.FromResult(_coffees);
        }

        public Task UpdateCoffeeStockAsync()
        {

            return Task.CompletedTask;
        }
    }
}
