using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using log4net;

namespace PublishPlatform.Trello
{
    public class TrelloClient : ITrelloClient
    {
        private List<TrelloBoard> Boards { get; set; }
        private List<TrelloList> Lists { get; set; }
        private readonly TrelloUrlBuilder _trelloUrlBuilder;
        private readonly ILog _log = LogManager.GetLogger(nameof(TrelloClient));

        public TrelloClient()
        {
            var token = ConfigurationManager.AppSettings["TrelloToken"];
            var auth = ConfigurationManager.AppSettings["TrelloAuth"];
            _trelloUrlBuilder = TrelloUrlBuilder.With()
                .Token(token)
                .Auth(auth);
        }

        public void PostCard(TrelloCard card, string boardName, string listName)
        {

            var boardId = SearchBoard(boardName);
            _log.Info($"BoardId of {boardName}: {boardId}");
            if (string.IsNullOrEmpty(boardId))
                return;

            var listId = SearchList(listName, boardId);
            _log.Info($"ListId of {listName}: {listId}");
            if (string.IsNullOrEmpty(listId))
                return;

            card.ListId = listId;
            try
            {
                _log.Info("Posting card...");
                PostCardAsync(card).Wait();
            }
            catch (Exception e)
            {
                _log.Error($"Exception while posting a card. Trace: {e.Message}");
            }
        }

        private async Task PostCardAsync(TrelloCard card)
        {
            var url = _trelloUrlBuilder
                .Action($"/cards")
                .Build();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _log.Info($"Posting card {card.Name} to {url}");
                HttpResponseMessage response = await client.PostAsJsonAsync("", card);
                _log.Info($"Response: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        public string SearchList(string name, string boardId)
        {
            try
            {
                GetListAsync(boardId).Wait();
                return Lists.FirstOrDefault(b => string.Equals(b.Name, name, StringComparison.CurrentCultureIgnoreCase))?.Id;
            }
            catch (Exception e)
            {
                _log.Error($"Exception while searching for list {name}. Trace: {e.Message}");
                return null;
            }
        }

        private async Task GetListAsync(string boardId)
        {
            var url = _trelloUrlBuilder
                .Action($"/boards/{boardId}/lists")
                .Build();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _log.Info($"Getting list from {url}");
                HttpResponseMessage response = await client.GetAsync("");
                _log.Info($"Response: {response.StatusCode} - {response.ReasonPhrase}");
                if (response.IsSuccessStatusCode)
                {
                    Lists = await response.Content.ReadAsAsync<List<TrelloList>>();
                }
            }
        }

        public string SearchBoard(string name)
        {
            try
            {
                GetBoardsAsync().Wait();
                return Boards.FirstOrDefault(b => string.Equals(b.Name, name, StringComparison.CurrentCultureIgnoreCase))?.Id;
            }
            catch (Exception e)
            {
                _log.Error($"Exception while searching for board {name}. Trace: {e.Message}");
                return null;
            }
        }

        private async Task GetBoardsAsync()
        {
            var url = _trelloUrlBuilder
                .Action("/member/me/boards")
                .Build();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _log.Info($"Getting board from {url}");
                HttpResponseMessage response = await client.GetAsync("");
                _log.Info($"Response: {response.StatusCode} - {response.ReasonPhrase}");
                if (response.IsSuccessStatusCode)
                {
                    Boards = await response.Content.ReadAsAsync<List<TrelloBoard>>();
                }
            }
        }
    }
}
