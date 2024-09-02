using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using TorrentRssFeed.Data;
using TorrentRssFeed.DTO;

namespace TorrentRssFeed.Controllers
{
    [Authorize]
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

        // POST: api/Torrents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Torrent>> PostTorrent(CreateTorrentDto createdTorrentDto)
        {
            var torrent = new Torrent()
            {
                Name = createdTorrentDto.Name,
                MagnetUrl = createdTorrentDto.Url
            };
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

		[HttpPost("clear")]
		public async Task<IActionResult> ClearTorrents()
		{
			await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.Torrent");
			return NoContent();
		}

		[HttpGet("rss")]
        public async Task<string> GetRssFeed()
        {
			var torrents = await _context.Torrent.ToListAsync();
            var feed = Utils.GetSyndicationFeedFromTorrentList(torrents);

			var formatter = new Rss20FeedFormatter(feed);
			var output = new StringBuilder();
			using (var writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
			{
				formatter.WriteTo(writer);
				writer.Flush();
			}

			return output.ToString();
		}

        private bool TorrentExists(int id)
        {
            return _context.Torrent.Any(e => e.Id == id);
        }
    }
}
