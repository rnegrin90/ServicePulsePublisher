using FluentAssertions;
using NUnit.Framework;

namespace PublishPlatform.Trello.Tests
{
    [TestFixture]
    public class TrelloClientTests
    {
        [Test]
        [Category("Integration")]
        public void SearchBoard_IgnoringCase_ReturnsCorrectBoard()
        {
            var client = new TrelloClient();
            //You need to create a board and search for it
            var resultId = client.SearchBoard("TestBoard");

            resultId.Should().Be("ID_OF_YOUR_BOARD");
        }
        
        [Test]
        [Category("Integration")]
        public void SearchList_IgnoringCase_ReturnsCorrectBoard()
        {
            var client = new TrelloClient();
            //Create a new list and put its name here
            var resultId = client.SearchList("TestList", "ID_OF_YOUR_BOARD");

            resultId.Should().Be("ID_OF_YOUR_LIST");
        }

        [Test]
        [Ignore("This will post cards on trello")]
        [Category("Integration")]
        public void TrelloClient_AddsACard()
        {
            var client = new TrelloClient();

            var card = new TrelloCard
            {
                Description = "this is a test card",
                Name = "Test",
                DueDate = null,
                UrlSource = "http://url.thatyouwanttoshow.com/",
                Pos = "bottom"
            };
            client.PostCard(card, "TestBoard", "TestList");
        }
    }
}
