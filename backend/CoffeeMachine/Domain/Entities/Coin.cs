namespace CoffeeMachine.Domain.Entities
{
    public class Coin
    {
        public int CoinValue { get; set; }
        public int CoinStock { get; set; }

        public Coin(int coinValue, int coinStock)
        {
            CoinValue = coinValue;
            CoinStock = coinStock;
        }
    }
}
