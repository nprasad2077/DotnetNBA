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

        // New method to test the database connection
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataTotalsPlayoffsCount()
        {
            var count = await _context.PlayerDataTotalsPlayoffs.CountAsync();
            return Ok(count);
        }
    }
}
