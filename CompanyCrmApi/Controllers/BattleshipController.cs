using CompanyCrmApi.Data.Repositories;
using CustomerModels.Battleship;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CompanyCrmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BattleshipController : ControllerBase
    {
        private readonly IBattleshipRepository _repository;

        public BattleshipController(IBattleshipRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("[action]/{shipId:int}")]
        public async Task<ActionResult<Ship>> GetShip(int shipId)
        {
            try
            {
                Ship ship = await _repository.GetShipAsync(shipId);

                if (ship == null)
                {
                    return NotFound($"Could not find ship with ID: {shipId}");
                }

                return ship;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.InnerException}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<bool>> Get(int gameId, bool isUser, int row, int column)
        {
            try
            {
                Coordinate coordinate = await _repository.GetCoordinateAsync(gameId, isUser, row, column);

                if (coordinate == null)
                {
                    return NotFound(false);
                }
                else
                {
                    Ship shipFiredOn = await IncrementShipHitCount(coordinate.ShipId);
                    if (shipFiredOn == null)
                    {
                        return BadRequest($"Failed to update hit count to ship ID: {coordinate.ShipId}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.InnerException}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Game>> Post([FromBody] Game game)
        {
            try
            {
                _repository.Add(game);

                if (await _repository.SaveChangesAsync())
                {
                    return new ObjectResult(game) { StatusCode = StatusCodes.Status201Created };
                }
                else
                {
                    return BadRequest("Failed to save fleet");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.InnerException}");
            }
        }

        public async Task<Ship> IncrementShipHitCount(int shipId)
        {
            Ship ship = await _repository.GetShipAsync(shipId);

            if (ship == null) return null;
            
            ship.HitCount++;

            if (await _repository.SaveChangesAsync())
            {
                return ship;
            }
            else
            {
                return null;
            }
        }
    }
}
