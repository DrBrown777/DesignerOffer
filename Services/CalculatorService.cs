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

        public Tuple<decimal?, decimal?> CalculateOfferPrice(int offerId)
        {
            Offer offer = RepositoryOffer.Get(offerId);

            Tuple<decimal?, decimal?> prices = new Tuple<decimal?, decimal?>(
                    offer.Part.Sum(it => it.Entry_Cost),
                    offer.Part.Sum(it => it.Out_Cost)
                );

            return prices;/*возвращать нужно % нацеки еще + доходность*/
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
}
