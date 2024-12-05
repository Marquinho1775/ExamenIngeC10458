// Infrastructure/Data/CoffeeRepository.cs
using CoffeeMachine.Domain.Entities;
using CoffeeMachine.Application.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeMachine.Infrastructure.Data
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private static readonly List<Coffee> _coffees = new List<Coffee>
        {
            new Coffee("Americano", 950, 10),
            new Coffee("Capuchino", 1200, 8),
            new Coffee("Late", 1350, 10),
            new Coffee("Mocachino", 1500, 15)
        };

        public Task<List<Coffee>> GetAllCoffeesAsync()
        {
            return Task.FromResult(_coffees);
        }

        public Task<Coffee> GetCoffeeByNameAsync(string coffeeName)
        {
            var coffee = _coffees.FirstOrDefault(c => c.CoffeeName.Equals(coffeeName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(coffee);
        }

        public Task UpdateCoffeeAsync(Coffee coffee)
        {
            // Como es una lista en memoria, los objetos se pasan por referencia y ya están actualizados.
            return Task.CompletedTask;
        }
    }
}
