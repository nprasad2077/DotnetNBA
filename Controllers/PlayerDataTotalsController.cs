using Microsoft.AspNetCore.Mvc;
using DotnetNBA.Data;
using DotnetNBA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotnetNBA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerDataTotalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerDataTotalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all player totals data.
        /// </summary>
        /// <returns>A list all player totals data.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDataTotals>>> GetPlayerDataTotals()
        {
            return await _context.PlayerDataTotals.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDataTotals>> GetPlayerDataTotal(int id)
        {
            var playerDataTotal = await _context.PlayerDataTotals.FindAsync(id);

            if (playerDataTotal == null)
            {
                return NotFound();
            }
            return Ok(playerDataTotal);
        }

        [HttpGet("name/{playerName}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotals>>> GetPlayerDataByName(string playerName)
        {
            var playerDataTotals = await _context.PlayerDataTotals
                .Where(p => EF.Functions.Like(p.PlayerName, $"%{playerName}%"))
                .ToListAsync();

            if (playerDataTotals == null || playerDataTotals.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataTotals);
        }

        [HttpGet("season/{season}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotals>>> GetPlayerDataBySeason(int season)
        {
            var playerDataTotals = await _context.PlayerDataTotals
                .Where(p => p.Season == season)
                .ToListAsync();

            if (playerDataTotals == null || playerDataTotals.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataTotals);
        }

        [HttpGet("playerid/{playerId}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotals>>> GetPlayerDataTotalsByPlayerId(string playerId)
        {
            var playerDataTotals = await _context.PlayerDataTotals
                .Where(p => EF.Functions.Like(p.PlayerId, $"%{playerId}%"))
                .ToListAsync();

            if (playerDataTotals == null || playerDataTotals.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataTotals);
        }

        [HttpGet("team/{team}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotals>>> GetPlayerDataByTeam(string team)
        {
            var playerDataTotals = await _context.PlayerDataTotals
                .Where(p => EF.Functions.Like(p.Team, $"%{team}%"))
                .ToListAsync();

            if (playerDataTotals == null || playerDataTotals.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataTotals);
        }

        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotals>>> QueryPlayerDataTotals(
            string? playerName = null,
            int? season = null,
            string? team = null,
            string? playerId = null,
            string sortBy = "PlayerName",
            bool ascending = true,
            int pageNumber = 1,
            int pageSize = 10)
            {
                var query = _context.PlayerDataTotals.AsQueryable();

                if (!string.IsNullOrEmpty(playerName))
                {
                    query = query.Where(p => EF.Functions.Like(p.PlayerName, $"%{playerName}%"));
                }

                if (season.HasValue)
                {
                    query = query.Where(p=> p.Season == season.Value);
                }

                if (!string.IsNullOrEmpty(team))
                {
                    query = query.Where(p => EF.Functions.Like(p.PlayerName, $"%{team}%"));
                }

                if (!string.IsNullOrEmpty(playerId))
                {
                    query = query.Where(p => EF.Functions.Like(p.PlayerName, $"%{playerId}%"));
                }

                query = sortBy switch
                {
                    "PlayerName" => ascending ? query.OrderBy(p => p.PlayerName) : query.OrderByDescending(p => p.PlayerName),
                    "Season" => ascending ? query.OrderBy(p => p.Season) : query.OrderByDescending(p => p.Season),
                    "Team" => ascending ? query.OrderBy(p => p.Team) : query.OrderByDescending(p => p.Team),
                    _ => query.OrderBy(p => p.PlayerName)
                };

                var playerDataTotals = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (playerDataTotals == null || playerDataTotals.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataTotals);
            }

        // New method to test the database connection
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataTotalsCount()
        {
            var count = await _context.PlayerDataTotals.CountAsync();
            return Ok(count);
        }
    }
}
