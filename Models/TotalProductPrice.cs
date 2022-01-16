namespace Designer_Offer.Models
{
    internal class TotalProductPrice : Price
    {
        public override decimal? EntryCost { get; set; }

        public override decimal? OutCost { get; set; }

        public override decimal? Proceeds => OutCost - EntryCost;

        public override decimal? Proffit => OutCost != null && OutCost != 0
            ? RoundDecimal((OutCost - EntryCost) / OutCost * 100)
            : 0;
    }
}
