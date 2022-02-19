using CompanyCrmApi.Data.Entities;
using CustomerModels;
using CustomerModels.Battleship;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data
{
    public class CompanyCrmContext : DbContext
    {
        private readonly IConfiguration _config;
        
        public CompanyCrmContext(DbContextOptions options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Coordinate> Coordinates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("CompanyCrmDBConnectionString"));

            //// Remove SensitiveDataLogging prior to production
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
