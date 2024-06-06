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
        public IEnumerable<Card> GetCardsFromClient(long clientId)
        {
            return FindByCondition(card => card.ClientId==clientId)
                .ToList();
        }

        public IEnumerable<Card> FindCardByColorFromClient(string color, long id)
        {
            return FindByCondition(card=>card.Color==color&&card.ClientId==id)
                .ToList();
        }


        public IEnumerable<Card> FindCardByTypeFromClient(string type,long id)
        {
            return FindByCondition(card=>card.Type==type && card.ClientId == id)
                .ToList();
        }
        public Card GetCardByNumber(string number)
        {
            return FindByCondition(card => card.Number == number)
                .FirstOrDefault();
        }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }
    }
}
