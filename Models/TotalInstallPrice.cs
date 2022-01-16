namespace Designer_Offer.Models
{
    internal class TotalInstallPrice : Price
    {
        private readonly decimal marginAdmin, marginProduct;

        public decimal? AdminEntryCost
        {
            get => RoundDecimal(EntryCost * marginAdmin);
            set { }
        }

        public decimal? AdminOutCost
        {
            get => RoundDecimal(AdminEntryCost * marginProduct);
            set { }
        }

        public override decimal? EntryCost { get; set; }

        public override decimal? OutCost { get; set; }

        public override decimal? Proceeds => OutCost - EntryCost;

        public override decimal? Proffit => OutCost != null && OutCost != 0
            ? RoundDecimal((OutCost - EntryCost) / OutCost * 100)
            : 0;

        public TotalInstallPrice(decimal mrgAdm, decimal mrgProd)
        {
            marginAdmin = mrgAdm;
            marginProduct = mrgProd;
        }
    }
}
