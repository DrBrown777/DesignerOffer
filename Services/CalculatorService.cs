using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer_Offer.Services
{
    internal class CalculatorService : ICalculator
    {
        #region ПОЛЯ
        /// <summary>
        /// Репозитоторий КП
        /// </summary>
        private readonly IRepository<Offer> RepositoryOffer;
        /// <summary>
        /// Репозитоторий Систем
        /// </summary>
        private readonly IRepository<Part> RepositoryPart;
        #endregion

        public OfferPrice CalculateOfferPrice(int offerId)
        {
            Offer offer = RepositoryOffer.Get(offerId);

            OfferPrice offerPrice = new OfferPrice()
            {
                OfferEntryCost = offer.Part.Sum(it => it.Entry_Cost),
                OfferOutCost = offer.Part.Sum(it => it.Out_Cost),
            };

            return offerPrice;
        }

        public void CalculatePartPrice(int partId)
        {
            List<Part> parts = RepositoryPart.Items.Where(it => it.Id == partId).ToList();

            foreach (Part item in parts)
            {
                item.Entry_Cost = item.InstallPart.Sum(it => it.Entry_Summ) + item.ProductPart.Sum(it => it.Entry_Summ);
                item.Out_Cost = item.InstallPart.Sum(it => it.Out_Summ) + item.ProductPart.Sum(it => it.Out_Summ);

                /*Требуется добавить админ расходы и вернуть их*/
            }
        }

        public void CalculateOneInstallPrice(int productId, int installId)
        {
            throw new NotImplementedException();
        }

        public void CalculateOneProductPrice(int productId, int installId)
        {
            throw new NotImplementedException();
        }

        public CalculatorService(IRepository<Offer> repaOffer, IRepository<Part> repaPart)
        {
            RepositoryOffer = repaOffer;
            RepositoryPart = repaPart;
        }
    }
    internal struct OfferPrice
    {
        /// <summary>
        /// Входящаяя цена КП
        /// </summary>
        public decimal? OfferEntryCost { get; set; }
        /// <summary>
        /// Исходящая цена КП
        /// </summary>
        public decimal? OfferOutCost { get; set; }
        /// <summary>
        /// Доходность КП в деньгах
        /// </summary>
        public decimal? OfferProceeds => OfferOutCost - OfferEntryCost;
        /// <summary>
        /// Доходность КП в %
        /// </summary>
        public decimal? OfferProffit => OfferOutCost != null && OfferOutCost != 0
            ? RoundDecimal((OfferOutCost - OfferEntryCost) / OfferOutCost * 100)
            : 0;
        /// <summary>
        /// Округление цены
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private decimal RoundDecimal(decimal? number)
        {
            return decimal.Round((decimal)number, 2, MidpointRounding.AwayFromZero);
        }
    }
}
