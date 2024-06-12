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
    public class ShotChartDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShotChartDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShotChartData>>> GetShotChartData()
        {
            return await _context.ShotChartData.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShotChartData>> GetShotChartData(int id)
        {
            var shotChartData = await _context.ShotChartData.FindAsync(id);

            if (shotChartData == null)
            {
                return NotFound();
            }
            return shotChartData;
        }

        [HttpGet("playerid/{playerId}")]
        public async Task<ActionResult<IEnumerable<ShotChartData>>> GetShotChartDataByPlayerId(string playerId)
        {
            var shotChartData = await _context.ShotChartData
                .Where(p => EF.Functions.Like(p.PlayerId, $"%{playerId}%"))
                .ToListAsync();

            if (shotChartData == null || shotChartData.Count == 0)
            {
                return NotFound();
            }

            return Ok(shotChartData);
        }

        // New endpoint to get distinct player names
        [HttpGet("distinct-player-names")]
        public async Task<ActionResult<IEnumerable<string>>> GetDistinctPlayerNames()
        {
            var distinctPlayerNames = await _context.ShotChartData
                .Select(p => p.PlayerName)
                .Distinct()
                .ToListAsync();

            if (distinctPlayerNames == null || distinctPlayerNames.Count == 0)
            {
                return NotFound();
            }

            return Ok(distinctPlayerNames);
        }

        // New endpoint to get distinct seasons for a given player name
        [HttpGet("seasons/{playerName}")]
        public async Task<ActionResult<IEnumerable<int>>> GetDistinctSeasonsByPlayerName(string playerName)
        {
            var distinctSeasons = await _context.ShotChartData
                .Where(p => p.PlayerName == playerName)
                .Select(p => p.Season)
                .Distinct()
                .ToListAsync();

            if (distinctSeasons == null || distinctSeasons.Count == 0)
            {
                return NotFound();
            }

            return Ok(distinctSeasons);
        }

        [HttpGet("player/{playerName}/season/{season}")]
        public async Task<ActionResult<IEnumerable<ShotChartData>>> GetShotChartDataByPlayerNameAndSeason(string playerName, int season)
        {
            var shotChartData = await _context.ShotChartData
                .Where(p => p.PlayerName == playerName && p.Season == season)
                .ToListAsync();

            if (shotChartData == null || shotChartData.Count == 0)
            {
                return NotFound();
            }

            return Ok(shotChartData);
        }


        [HttpGet("count")]
        public async Task<ActionResult<int>> GetShotChartDataCount()
        {
            var count = await _context.ShotChartData.CountAsync();
            return Ok(count);
        }
    }
}
