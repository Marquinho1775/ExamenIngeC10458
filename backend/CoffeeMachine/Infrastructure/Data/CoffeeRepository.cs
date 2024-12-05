using CoffeeMachine.Domain.Entities;
using CoffeeMachine.Application.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public async Task UpdateCoffeeStockAsync(string coffeeName, int quantityToBuy)
        {
            var coffee = _coffees.FirstOrDefault(c => c.CoffeeName == coffeeName);
            if (coffee == null)
            {
                throw new Exception($"El café '{coffeeName}' no existe.");
            }

            if (coffee.CoffeeStock < quantityToBuy)
            {
                throw new Exception($"No hay suficiente stock para '{coffeeName}'. Stock disponible: {coffee.CoffeeStock}");
            }

            coffee.CoffeeStock -= quantityToBuy;

            // Simula un retraso para representar un proceso asincrónico (opcional)
            await Task.CompletedTask;
        }

    }
}
