using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        Card GetCardById(long id);
        IEnumerable<Card> GetCardsFromClient(long clientId);
        IEnumerable<Card> FindCardByTypeFromClient(string type,long id);
        IEnumerable<Card> FindCardByColorFromClient(string color,long id);
        Card GetCardByNumber(string number);
        void Save (Card loan);
    }
}
