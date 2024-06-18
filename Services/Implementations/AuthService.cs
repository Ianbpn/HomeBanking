using HomeBanking.DTOs;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using System.Net;

namespace HomeBanking.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IClientsService _clientsService;

        public AuthService(IClientsService clientsService)
        {
            this._clientsService = clientsService;
        }

        public Client FindUserByEmail(string email)
        {
            return _clientsService.GetFullClientByEmail(email);
        }

        public bool VerifyData(LoginDTO loginDTO)
        {
            try
            {
                var user = _clientsService.GetFullClientByEmail(loginDTO.Email);
                if (user == null)
                {
                    throw new CustomException("User not Found", 400);
                }
                else if (!user.Password.Equals(loginDTO.Password))
                {
                    throw new CustomException("Password is Incorrect", 400);
                }
                return true;

            }
            catch (CustomException ex)
            {

                throw new CustomException (ex.message,ex.statusCode);
            }
        }
    }
}
