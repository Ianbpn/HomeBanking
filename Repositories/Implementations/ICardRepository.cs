using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        Card GetCardById(long id);
        void Save (Card loan);
    }
}
