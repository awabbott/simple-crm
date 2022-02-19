using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data.Repositories
{
    public class GenericRepository
    {
        private readonly CompanyCrmContext _context;
        private readonly ILogger<CompanyCrmContext> _logger;

        public GenericRepository(CompanyCrmContext context, ILogger<CompanyCrmContext> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempting to save the changes in the context");

            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
