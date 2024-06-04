using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        //[HttpGet]
        //public IActionResult Hello()
        //{
        //    return Ok("Buenos dias querido Mundo!!!");
        //}

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = clients.Select(c => new ClientDTO(c)).ToList();
                return StatusCode(StatusCodes.Status200OK, clientsDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetClientById(long id)
        {
            try
            {
                var client = _clientRepository.FindById(id);
                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpGet("current")]
        [Authorize(Policy ="ClientOnly")]
        public IActionResult GetCurrentClient()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : null;
                if (email.IsNullOrEmpty())
                {
                    return StatusCode(403, "User not Found");
                }
                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                return Ok(new ClientDTO(client));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost]
        public IActionResult NewClient([FromBody] NewClientDTO newClientDTO)
        {
            try
            {
                if(newClientDTO.FirstName.IsNullOrEmpty() || newClientDTO.LastName.IsNullOrEmpty() || newClientDTO.Password.IsNullOrEmpty() || newClientDTO.Email.IsNullOrEmpty())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Missing fields");
                }
                Client client = _clientRepository.FindByEmail(newClientDTO.Email);
                if (client != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Email is already in use");
                }
                Client newClient = new Client
                {
                    FirstName = newClientDTO.FirstName,
                    LastName = newClientDTO.LastName,
                    Password = newClientDTO.Password,
                    Email = newClientDTO.Email
                };
                _clientRepository.Save(newClient);
                return Ok(newClient);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
