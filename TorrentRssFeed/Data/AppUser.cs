using Microsoft.AspNetCore.Identity;

namespace TorrentRssFeed.Data
{
	public class AppUser : IdentityUser
	{
		public ICollection<TorrentList> TorrentLists { get; set; }
	}
}
