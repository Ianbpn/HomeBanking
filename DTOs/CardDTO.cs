﻿using HomeBanking.Models;

namespace HomeBanking.DTOs
{
    public class CardDTO
    {
        public CardDTO(Card card)
        {
            Id = card.Id;
            CardHolder = card.CardHolder;
            Type = card.Type.ToString();
            Color = card.Color.ToString();
            Cvv = card.Cvv;
            Number = card.Number;
            FromDate = card.FromDate;
            ThruDate = card.ThruDate;
        }

        public long Id { get; set; }
        public int Cvv {  get; set; }
        public string CardHolder { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ThruDate { get; set; }


    }
}
