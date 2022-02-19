using CompanyCrmApi.Data.Entities;
using CustomerModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data.Repositories
{
    public interface ICustomerRepository : IGenericRepository
    {
        Task<Customer[]> GetAllCustomersAsync();
        Task<Customer> GetCustomerAsync(int customerId, bool includeAddresses = false);
        Task<Customer[]> GetAllCustomersForUserAsync(string userId);
    }
}
