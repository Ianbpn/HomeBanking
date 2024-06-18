using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Exceptions;
using HomeBanking.Repositories.Implementations;
using System.Security.Cryptography;
using System.Net;

namespace HomeBanking.Services.Implementations
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IClientsService _clientsService;
        public CardService(ICardRepository cardRepository, IClientsService clientService)
        {
            _cardRepository = cardRepository;
            _clientsService = clientService;
        }
        public List<CardDTO> GetAllCards()
        {
            var cards = _cardRepository.GetAllCards();
            var cardsDTO = cards.Select(cards => new CardDTO(cards)).ToList();
            return cardsDTO;
        }

        public CardDTO GetCardById(long id)
        {
            var card = _cardRepository.GetCardById(id);
            var cardDTO = new CardDTO(card);
            return cardDTO;
        }

        public int GenerateCVV()
        {
            var lowerBound = 100;
            var upperBound = 1000;
            int rng = RandomNumberGenerator.GetInt32(lowerBound, upperBound);
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

        public List<CardDTO> GetCardsFromClient(long id)
        {
            var cards = _cardRepository.GetCardsFromClient(id);
            var cardDTO = cards.Select(card => new CardDTO(card)).ToList();
            return cardDTO;
        }

        public Card NewCardForClient(NewCardDTO newCardDTO, long clientId)
        {
            try
            {
                var cards = _cardRepository.FindCardByTypeFromClient(newCardDTO.Type,clientId);
                if (cards.Count() >= 3)
                {
                    throw new CustomException("El limite de tarjetas se alcanzo", 400);
                }
                else
                {
                    if (cards.Any(c => c.Color == newCardDTO.Color))
                    {
                        throw new CustomException("Ya hay de este color", 400);
                    }
                    var currentClient = _clientsService.GetClientById(clientId);

                    int newCvv = GenerateCVV();
                    string newNumber = GenerateUniqueNumber();
                    string newType = newCardDTO.Type.ToString().ToUpper();
                    string newColor = newCardDTO.Color.ToString().ToUpper();

                    Card newCard = new Card()
                    {
                        ClientId = clientId,
                        CardHolder = currentClient.FirstName + " " + currentClient.LastName,
                        Type = newType,
                        Color = newColor,
                        Number = newNumber,
                        Cvv = newCvv,
                        FromDate = DateTime.Now,
                        ThruDate = DateTime.Now.AddYears(5),
                    };

                    _cardRepository.Save(newCard);
                    return newCard;
                }
            }
            catch (CustomException ex)
            {

                throw new CustomException(ex.message, ex.statusCode);
            }
        }

    }
}
