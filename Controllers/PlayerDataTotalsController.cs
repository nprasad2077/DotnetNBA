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
        /// Multi paramter query
        /// </summary>
        /// <returns>Query by multiple paramters.</returns>
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
                query = query.Where(p => EF.Functions.Like(p.PlayerName.ToLower(), $"%{playerName.ToLower()}%"));
            }

            if (season.HasValue)
            {
                query = query.Where(p => p.Season == season.Value);
            }

            if (!string.IsNullOrEmpty(team))
            {
                query = query.Where(p => EF.Functions.Like(p.Team, $"%{team}%"));
            }

            if (!string.IsNullOrEmpty(playerId))
            {
                query = query.Where(p => EF.Functions.Like(p.PlayerId, $"%{playerId}%"));
            }

            query = sortBy switch
            {
                "PlayerName" => ascending ? query.OrderBy(p => p.PlayerName) : query.OrderByDescending(p => p.PlayerName),
                "Season" => ascending ? query.OrderBy(p => p.Season) : query.OrderByDescending(p => p.Season),
                "Team" => ascending ? query.OrderBy(p => p.Team) : query.OrderByDescending(p => p.Team),
                "Points" => ascending ? query.OrderBy(p => p.Points) : query.OrderByDescending(p => p.Points),
                "Assists" => ascending ? query.OrderBy(p => p.Assists) : query.OrderByDescending(p => p.Assists),
                "Games" => ascending ? query.OrderBy(p => p.Games) : query.OrderByDescending(p => p.Games),
                "TotalRb" => ascending ? query.OrderBy(p => p.Team) : query.OrderByDescending(p => p.Team),
                "Blocks" => ascending ? query.OrderBy(p => p.Blocks) : query.OrderByDescending(p => p.Blocks),
                "Steals" => ascending ? query.OrderBy(p => p.Steals) : query.OrderByDescending(p => p.Steals),
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

        /*/// <summary>
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
        }*/

        /// <summary>
        /// Gets data for specfic player by name.
        /// </summary>
        /// <returns>A list of all data available for a specific player.</returns>
        [HttpGet("name/{playerName}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotals>>> GetPlayerDataByName(string playerName)
        {
            var playerDataTotals = await _context.PlayerDataTotals
                .Where(p => EF.Functions.Like(p.PlayerName.ToLower(), $"%{playerName.ToLower()}%"))
                .ToListAsync();

            if (playerDataTotals == null || playerDataTotals.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataTotals);
        }

        /// <summary>
        /// Gets data for specfic all players in a season.
        /// </summary>
        /// <returns>All player data for a specified season.</returns>
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

        /// <summary>
        /// Get all data, across all seasons for any team.
        /// </summary>
        /// <returns>Get all data, across all seasons for any team (abbreviation).</returns>
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


        // New method to test the database connection
        /// <summary>
        ///  Method to test the database connection and retrieve player count.
        /// </summary>
        /// <returns>Method to test the database connection and retrieve player count.</returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataTotalsCount()
        {
            var count = await _context.PlayerDataTotals.CountAsync();
            return Ok(count);
        }
    }
}
