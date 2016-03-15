namespace PublishPlatform.Trello
{
    public interface ITrelloClient
    {
        void PostCard(TrelloCard card, string boardName, string listName);
        string SearchBoard(string name);
        string SearchList(string name, string boardId);
    }
}