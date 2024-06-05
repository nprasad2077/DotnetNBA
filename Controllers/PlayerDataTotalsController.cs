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
            return playerDataTotal;
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
