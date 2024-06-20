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
    public class PlayerDataAdvancedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerDataAdvancedController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Multi paramter query
        /// </summary>
        /// <returns>Query by multiple paramters.</returns>
        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvanced>>> QueryPlayerDataAdvanced(
            string? playerName = null,
            int? season = null,
            string? team = null,
            string? playerId = null,
            string sortBy = "PlayerName",
            bool ascending = true,
            int pageNumber = 1,
            int pageSize = 10)
            {
                var query = _context.PlayerDataAdvanced.AsQueryable();

                if (!string.IsNullOrEmpty(playerName))
                {
                    query = query.Where(p => EF.Functions.Like(p.PlayerName.ToLower(), $"%{playerName.ToLower()}%"));
                }

                if (season.HasValue)
                {
                    query = query.Where(p=> p.Season == season.Value);
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

                var playerDataAdvanced = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (playerDataAdvanced == null || playerDataAdvanced.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataAdvanced);
            }

    /*[HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvanced>>> GetPlayerDataAdvanced()
        {
            return await _context.PlayerDataAdvanced.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDataAdvanced>> GetPlayerDataAdvanced(int id)
        {
            var playerDataAdvanced = await _context.PlayerDataAdvanced.FindAsync(id);

            if (playerDataAdvanced == null)
            {
                return NotFound();
            }

            return Ok(playerDataAdvanced);
        }*/

        /// <summary>
        /// Gets data for specfic player by name.
        /// </summary>
        /// <returns>A list of all data available for a specific player.</returns>
        [HttpGet("name/{playerName}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvanced>>> GetPlayerDataByName(string playerName)
        {
                var playerDataAdvanced = await _context.PlayerDataAdvanced
                    .Where(p => EF.Functions.Like(p.PlayerName.ToLower(), $"%{playerName.ToLower()}%"))
                    .ToListAsync();

                if (playerDataAdvanced == null || playerDataAdvanced.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataAdvanced);
        }

        /// <summary>
        /// Gets data for specfic all players in a season.
        /// </summary>
        /// <returns>All player data for a specified season.</returns>
        [HttpGet("season/{season}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvanced>>> GetPlayerDataBySeason(int season)
        {
            var playerDataAdvanced = await _context.PlayerDataAdvanced
                .Where(p => p.Season == season)
                .ToListAsync();

                if (playerDataAdvanced == null || playerDataAdvanced.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataAdvanced);
        }


        [HttpGet("playerid/{playerId}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvanced>>> GetPlayerDataAdvancedByPlayerId (string playerId)
        {
            var playerDataAdvanceds = await _context.PlayerDataAdvanced
                .Where(p => EF.Functions.Like(p.PlayerId, $"%{playerId}%"))
                .ToListAsync();

            if (playerDataAdvanceds == null || playerDataAdvanceds.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataAdvanceds);
        }

        /// <summary>
        /// Get all data, across all seasons for any team.
        /// </summary>
        /// <returns>Get all data, across all seasons for any team (abbreviation).</returns>
        [HttpGet("team/{team}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvanced>>> GetPlayerDataByTeam (string team)
        {
            var playerDataAdvanced = await _context.PlayerDataAdvanced
                .Where(p => EF.Functions.Like(p.Team, $"%{team}%"))
                .ToListAsync();
            
            if (playerDataAdvanced == null || playerDataAdvanced.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataAdvanced);
        }

        // New method to test the database connection
        /// <summary>
        ///  Method to test the database connection and retrieve player count.
        /// </summary>
        /// <returns>Method to test the database connection and retrieve player count.</returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataAdvancedCount()
        {
            var count = await _context.PlayerDataAdvanced.CountAsync();
            return Ok(count);
        }
    }

}