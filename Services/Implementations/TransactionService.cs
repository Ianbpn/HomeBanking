﻿using HomeBanking.DTOs;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using Microsoft.IdentityModel.Tokens;
using System.Net;


namespace HomeBanking.Services.Implementations
{
    public class TransactionService : ITransactionsService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountsService _accountsService;

        public TransactionService(ITransactionRepository transactionRepository, IAccountsService accountsService)
        {
            _transactionRepository = transactionRepository;
            _accountsService = accountsService;
        }
        public void NewTransaction(long clientId, NewTransactionDTO newTransactionDTO)
        {
            try
            {
                if (VerifyDataFromPost(newTransactionDTO)) // Hago la Verificación de Data necesaria
                {
                    var accounts = _accountsService.GetAccountsByClient(clientId); //Busco las cuenta del usuario autenticado
                    if (accounts.Any(a => a.Number == newTransactionDTO.FromAccountNumber)) //Verifico que la cuenta de origen pertenezca a las del usuario
                    {
                        //Busco ambas cuentas que formaran parte del proceso
                        var fromAccount = _accountsService.FindAccountByNumber(newTransactionDTO.FromAccountNumber);
                        var toAccount = _accountsService.FindAccountByNumber(newTransactionDTO.ToAccountNumber);
                        if (fromAccount.Balance < newTransactionDTO.Amount) //Verifico que la cuenta de Origen tenga los fondos suficientes
                        {
                            throw new CustomException("Esta cuenta no tiene los fondos suficientes", HttpStatusCode.BadRequest);
                        }
                        //Genero ambas transacciones
                        var DebitTransaction = new Transaction
                        {
                            AccountId = fromAccount.Id,
                            Amount = -newTransactionDTO.Amount,
                            Date = DateTime.Now,
                            Description = newTransactionDTO.Description + ". Enviado a: " + toAccount.Number,
                            Type = Enums.TransactionType.DEBIT
                        };
                        var CreditTransaction = new Transaction
                        {
                            AccountId = toAccount.Id,
                            Amount = newTransactionDTO.Amount,
                            Date = DateTime.Now,
                            Description = newTransactionDTO.Description + ". Recibido de: " + fromAccount.Number,
                            Type = Enums.TransactionType.CREDIT
                        };
                        //Agrego ambas entidades para su guardado
                        fromAccount.Transactions.Add(DebitTransaction);
                        toAccount.Transactions.Add(CreditTransaction);

                        //Modifico el balance de ambas cuentas
                        fromAccount.Balance = fromAccount.Balance - newTransactionDTO.Amount;
                        toAccount.Balance = toAccount.Balance + newTransactionDTO.Amount;

                        //Guardamos las nuevas transacciones en la BD
                        _transactionRepository.Save(DebitTransaction);
                        _transactionRepository.Save(CreditTransaction);
                        //Actualizo ambas cuentas en la BD
                        _accountsService.Save(toAccount);
                        _accountsService.Save(fromAccount);
                    }
                    else
                    {
                        throw new CustomException("Esta cuenta no pertenece al usuario", HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (CustomException)
            {
                {
                    throw;
                }
            }
        }

        public bool VerifyDataFromPost(NewTransactionDTO transactionDTO)
        {
            //Verifico que todos los campos provenientes del Front esten correctamente cargados
            if (transactionDTO.FromAccountNumber.IsNullOrEmpty() || transactionDTO.ToAccountNumber.IsNullOrEmpty()
                || transactionDTO.Description.IsNullOrEmpty() || transactionDTO.Amount <= 0)
            {
                throw new CustomException("Verificar los campos vacios", HttpStatusCode.BadRequest);
            }
            //Reviso que los numeros de cuentas no sean identicos
            else if (transactionDTO.ToAccountNumber.Equals(transactionDTO.FromAccountNumber))
            {
                throw new CustomException("Los numeros de cuenta no pueden ser identicos", HttpStatusCode.BadRequest);
            }
            //Busco en la base de datos si las cuentas existen
            if (!DoesAccountExist(transactionDTO.FromAccountNumber) || !DoesAccountExist(transactionDTO.ToAccountNumber))
            {
                throw new CustomException($"Una de las cuentas no a sido encontrada", HttpStatusCode.BadRequest);
            }
            return true;
        }
        public bool DoesAccountExist(string accountNumber) //Este metodo revisa si las cuentas estan en la BD
        {
            Account account = _accountsService.FindAccountByNumber(accountNumber);
            if (account == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
