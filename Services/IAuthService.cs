using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IAuthService
    {
        bool VerifyData(LoginDTO loginDTO);
        Client FindUserByEmail(string email);


    }
}
