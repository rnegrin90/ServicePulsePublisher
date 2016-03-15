using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PublishPlatform.Trello
{
    
    public class TrelloCard
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
        [JsonProperty("due")]
        public DateTime? DueDate { get; set; }
        [JsonProperty("idList")]
        public string ListId { get; set; }
        [JsonProperty("idMembers")]
        public List<string> MembersId { get; set; }
        [JsonProperty("idLabels")]
        public List<string> LabelsId { get; set; }
        [JsonProperty("urlSource")]
        public string UrlSource { get; set; }
        [JsonProperty("pos")]
        public string Pos { get; set; }
    }
}