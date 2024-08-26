using Microsoft.EntityFrameworkCore;
using System;

namespace TorrentRssFeed.Data
{
	public class TorrentRssFeedDbContext : DbContext
	{
		public TorrentRssFeedDbContext(DbContextOptions<TorrentRssFeedDbContext> options)
			: base(options)
		{
		}

		public DbSet<Torrent> Torrent { get; set; }
	}
}
