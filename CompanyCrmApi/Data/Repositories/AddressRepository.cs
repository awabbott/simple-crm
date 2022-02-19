using CompanyCrmApi.Data.Entities;
using CustomerModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data.Repositories
{
    public class AddressRepository : GenericRepository, IAddressRepository
    {
        private readonly CompanyCrmContext _context;
        private readonly ILogger<CompanyCrmContext> _logger;

        public AddressRepository(CompanyCrmContext context, ILogger<CompanyCrmContext> logger)
            : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<Address> GetAddressAsync(int addressId)
        {
            _logger.LogInformation($"Getting address with ID: {addressId}");

            IQueryable<Address> query = _context.Addresses
                .Where(a => a.AddressId == addressId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Address[]> GetAllAddressesForCustomerAsync(int customerId)
        {
            _logger.LogInformation($"Getting all addresses for Customer ID: {customerId}");

            IQueryable<Address> query = _context.Addresses
                .Where(a => a.CustomerId == customerId)
                .OrderBy(a => a.Country)
                .ThenBy(a => a.State)
                .ThenBy(a => a.City);

            return await query.ToArrayAsync();
        }

        public async Task<Country[]> GetAllCountriesAsync()
        {
            _logger.LogInformation("Getting all countries.");

            IQueryable<Country> countries = _context.Countries.AsNoTracking()
                .OrderBy(c => c.Name);

            return await countries.ToArrayAsync();
        }

        public async Task<Region[]> GetAllRegionsForCountryAsync(string countryCode)
        {
            _logger.LogInformation("Getting list of regions");

            IQueryable<Region> query = _context.Regions
                .Where(r => r.CountryCode == countryCode)
                .OrderBy(r => r.Name);

            return await query.ToArrayAsync();
        }
    }
}
