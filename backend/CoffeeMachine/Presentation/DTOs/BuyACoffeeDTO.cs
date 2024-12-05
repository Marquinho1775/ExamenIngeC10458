using CoffeeMachine.Domain.Entities;

namespace CoffeeMachine.Presentation.DTOs
{
    public class BuyACoffeeDTO
    {
        public List<Coffee> coffees;
        public List<Coin> coins;
    }
}
