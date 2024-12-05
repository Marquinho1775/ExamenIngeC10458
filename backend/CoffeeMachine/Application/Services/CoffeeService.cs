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

        public async Task PurchaseCoffeeAsync(string coffeeName, int quantityToBuy)
        {
            if (!IsValidQuantity(quantityToBuy))
            {
                throw new ArgumentException("La cantidad a comprar debe ser mayor que cero.");
            }

            var coffee = await _coffeeRepository.GetCoffeeByNameAsync(coffeeName);
            if (coffee == null)
            {
                throw new Exception($"El café '{coffeeName}' no existe.");
            }

            if (!IsStockAvailable(coffee, quantityToBuy))
            {
                throw new Exception($"No hay suficiente stock para '{coffeeName}'. Stock disponible: {coffee.CoffeeStock}");
            }

            coffee.CoffeeStock -= quantityToBuy;

            await _coffeeRepository.UpdateCoffeeAsync(coffee);
        }

        private bool IsValidQuantity(int quantity)
        {
            return quantity > 0;
        }

        private bool IsStockAvailable(Coffee coffee, int quantityRequested)
        {
            return coffee.CoffeeStock >= quantityRequested;
        }
    }
}
