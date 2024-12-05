namespace CoffeeMachine.Domain.Entities
{
    public class Coffee
    {
        public string CoffeeName { get; set; }
        public int CoffeePrice { get; set; }
        public int CoffeeStock { get; set; }

        public Coffee(string coffeeName, int coffeePrice, int coffeeStock)
        {
            CoffeeName = coffeeName;
            CoffeePrice = coffeePrice;
            CoffeeStock = coffeeStock;
        }
    }
}
