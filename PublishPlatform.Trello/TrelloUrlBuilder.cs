namespace PublishPlatform.Trello
{
    public class TrelloUrlBuilder
    {
        private const string UrlBase = "https://api.trello.com/1";
        private string _token;
        private string _auth;
        private string _action;

        private TrelloUrlBuilder() { }

        public static TrelloUrlBuilder With()
        {
            return new TrelloUrlBuilder();
        }

        public TrelloUrlBuilder Token(string key)
        {
            _token = key;
            return this;
        }

        public TrelloUrlBuilder Auth(string key)
        {
            _auth = key;
            return this;
        }

        public TrelloUrlBuilder Action(string action)
        {
            _action = action;
            return this;
        }

        public string Build()
        {
            var url = UrlBase;
            url += _action ?? "/no_action_specified";
            url += _token != null ? (url.Contains("?") ? "&key=" : "?key=") : "";
            url += _token ?? "";
            url += _token != null ? (_auth != null ? "&token=" + _auth : "") : "";
            return url;
        }
    }
}