using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorrentRssFeed.Data;
using TorrentRssFeed.DTO.Torrent;
using TorrentRssFeed.DTO.TorrentList;
using TorrentRssFeed.Services;

namespace TorrentRssFeed.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TorrentListsController : ControllerBase
    {
        //TODO: Overposting Attacks
        //TODO: Input Validation

		private readonly UserManager<AppUser> _userManager;
		private readonly TorrentListService torrentListService;

		public TorrentListsController(UserManager<AppUser> userManager, TorrentListService torrentListService)
        {
			this._userManager = userManager;
			this.torrentListService = torrentListService;
		}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewTorrentListDto>>> GetTorrentList()
        {
            var currentUser = await GetCurrentUserAsync(User);
            var response = await torrentListService.GetAllTorrentListAsync(currentUser);
			return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ViewTorrentListDto>> PostTorrentList(CreateTorrentListDto createTorrentListDto)
        {
			var currentUser = await GetCurrentUserAsync(User);
			var viewTorrentListDto = await torrentListService.CreateTorrentListAsync(createTorrentListDto, currentUser);
            return Ok(viewTorrentListDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTorrentList(int id, UpdateTorrentListDto updateTorrentListDto)
        {
			var currentUser = await GetCurrentUserAsync(User);
			var torrentList = await torrentListService.UpdateTorrentListAsync(id, updateTorrentListDto, currentUser);
            if (torrentList == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTorrentList(int id)
        {
            var currentUser = await GetCurrentUserAsync(User);
            var torrentList = await torrentListService.DeleteTorrentListAsync(id, currentUser);

            if (torrentList == null)
            {
                return NotFound();
            }

            return NoContent();
        }

		[HttpGet("{torrentListId}/torrents")]
		public async Task<ActionResult<IEnumerable<ViewTorrentDto>>> GetTorrentList(int torrentListId)
		{
			var currentUser = await GetCurrentUserAsync(User);
			var torrents = await torrentListService.GetTorrentsFromTorrentList(torrentListId, currentUser);

			if (torrents == null)
			{
				return NotFound();
			}

			return Ok(torrents);
		}
		[HttpGet("{torrentListId}/torrents/rss")]
		public async Task<ActionResult<string>> GetTorrentListAsRss(int torrentListId)
		{
			var currentUser = await GetCurrentUserAsync(User);
			var rss = await torrentListService.GetRssFeedForTorrentList(torrentListId, currentUser);

			if (rss == null)
			{
				return NotFound();
			}

			return Ok(rss);
		}

		[HttpPost("{torrentListId}/torrents")]
		public async Task<ActionResult<ViewTorrentDto>> AddTorrent(int torrentListId, CreateTorrentDto createTorrentDto)
		{
			var currentUser = await GetCurrentUserAsync(User);
			var torrent = await torrentListService.AddTorrentAsync(torrentListId, createTorrentDto, currentUser);

			if (torrent == null)
			{
				return NotFound();
			}

			return Ok(torrent);
		}

		[HttpDelete("{torrentListId}/torrents/{torrentId}")]
		public async Task<ActionResult> DeleteTorrent(int torrentListId, int torrentId)
		{
			var currentUser = await GetCurrentUserAsync(User);
			var torrent = await torrentListService.DeleteTorrentAsync(torrentListId, torrentId, currentUser);

			if (torrent == null)
			{
				return NotFound();
			}

			return NoContent();
		}

		private async Task<AppUser> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return await this._userManager.GetUserAsync(user);
		}
    }
}
