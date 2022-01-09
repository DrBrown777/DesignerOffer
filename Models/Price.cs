using System;

namespace Designer_Offer.Models
{
    internal abstract class Price
    {
        /// <summary>
        /// Входящаяя цена
        /// </summary>
        public abstract decimal? EntryCost { get; set; }

        /// <summary>
        /// Исходящая цена
        /// </summary>
        public abstract decimal? OutCost { get; set; }

        /// <summary>
        /// Доходность в деньгах
        /// </summary>
        public abstract decimal? Proceeds { get; }

        /// <summary>
        /// Доходность в %
        /// </summary>
        public abstract decimal? Proffit { get; }

        /// <summary>
        /// Округление цен
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public decimal RoundDecimal(decimal? number)
        {
            return decimal.Round((decimal)number, 2, MidpointRounding.AwayFromZero);
        }
    }
}
