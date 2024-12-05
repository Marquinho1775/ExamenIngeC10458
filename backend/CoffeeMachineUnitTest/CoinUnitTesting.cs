using NUnit.Framework;
using Moq;
using CoffeeMachine.Application.Services;
using CoffeeMachine.Application.Interfaces;
using CoffeeMachine.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace CoffeeMachineUnitTests.Application.Services
{
    [TestFixture]
    public class CoinUnitTesting
    {
        private Mock<ICoinRepository> _coinRepositoryMock;
        private CoinService _coinService;

        [SetUp]
        public void SetUp()
        {
            _coinRepositoryMock = new Mock<ICoinRepository>();
            _coinService = new CoinService(_coinRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllCoinsAsync_ShouldReturnListOfCoins()
        {
            // Arrange
            var coins = new List<Coin>
            {
                new Coin(500, 20),
                new Coin(100, 30)
            };
            _coinRepositoryMock.Setup(repo => repo.GetAllCoinsAsync()).ReturnsAsync(coins);

            // Act
            var result = await _coinService.GetAllCoinsAsync();

            // Assert
            Assert.That(result, Is.Not.Null); // Verifica que el resultado no sea nulo
            Assert.That(result.Count, Is.EqualTo(2)); // Verifica que la lista tenga dos elementos
            _coinRepositoryMock.Verify(repo => repo.GetAllCoinsAsync(), Times.Once);
        }

        [Test]
        public async Task RegisterCoinsAsync_ShouldUpdateCoinStock_WhenCoinsExist()
        {
            // Arrange
            var coinsToAdd = new List<Coin>
            {
                new Coin(500, 5),
                new Coin(100, 10)
            };

            var existingCoins = new List<Coin>
            {
                new Coin(500, 20),
                new Coin(100, 30)
            };

            _coinRepositoryMock.Setup(repo => repo.GetAllCoinsAsync()).ReturnsAsync(existingCoins);
            _coinRepositoryMock.Setup(repo => repo.UpdateCoinAsync(It.IsAny<Coin>())).Returns(Task.CompletedTask);

            // Act
            await _coinService.RegisterCoinsAsync(coinsToAdd);

            // Assert
            var updated500Coin = existingCoins.First(c => c.CoinValue == 500);
            var updated100Coin = existingCoins.First(c => c.CoinValue == 100);
            Assert.That(updated500Coin.CoinStock, Is.EqualTo(25)); // Verifica que el stock se incrementa correctamente
            Assert.That(updated100Coin.CoinStock, Is.EqualTo(40));
            _coinRepositoryMock.Verify(repo => repo.UpdateCoinAsync(It.IsAny<Coin>()), Times.Exactly(2));
        }

        [Test]
        public async Task RegisterCoinsAsync_ShouldAddNewCoins_WhenCoinsDoNotExist()
        {
            // Arrange
            var coinsToAdd = new List<Coin>
            {
                new Coin(50, 10),
                new Coin(25, 15)
            };

            var existingCoins = new List<Coin>
            {
                new Coin(500, 20),
                new Coin(100, 30)
            };

            _coinRepositoryMock.Setup(repo => repo.GetAllCoinsAsync()).ReturnsAsync(existingCoins);

            // Act
            await _coinService.RegisterCoinsAsync(coinsToAdd);

            // Assert
            Assert.That(existingCoins.Count, Is.EqualTo(4)); // Verifica que se agregaron dos nuevas monedas
            var new50Coin = existingCoins.FirstOrDefault(c => c.CoinValue == 50);
            var new25Coin = existingCoins.FirstOrDefault(c => c.CoinValue == 25);
            Assert.That(new50Coin, Is.Not.Null);
            Assert.That(new25Coin, Is.Not.Null);
            Assert.That(new50Coin.CoinStock, Is.EqualTo(10));
            Assert.That(new25Coin.CoinStock, Is.EqualTo(15));
            _coinRepositoryMock.Verify(repo => repo.UpdateCoinAsync(It.IsAny<Coin>()), Times.Never);
        }

        [Test]
        public async Task CalculateExchangeAsync_ShouldReturnExchange_WhenChangeIsPossible()
        {
            // Arrange
            int changeToGive = 650;
            var existingCoins = new List<Coin>
            {
                new Coin(500, 10),
                new Coin(100, 10),
                new Coin(50, 10)
            };

            _coinRepositoryMock.Setup(repo => repo.GetAllCoinsAsync()).ReturnsAsync(existingCoins);
            _coinRepositoryMock.Setup(repo => repo.UpdateCoinAsync(It.IsAny<Coin>())).Returns(Task.CompletedTask);

            // Act
            var exchange = await _coinService.CalculateExchangeAsync(changeToGive);

            // Assert
            Assert.That(exchange, Is.Not.Null);
            Assert.That(exchange.Count, Is.EqualTo(3));

            var coin500 = exchange.First(c => c.CoinValue == 500);
            var coin100 = exchange.First(c => c.CoinValue == 100);
            var coin50 = exchange.First(c => c.CoinValue == 50);

            Assert.That(coin500.CoinStock, Is.EqualTo(1));
            Assert.That(coin100.CoinStock, Is.EqualTo(1));
            Assert.That(coin50.CoinStock, Is.EqualTo(1));

            var updatedCoin500 = existingCoins.First(c => c.CoinValue == 500);
            var updatedCoin100 = existingCoins.First(c => c.CoinValue == 100);
            var updatedCoin50 = existingCoins.First(c => c.CoinValue == 50);

            Assert.That(updatedCoin500.CoinStock, Is.EqualTo(9));
            Assert.That(updatedCoin100.CoinStock, Is.EqualTo(9));
            Assert.That(updatedCoin50.CoinStock, Is.EqualTo(9));

            _coinRepositoryMock.Verify(repo => repo.UpdateCoinAsync(It.IsAny<Coin>()), Times.Exactly(3));
        }

        [Test]
        public void CalculateExchangeAsync_ShouldThrowException_WhenChangeIsNotPossible()
        {
            // Arrange
            int changeToGive = 650;
            var existingCoins = new List<Coin>
            {
                new Coin(500, 0),
                new Coin(100, 0),
                new Coin(50, 0)
            };

            _coinRepositoryMock.Setup(repo => repo.GetAllCoinsAsync()).ReturnsAsync(existingCoins);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _coinService.CalculateExchangeAsync(changeToGive));
            Assert.That(ex.Message, Is.EqualTo("No hay suficiente cambio disponible."));
        }

        [Test]
        public void CanGiveExactChange_ShouldReturnTrue_WhenChangeIsPossible()
        {
            // Arrange
            int changeToGive = 650;
            var existingCoins = new List<Coin>
            {
                new Coin(500, 10),
                new Coin(100, 10),
                new Coin(50, 10)
            };

            _coinRepositoryMock.Setup(repo => repo.GetAllCoinsAsync()).ReturnsAsync(existingCoins);

            // Act
            var result = _coinService.CanGiveExactChange(changeToGive);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanGiveExactChange_ShouldReturnFalse_WhenChangeIsNotPossible()
        {
            // Arrange
            int changeToGive = 650;
            var existingCoins = new List<Coin>
            {
                new Coin(500, 0),
                new Coin(100, 0),
                new Coin(50, 0)
            };

            _coinRepositoryMock.Setup(repo => repo.GetAllCoinsAsync()).ReturnsAsync(existingCoins);

            // Act
            var result = _coinService.CanGiveExactChange(changeToGive);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
