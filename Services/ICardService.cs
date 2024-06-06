namespace HomeBanking.Services
{
    public interface ICardService
    {
        bool IsUniqueTypeAndColor(long clientId,string type,string color);
        int GenerateCVV();
        string GenerateUniqueNumber();
    }
}
