﻿using CoffeeMachine.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CoffeeMachine.Application.Interfaces
{
    public interface ICoffeeRepository
    {
        Task<List<Coffee>> GetAllCoffeesAsync();
        Task UpdateCoffeeStockAsync();
    }
}