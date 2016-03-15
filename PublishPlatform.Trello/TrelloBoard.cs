using System;

namespace PublishPlatform.Trello
{
    public class TrelloBoard
    {
        public string Name { get; set; }
        public string IdOrganization { get; set; }
        public string ShortLink { get; set; }
        public DateTime? DateLastActivity { get; set; } 
        public string Id { get; set; }
    }
}
