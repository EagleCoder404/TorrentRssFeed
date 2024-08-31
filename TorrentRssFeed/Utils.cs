using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using TorrentRssFeed.Data;

namespace TorrentRssFeed
{
    public class Utils
    {
        public static SyndicationFeed GetSyndicationFeedFromTorrentList(List<Torrent> torrents)
        {
			SyndicationFeed feed = new SyndicationFeed("Torrent Feed", "This is a torrent feed", new Uri("https://torrentrssfeed.azurewebsites.net"));

			feed.Authors.Add(new SyndicationPerson("harsha.jediknight@gmail.com"));
			feed.Categories.Add(new SyndicationCategory("Torrent"));
			feed.Description = new TextSyndicationContent("Dummy");

			var items = new List<SyndicationItem>();


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

			return feed;

		}
    }
}
