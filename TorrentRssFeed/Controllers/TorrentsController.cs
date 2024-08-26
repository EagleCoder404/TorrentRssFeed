using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorrentRssFeed.Data;

namespace TorrentRssFeed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorrentsController : ControllerBase
    {
        private readonly TorrentRssFeedDbContext _context;

        public TorrentsController(TorrentRssFeedDbContext context)
        {
            _context = context;
        }

        // GET: api/Torrents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Torrent>>> GetTorrent()
        {
            return await _context.Torrent.ToListAsync();
        }

        // GET: api/Torrents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Torrent>> GetTorrent(int id)
        {
            var torrent = await _context.Torrent.FindAsync(id);

            if (torrent == null)
            {
                return NotFound();
            }

            return torrent;
        }

        // PUT: api/Torrents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTorrent(int id, Torrent torrent)
        {
            if (id != torrent.Id)
            {
                return BadRequest();
            }

            _context.Entry(torrent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TorrentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Torrents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Torrent>> PostTorrent(Torrent torrent)
        {
            _context.Torrent.Add(torrent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTorrent", new { id = torrent.Id }, torrent);
        }

        // DELETE: api/Torrents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTorrent(int id)
        {
            var torrent = await _context.Torrent.FindAsync(id);
            if (torrent == null)
            {
                return NotFound();
            }

            _context.Torrent.Remove(torrent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TorrentExists(int id)
        {
            return _context.Torrent.Any(e => e.Id == id);
        }
    }
}
