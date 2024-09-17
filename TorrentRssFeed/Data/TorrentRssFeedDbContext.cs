using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System;
using TorrentRssFeed.Data;

namespace TorrentRssFeed.Data
{
	public class TorrentRssFeedDbContext : IdentityDbContext<AppUser>
	{
		public TorrentRssFeedDbContext(DbContextOptions<TorrentRssFeedDbContext> options)
			: base(options)
		{
		}

		public DbSet<Torrent> Torrent { get; set; }
	    public DbSet<TorrentList> TorrentList { get; set; } = default!;
	}
}
