﻿using HomeBanking.Models;

namespace HomeBanking.DTOs
{
    public class AccountClientDTO
    {
        public AccountClientDTO(Account account)
        {
            Id = account.Id;
            Number = account.Number;
            CreationDate = account.CreationDate;
            Balance = account.Balance;
        }

        public long Id { get; set; }
        public string Number { get; set; }

        public DateTime CreationDate { get; set; }

        public double Balance { get; set; }
    }
}
