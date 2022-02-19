using CompanyCrmApi.Data.Entities;
using CustomerModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data.Repositories
{
    public interface IAddressRepository : IGenericRepository
    {
        Task<Address> GetAddressAsync(int addressId);
        Task<Address[]> GetAllAddressesForCustomerAsync(int customerId);
        Task<Country[]> GetAllCountriesAsync();
        Task<Region[]> GetAllRegionsForCountryAsync(string countryCode);
    }
}
