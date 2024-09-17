﻿namespace TorrentRssFeed.Data
{
    public class Torrent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MagnetUrl { get; set; }
        public DateTime CreatedOn { get; set; }

        public TorrentList TorrentList { get; set; }
    }
}
