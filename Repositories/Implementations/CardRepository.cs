using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Card> GetAllCards()
        {
            return FindAll();
        }

        public Card GetCardById(long id)
        {
            return FindByCondition(card => card.Id == id)
                .FirstOrDefault();
        }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }
    }
}
