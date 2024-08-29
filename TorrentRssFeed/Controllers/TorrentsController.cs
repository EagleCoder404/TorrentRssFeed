using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
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
        [HttpGet("rss")]
        public async Task<string> GetRssFeed()
        {
			SyndicationFeed feed = new SyndicationFeed("Torrent Feed", "This is a torrent feed", new Uri("https://torrentrssfeed.azurewebsites.net"));
			feed.Authors.Add(new SyndicationPerson("harsha.jediknight@gmail.com"));
			feed.Categories.Add(new SyndicationCategory("Torrent"));
			feed.Description = new TextSyndicationContent("Dummy");

			var items = new List<SyndicationItem>();

			var torrents = await _context.Torrent.ToListAsync();

			foreach (var torrent in torrents)
            {
                var item = new SyndicationItem(
                    torrent.Name,
                    torrent.MagnetUrl,
					new Uri("http://localhost/Content/One"),
                    torrent.Id.ToString(),
                    DateTime.Now
				);
                item.ElementExtensions.Add(
                    new XElement(
                        "enclosure",
                            new XAttribute("type", "application/x-bittorrent"),
                            new XAttribute("url", torrent.MagnetUrl)
                    )
                );
                items.Add(item);
            }

            feed.Items = items;
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
