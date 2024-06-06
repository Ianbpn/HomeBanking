using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using System.Security.Cryptography;

namespace HomeBanking.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public int GenerateCVV()
        {
            var lowerBound = 100;
            var upperBound = 1000;
            int rng = RandomNumberGenerator.GetInt32(lowerBound,upperBound);
            return rng;
        }

        public string GenerateUniqueNumber()
        {
            string uniqueNumber;
            var lowerBound = 1000;
            var upperBound = 10000;
            do
            {
                uniqueNumber = RandomNumberGenerator.GetInt32(lowerBound, upperBound) + "-"
                    + RandomNumberGenerator.GetInt32(lowerBound, upperBound) + "-"
                    + RandomNumberGenerator.GetInt32(lowerBound, upperBound) + "-"
                    + RandomNumberGenerator.GetInt32(lowerBound, upperBound);
            } while (_cardRepository.GetCardByNumber(uniqueNumber) != null);
            return uniqueNumber;
        }

        public bool IsUniqueTypeAndColor(long clientId, string type, string color)
        {
            //var card = _cardRepository.GetCardsFromClient(clientId);
            //if (card.Count() >= 6)
            //{
            //    return false;
            //}

            //no me salio
            return false;
        }

    }
}
