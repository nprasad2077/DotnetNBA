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
    public class PlayerDataTotalsPlayoffsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerDataTotalsPlayoffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDataTotalsPlayoffs>>> GetPlayerDataTotalsPlayoffs()
        {
            return await _context.PlayerDataTotalsPlayoffs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDataTotalsPlayoffs>> GetPlayerDataTotal(int id)
        {
            var playerDataTotal = await _context.PlayerDataTotalsPlayoffs.FindAsync(id);

            if (playerDataTotal == null)
            {
                return NotFound();
            }
            return playerDataTotal;
        }

        [HttpGet("name/{playerName}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotalsPlayoffs>>> GetPlayerDataByName(string playerName)
        {
                var playerDataTotalsPlayoffs = await _context.PlayerDataTotalsPlayoffs
                    .Where(p => EF.Functions.Like(p.PlayerName, $"%{playerName}%"))
                    .ToListAsync();

                if (playerDataTotalsPlayoffs == null || playerDataTotalsPlayoffs.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataTotalsPlayoffs);
        }

        [HttpGet("season/{season}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotalsPlayoffs>>> GetPlayerDataBySeason(int season)
        {
            var playerDataTotalsPlayoffs = await _context.PlayerDataTotalsPlayoffs
                .Where(p => p.Season == season)
                .ToListAsync();

                if (playerDataTotalsPlayoffs == null || playerDataTotalsPlayoffs.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataTotalsPlayoffs);
        }

        [HttpGet("playerid/{playerId}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotalsPlayoffs>>> GetPlayerDataTotalsPlayoffsByPlayerId (string playerId)
        {
            var playerDataTotalsPlayoffs = await _context.PlayerDataTotalsPlayoffs
                .Where(p => EF.Functions.Like(p.PlayerId, $"%{playerId}%"))
                .ToListAsync();

            if (playerDataTotalsPlayoffs == null || playerDataTotalsPlayoffs.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataTotalsPlayoffs);
        }

        [HttpGet("team/{team}")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotalsPlayoffs>>> GetPlayerDataByTeam (string team)
        {
            var playerDataTotalsPlayoffs = await _context.PlayerDataTotalsPlayoffs
                .Where(p => EF.Functions.Like(p.Team, $"%{team}%"))
                .ToListAsync();
            
            if (playerDataTotalsPlayoffs == null || playerDataTotalsPlayoffs.Count == 0)
            {
                return NotFound();
            }

            return Ok(playerDataTotalsPlayoffs);
        }


        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<PlayerDataTotalsPlayoffs>>> QueryPlayerDataTotalsPlayoffs(
            string? playerName = null,
            int? season = null,
            string? team = null,
            string? playerId = null,
            string sortBy = "PlayerName",
            bool ascending = true,
            int pageNumber = 1,
            int pageSize = 10)
            {
                var query = _context.PlayerDataTotalsPlayoffs.AsQueryable();

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

                var playerDataTotalsPlayoffs = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (playerDataTotalsPlayoffs == null || playerDataTotalsPlayoffs.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataTotalsPlayoffs);
            }


        // New method to test the database connection
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataTotalsPlayoffsCount()
        {
            var count = await _context.PlayerDataTotalsPlayoffs.CountAsync();
            return Ok(count);
        }
    }
}
