using CustomerModels.Battleship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data.Repositories
{
    public interface IBattleshipRepository : IGenericRepository
    {
        Task<Ship> GetShipAsync(int shipId);
        Task<Coordinate> GetCoordinateAsync(int gameId, bool isUser, int row, int column);
    }
}
