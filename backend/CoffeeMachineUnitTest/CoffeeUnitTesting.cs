using NUnit.Framework;
using Moq;
using CoffeeMachine.Application.Services;
using CoffeeMachine.Application.Interfaces;
using CoffeeMachine.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CoffeeMachineUnitTests.Application.Services
{
    [TestFixture]
    public class CoffeeServiceTests
    {
        private Mock<ICoffeeRepository> _coffeeRepositoryMock;
        private CoffeeService _coffeeService;

        [SetUp]
        public void SetUp()
        {
            _coffeeRepositoryMock = new Mock<ICoffeeRepository>();
            _coffeeService = new CoffeeService(_coffeeRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllCoffeesAsync_ShouldReturnListOfCoffees()
        {
            // Arrange
            var coffees = new List<Coffee>
            {
                new Coffee("Americano", 950, 10),
                new Coffee("Capuchino", 1200, 8)
            };
            _coffeeRepositoryMock.Setup(repo => repo.GetAllCoffeesAsync()).ReturnsAsync(coffees);

            // Act
            var result = await _coffeeService.GetAllCoffeesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            _coffeeRepositoryMock.Verify(repo => repo.GetAllCoffeesAsync(), Times.Once);
        }

        [Test]
        public async Task PurchaseCoffeeAsync_ShouldDecreaseStock_WhenValidRequest()
        {
            // Arrange
            var coffeeName = "Americano";
            int quantityToBuy = 5;
            var coffee = new Coffee(coffeeName, 950, 10);

            _coffeeRepositoryMock.Setup(repo => repo.GetCoffeeByNameAsync(coffeeName)).ReturnsAsync(coffee);
            _coffeeRepositoryMock.Setup(repo => repo.UpdateCoffeeAsync(It.IsAny<Coffee>())).Returns(Task.CompletedTask);

            // Act
            await _coffeeService.PurchaseCoffeeAsync(coffeeName, quantityToBuy);

            // Assert
            Assert.That(coffee.CoffeeStock, Is.EqualTo(5));
            _coffeeRepositoryMock.Verify(repo => repo.GetCoffeeByNameAsync(coffeeName), Times.Once);
            _coffeeRepositoryMock.Verify(repo => repo.UpdateCoffeeAsync(coffee), Times.Once);
        }

        [Test]
        public void PurchaseCoffeeAsync_ShouldThrowArgumentException_WhenQuantityIsInvalid()
        {
            // Arrange
            var coffeeName = "Americano";
            int quantityToBuy = 0;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _coffeeService.PurchaseCoffeeAsync(coffeeName, quantityToBuy));
            Assert.That(ex.Message, Is.EqualTo("La cantidad a comprar debe ser mayor que cero."));
        }

        [Test]
        public void PurchaseCoffeeAsync_ShouldThrowException_WhenCoffeeDoesNotExist()
        {
            // Arrange
            var coffeeName = "Desconocido";
            int quantityToBuy = 2;

            _coffeeRepositoryMock.Setup(repo => repo.GetCoffeeByNameAsync(coffeeName)).ReturnsAsync((Coffee)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _coffeeService.PurchaseCoffeeAsync(coffeeName, quantityToBuy));
            Assert.That(ex.Message, Is.EqualTo($"El café '{coffeeName}' no existe."));
        }

        [Test]
        public void PurchaseCoffeeAsync_ShouldThrowException_WhenInsufficientStock()
        {
            // Arrange
            var coffeeName = "Americano";
            int quantityToBuy = 15;
            var coffee = new Coffee(coffeeName, 950, 10);

            _coffeeRepositoryMock.Setup(repo => repo.GetCoffeeByNameAsync(coffeeName)).ReturnsAsync(coffee);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _coffeeService.PurchaseCoffeeAsync(coffeeName, quantityToBuy));
            Assert.That(ex.Message, Is.EqualTo($"No hay suficiente stock para '{coffeeName}'. Stock disponible: {coffee.CoffeeStock}"));
        }
    }
}
