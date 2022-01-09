using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer_Offer.Models
{
    internal class PartPrice : Price
    {
        private readonly decimal? marginAdmin, marginProduct, installSummEntry;

        private readonly decimal? installSummOut, productSummEntry, productSummOut;

        /// <summary>
        /// Название системы
        /// </summary>
        public string PartName { get; set; }

        public override decimal? EntryCost
        {
            get => RoundDecimal(installSummEntry * marginAdmin) + installSummEntry + productSummEntry;
            set { }
        }

        public override decimal? OutCost
        {
            get => RoundDecimal(installSummEntry * marginAdmin * marginProduct) + installSummOut + productSummOut;
            set { }
        }

        public override decimal? Proceeds => OutCost - EntryCost;

        public override decimal? Proffit => OutCost != null && OutCost != 0
            ? RoundDecimal((OutCost - EntryCost) / OutCost * 100)
            : 0;

        public PartPrice(decimal? mrgAdm, decimal? mrgProd, decimal? instSumEntry,
                            decimal? prodSumEntry, decimal? instSumOut, decimal? prodSumOut)
        {
            marginAdmin = mrgAdm;
            marginProduct = mrgProd;
            installSummEntry = instSumEntry;
            productSummEntry = prodSumEntry;
            installSummOut = instSumOut;
            productSummOut = prodSumOut;
        }
    }
}
