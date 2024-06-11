﻿using HomeBanking.DTOs;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace HomeBanking.Services.Implementations
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly iClientsService _clientsService;
        private readonly IAccountsService _accountsService;
        private readonly IClientLoanService _clientLoanService;
        private readonly ITransactionsService _transactionsService;
        public LoanService(ILoanRepository loanRepository, iClientsService clientsService, IAccountsService accountsService, IClientLoanService clientLoanService, ITransactionsService transactionsService)
        {
            _loanRepository = loanRepository;
            _clientsService = clientsService;
            _accountsService = accountsService;
            _clientLoanService = clientLoanService;
            _transactionsService = transactionsService;
        }

        public Loan FindLoanById(long id)
        {
            return _loanRepository.GetLoanById(id);
        }

        public ClientLoan LoanRequest(LoanApplicationDTO loanApplicationDTO, Client client)
        {
            if (VerifyDataFromPost(loanApplicationDTO) && VerifyIfLoanMeetsRequest(loanApplicationDTO))
            {
                // Traigo la cuenta referida y reviso si es una cuenta del cliente
                var accounts = _accountsService.GetAccountsByClient(client.Id); //Busco las cuenta del usuario autenticado
                if (!accounts.Any(accs=>accs.Number == loanApplicationDTO.ToAccountNumber)) {
                    throw new CustomException("La cuenta no pertenece al Usuario", HttpStatusCode.BadRequest);
                }
                else
                {
                    //Busco la cuenta a actualizar y el prestamo al cual refiere
                    var accountToUpdate = _accountsService.FindAccountByNumber(loanApplicationDTO.ToAccountNumber);
                    var loanToUse = FindLoanById(loanApplicationDTO.LoanId);

                    //Genero el ClientLoan
                    var clientLoan = new ClientLoan
                    {
                        Payments=loanApplicationDTO.Payments,
                        Amount=loanApplicationDTO.Amount*1.2,
                        LoanId=loanApplicationDTO.LoanId,
                        ClientId=client.Id
                    };
                    //Genero la nueva Transaccion
                    var generatedTransaction = new Transaction
                    {
                        AccountId = accountToUpdate.Id,
                        Amount = loanApplicationDTO.Amount,
                        Date = DateTime.Now,
                        Description = loanToUse.Name + " loan approved.",
                        Type = Enums.TransactionType.CREDIT
                    };
                    //Actualizo los datos de la cuenta
                    accountToUpdate.Balance = accountToUpdate.Balance + loanApplicationDTO.Amount;
                    accountToUpdate.Transactions.Add(generatedTransaction);
                    
                    //Guardo todo en la BD
                    _transactionsService.AddTransaction(generatedTransaction);
                    _accountsService.Save(accountToUpdate);
                    _clientLoanService.AddClientLoan(clientLoan);

                    return clientLoan;
                }

            }
            throw new CustomException("Error de verificaciones",HttpStatusCode.Forbidden);
        }

        public bool VerifyDataFromPost(LoanApplicationDTO loanApplicationDTO)
        {
            //verifico que lleguen bien los datos
            if (loanApplicationDTO.ToAccountNumber.IsNullOrEmpty() || loanApplicationDTO.LoanId <= 0 ||
                loanApplicationDTO.Payments.IsNullOrEmpty() || loanApplicationDTO.Amount <= 0)
            {
                throw new CustomException("Verificar los campos vacios", HttpStatusCode.BadRequest);
            }
            if (_loanRepository.GetLoanById(loanApplicationDTO.LoanId) == null)
            {
                throw new CustomException("El Tipo de prestamo no se ha encontrado", HttpStatusCode.BadRequest);
            }
            if(_accountsService.FindAccountByNumber(loanApplicationDTO.ToAccountNumber)==null)
            {
                throw new CustomException("La cuenta referida no existe", HttpStatusCode.BadRequest);
            }
            return true;
        }
        public bool VerifyIfLoanMeetsRequest(LoanApplicationDTO loanApplicationDTO)
        {
            var compareTo = _loanRepository.GetLoanById(loanApplicationDTO.LoanId);
            //Verifico que el prestamo permita la cantidad de cuotas
            if (!compareTo.Paymnets.Contains(loanApplicationDTO.Payments))
            {
                throw new CustomException("Este prestamo no soporta esta cantidad de cuotas", HttpStatusCode.BadRequest);
            }
            else
            {
                if (compareTo.MaxAmount < loanApplicationDTO.Amount)
                {
                    throw new CustomException("El monto requerido excede el limite permitido para este prestamo", HttpStatusCode.BadRequest);
                }
                return true;
            }
        }
    }
}
