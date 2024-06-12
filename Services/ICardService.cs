using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface ICardService
    {
        bool IsUniqueTypeAndColor(long clientId,string type,string color);
        int GenerateCVV();
        string GenerateUniqueNumber();
        List<CardDTO> GetAllCards();
        CardDTO GetCardById(long id);
        List<CardDTO> GetCardsFromClient(long id);
        Card NewCardForClient(NewCardDTO newCardDTO, long clientId);
    }
}
