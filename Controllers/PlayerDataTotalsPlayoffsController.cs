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

        // New method to test the database connection
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPlayerDataTotalsPlayoffsCount()
        {
            var count = await _context.PlayerDataTotalsPlayoffs.CountAsync();
            return Ok(count);
        }
    }
}
