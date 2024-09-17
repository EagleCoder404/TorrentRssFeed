using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using TorrentRssFeed.Data;
using TorrentRssFeed.DTO.Torrent;
using TorrentRssFeed.DTO.TorrentList;

namespace TorrentRssFeed.Services
{
	public class TorrentListService
	{
		private readonly TorrentRssFeedDbContext context;

		public TorrentListService(TorrentRssFeedDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<ViewTorrentListDto>> GetAllTorrentListAsync(AppUser user)
		{
			var torrentLists = await context.TorrentList
				.Where(x => x.AppUser.Id == user.Id)
				.Select(x => new ViewTorrentListDto()
				{
					Id = x.Id,
					Name = x.Name,
				})
				.ToListAsync();

			return torrentLists;
		}

		public async Task<IEnumerable<ViewTorrentDto>> GetTorrentsFromTorrentList(int torrentListId, AppUser user)
		{
			var torrents = await context.Torrent
				.Include(x => x.TorrentList)
				.Where(x => x.TorrentList.Id == torrentListId && x.TorrentList.AppUser.Id == user.Id)
				.Select(x => new ViewTorrentDto()
				{
					Id = x.Id,
					Name = x.Name,
					Url = x.MagnetUrl,
				}).ToListAsync();

			return torrents;
		}

		public async Task<string> GetRssFeedForTorrentList(int torrentListId, AppUser user)
		{
			var torrents = await context.Torrent
				.Include(x => x.TorrentList)
				.Where(x => x.TorrentList.Id == torrentListId && x.TorrentList.AppUser.Id == user.Id)
				.ToListAsync();

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

		public async Task<ViewTorrentListDto> CreateTorrentListAsync(CreateTorrentListDto createTorrentListDto, AppUser user)
		{
			var torrentList = new TorrentList()
			{
				Name = createTorrentListDto.Name,
				AppUser = user
			};

			context.Add(torrentList);
			await context.SaveChangesAsync();

			return new ViewTorrentListDto()
			{
				Id = torrentList.Id,
				Name = torrentList.Name,
			};
		}

		public async Task<TorrentList> UpdateTorrentListAsync(int torrentListId, UpdateTorrentListDto updateTorrentListDto, AppUser user)
		{
			var torrentList = await context.TorrentList.FirstOrDefaultAsync(x => x.Id == torrentListId && x.AppUser.Id == user.Id);
			if(torrentList != null)
			{
				torrentList.Name = updateTorrentListDto.Name;
				await context.SaveChangesAsync();
			}
			return torrentList;
		}

		public async Task<TorrentList> DeleteTorrentListAsync(int id, AppUser user)
		{
			var torrentList = await context.TorrentList.FirstOrDefaultAsync(x => x.Id == id && x.AppUser.Id == user.Id);
			if (torrentList != null)
			{
				context.Remove(torrentList);
				await context.SaveChangesAsync();
			}
			return torrentList;
		}

		public async Task<ViewTorrentDto> AddTorrentAsync(int torrentListId, CreateTorrentDto createTorrentDto, AppUser user)
		{
			var torrentList = await context.TorrentList.FirstOrDefaultAsync(x => x.Id == torrentListId && x.AppUser.Id == user.Id);

			if (torrentList != null)
			{
				var torrent = new Torrent()
				{
					MagnetUrl = createTorrentDto.Url,
					Name = createTorrentDto.Name,
					TorrentList = torrentList,
					CreatedOn = DateTime.Now
				};
				context.Add(torrent);
				await context.SaveChangesAsync();

				return new ViewTorrentDto()
				{
					Id = torrent.Id,
					Name = torrent.Name,
					Url = torrent.MagnetUrl,
					CreatedOn = torrent.CreatedOn
				};
			}

			return null;
		}

		public async Task<Torrent?> DeleteTorrentAsync(int torrentListid, int torrentId, AppUser user)
		{
			var torrent = await context.Torrent
				.Include(x => x.TorrentList)
				.FirstOrDefaultAsync(x => x.Id == torrentId && x.TorrentList.AppUser.Id == user.Id && x.TorrentList.Id == torrentListid);
			if (torrent != null)
			{
				context.Remove(torrent);
				await context.SaveChangesAsync();

				return torrent;
			}

			return torrent;
		}
	}
}
