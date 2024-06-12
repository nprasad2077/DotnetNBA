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

        [HttpGet]
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
        }

        [HttpGet("name/{playerName}")]
        public async Task<ActionResult<IEnumerable<PlayerDataAdvanced>>> GetPlayerDataByName(string playerName)
        {
                var playerDataAdvanced = await _context.PlayerDataAdvanced
                    .Where(p => EF.Functions.Like(p.PlayerName, $"%{playerName}%"))
                    .ToListAsync();

                if (playerDataAdvanced == null || playerDataAdvanced.Count == 0)
                {
                    return NotFound();
                }

                return Ok(playerDataAdvanced);
        }

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

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataAdvancedCount()
        {
            var count = await _context.PlayerDataAdvanced.CountAsync();
            return Ok(count);
        }
    }

}