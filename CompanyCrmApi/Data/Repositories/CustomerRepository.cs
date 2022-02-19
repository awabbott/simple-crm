using CompanyCrmApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data.Repositories
{
    public class CustomerRepository : GenericRepository, ICustomerRepository
    {
        private readonly CompanyCrmContext _context;
        private readonly ILogger<CompanyCrmContext> _logger;

        public CustomerRepository(CompanyCrmContext context, ILogger<CompanyCrmContext> logger)
            : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<Customer[]> GetAllCustomersAsync()
        {
            _logger.LogInformation("Getting all customers");

            IQueryable<Customer> query = _context.Customers
                .OrderBy(c => c.LastName);

            return await query.ToArrayAsync();
        }
        
        public async Task<Customer> GetCustomerAsync(int customerId, bool includeAddresses = false)
        {
            _logger.LogInformation($"Getting customer with ID: {customerId}");

            IQueryable<Customer> query = _context.Customers;

            if (includeAddresses)
            {
                query = query
                  .Include(c => c.Addresses);
            }

            query = query.Where(c => c.CustomerId == customerId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Customer[]> GetAllCustomersForUserAsync(string userId)
        {
            _logger.LogInformation($"Getting all customers for User ID: {userId}");

            IQueryable<Customer> query = _context.Customers
              .Where(c => c.UserId == userId)
              .OrderBy(c => c.LastName);

            return await query.ToArrayAsync();
        }
    }
}