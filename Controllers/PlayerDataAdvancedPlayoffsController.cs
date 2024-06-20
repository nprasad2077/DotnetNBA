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
    public class PlayerDataAdvancedPlayoffsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerDataAdvancedPlayoffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Multi paramter query
        /// </summary>
        /// <returns>Query by multiple paramters.</returns>
        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> QueryPlayerDataAdvancedPlayoffs(
            string? playerName = null,
            int? season = null,
            string? team = null,
            string? playerId = null,
            string sortBy = "PlayerName",
            bool ascending = true,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.PlayerDataAdvancedPlayoffs.AsQueryable();

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
                "Games" => ascending ? query.OrderBy(p => p.Games) : query.OrderByDescending(p => p.Games),
                "PER" => ascending ? query.OrderBy(p => p.PER) : query.OrderByDescending(p => p.PER),
                "TSPercent" => ascending ? query.OrderBy(p => p.TSPercent) : query.OrderByDescending(p => p.TSPercent),
                "TotalRBPercent" => ascending ? query.OrderBy(p => p.TotalRBPercent) : query.OrderByDescending(p => p.TotalRBPercent),
                "AssistPercent" => ascending ? query.OrderBy(p => p.AssistPercent) : query.OrderByDescending(p => p.AssistPercent),
                "StealPercent" => ascending ? query.OrderBy(p => p.StealPercent) : query.OrderByDescending(p => p.StealPercent),
                "BlockPercent" => ascending ? query.OrderBy(p => p.BlockPercent) : query.OrderByDescending(p => p.BlockPercent),
                "TurnoverPercent" => ascending ? query.OrderBy(p => p.TurnoverPercent) : query.OrderByDescending(p => p.TurnoverPercent),
                "UsagePercent" => ascending ? query.OrderBy(p => p.UsagePercent) : query.OrderByDescending(p => p.UsagePercent),
                "WinShares" => ascending ? query.OrderBy(p => p.WinShares) : query.OrderByDescending(p => p.WinShares),
                "Box" => ascending ? query.OrderBy(p => p.Box) : query.OrderByDescending(p => p.Box),
                "VORP" => ascending ? query.OrderBy(p => p.VORP) : query.OrderByDescending(p => p.VORP),
                _ => query.OrderBy(p => p.PlayerName)
            };

            var playerDataAdvancedPlayoffs = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (playerDataAdvancedPlayoffs == null || playerDataAdvancedPlayoffs.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataAdvancedPlayoffs);
        }

        /*
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataAdvancedPlayoffs()
        {
            return await _context.PlayerDataAdvancedPlayoffs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDataAdvancedPlayoffs>> GetPlayerDataAdvancedPlayoffs(int id)
        {
            var playerDataAdvancedPlayoffs = await _context.PlayerDataAdvancedPlayoffs.FindAsync(id);

            if (playerDataAdvancedPlayoffs == null)
            {
                return NotFound();
            }

            return Ok(playerDataAdvancedPlayoffs);
        }*/

        /// <summary>
        /// Gets data for specfic player by name.
        /// </summary>
        /// <returns>A list of all data available for a specific player.</returns>
        [HttpGet("name/{playerName}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataByName(string playerName)
        {
            var playerDataAdvancedPlayoffs = await _context.PlayerDataAdvancedPlayoffs
                .Where(p => EF.Functions.Like(p.PlayerName.ToLower(), $"%{playerName.ToLower()}%"))
                .ToListAsync();

            if (playerDataAdvancedPlayoffs == null || playerDataAdvancedPlayoffs.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataAdvancedPlayoffs);
        }


        /// <summary>
        /// Gets data for specfic all players in a season.
        /// </summary>
        /// <returns>All player data for a specified season.</returns>
        [HttpGet("season/{season}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataBySeason(int season)
        {
            var playerDataAdvancedPlayoffs = await _context.PlayerDataAdvancedPlayoffs
                .Where(p => p.Season == season)
                .ToListAsync();

            if (playerDataAdvancedPlayoffs == null || playerDataAdvancedPlayoffs.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataAdvancedPlayoffs);
        }


        [HttpGet("playerid/{playerId}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataAdvancedPlayoffsByPlayerId(string playerId)
        {
            var playerDataAdvancedPlayoffs = await _context.PlayerDataAdvancedPlayoffs
                .Where(p => EF.Functions.Like(p.PlayerId, $"%{playerId}%"))
                .ToListAsync();

            if (playerDataAdvancedPlayoffs == null || playerDataAdvancedPlayoffs.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataAdvancedPlayoffs);
        }

        /// <summary>
        /// Get all data, across all seasons for any team.
        /// </summary>
        /// <returns>Get all data, across all seasons for any team (abbreviation).</returns>
        [HttpGet("team/{team}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataByTeam(string team)
        {
            var playerDataAdvancedPlayoffs = await _context.PlayerDataAdvancedPlayoffs
                .Where(p => EF.Functions.Like(p.Team, $"%{team}%"))
                .ToListAsync();

            if (playerDataAdvancedPlayoffs == null || playerDataAdvancedPlayoffs.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataAdvancedPlayoffs);
        }

        // New method to test the database connection
        /// <summary>
        ///  Method to test the database connection and retrieve player count.
        /// </summary>
        /// <returns>Method to test the database connection and retrieve player count.</returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataAdvancedPlayoffsCount()
        {
            var count = await _context.PlayerDataAdvancedPlayoffs.CountAsync();
            return Ok(count);
        }
    }

}