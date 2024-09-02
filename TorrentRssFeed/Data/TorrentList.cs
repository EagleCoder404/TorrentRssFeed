namespace TorrentRssFeed.Data
{
	public class TorrentList
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<Torrent> Torrents { get; set; }
	}
}
