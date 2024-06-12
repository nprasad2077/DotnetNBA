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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataAdvancedPlayoffs()
        {
            return await _context.PlayerDataAdvancedPlayoffs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task <ActionResult<PlayerDataAdvancedPlayoffs>> GetPlayerDataAdvancedPlayoffs(int id)
        {
            var playerDataAdvancedPlayoffs = await _context.PlayerDataAdvancedPlayoffs.FindAsync(id);

            if (playerDataAdvancedPlayoffs == null)
            {
                return NotFound();
            }

            return Ok(playerDataAdvancedPlayoffs);
        }

        [HttpGet("name/{playerName}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataByName(string playerName)
        {
                var playerDataAdvancedPlayoffs = await _context.PlayerDataAdvancedPlayoffs
                    .Where(p => EF.Functions.Like(p.PlayerName, $"%{playerName}%"))
                    .ToListAsync();

                if (playerDataAdvancedPlayoffs == null || playerDataAdvancedPlayoffs.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataAdvancedPlayoffs);
        }

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
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataAdvancedPlayoffsByPlayerId (string playerId)
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

        [HttpGet("team/{team}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvancedPlayoffs>>> GetPlayerDataByTeam (string team)
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

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataAdvancedPlayoffsCount()
        {
            var count = await _context.PlayerDataAdvancedPlayoffs.CountAsync();
            return Ok(count);
        }
    }

}