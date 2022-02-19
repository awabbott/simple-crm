using CustomerModels.Battleship;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data.Repositories
{
    public class BattleshipRepository : GenericRepository, IBattleshipRepository
    {
        private readonly CompanyCrmContext _context;
        private readonly ILogger<CompanyCrmContext> _logger;

        public BattleshipRepository(CompanyCrmContext context, ILogger<CompanyCrmContext> logger)
            : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<Ship> GetShipAsync(int shipId)
        {
            _logger.LogInformation($"Getting ship with ID: {shipId}");

            IQueryable<Ship> query = _context.Ships
                .Where(s => s.ShipId == shipId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Coordinate> GetCoordinateAsync(int gameId, bool isUser, int row, int column)
        {
            _logger.LogInformation("Checking for presence of coordinate");

            IQueryable<Ship> query = _context.Ships.Include(nameof(Ship.Coordinates))
                .Where(s => s.GameId == gameId)
                .Where(s => s.BelongsToUser == isUser);

            List<Ship> ships = await query.ToListAsync();

            foreach (var ship in ships)
            {
                foreach (var coordinate in ship.Coordinates)
                {
                    if (coordinate.Row == row && coordinate.Column == column)
                    {
                        return coordinate;
                    }
                }
            }

            return null;
        }
    }
}
