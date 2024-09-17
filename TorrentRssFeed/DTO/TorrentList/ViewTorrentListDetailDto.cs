using TorrentRssFeed.DTO.Torrent;

namespace TorrentRssFeed.DTO.TorrentList
{
	public class ViewTorrentListDetailDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<ViewTorrentDto> Torrents { get; set; }
	}
}
