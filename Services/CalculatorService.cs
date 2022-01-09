using Designer_Offer.Data;
using Designer_Offer.Models;
using Designer_Offer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        /// <summary>
        /// Расчет цены одного отдельного КП
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public OfferPrice CalculateOfferPrice(int offerId)
        {
            Offer offer = RepositoryOffer.Get(offerId);

            OfferPrice offerPrice = new OfferPrice()
            {
                EntryCost = offer.Part.Sum(it => it.Entry_Cost),
                OutCost = offer.Part.Sum(it => it.Out_Cost),
            };

            return offerPrice;
        }

        /// <summary>
        /// Расчет цены одной отдельной системы
        /// </summary>
        /// <param name="partId"></param>
        public ObservableCollection<PartPrice> CalculatePartPrice(ICollection<Part> parts)
        {
            ObservableCollection<PartPrice> partPrices = new ObservableCollection<PartPrice>();

            foreach (Part item in parts)
            {
                PartPrice partPrice = new PartPrice(item.Offer.Config.Margin_Admin, item.Offer.Config.Margin_Product,
                    item.InstallPart.Sum(it => it.Entry_Summ), item.ProductPart.Sum(it => it.Entry_Summ),
                    item.InstallPart.Sum(it => it.Out_Summ), item.ProductPart.Sum(it => it.Out_Summ))
                {
                    PartName = item.Name
                };

                partPrices.Add(partPrice);

                item.Entry_Cost = partPrice.EntryCost;
                item.Out_Cost = partPrice.OutCost;

                RepositoryPart.Update(item);
            }

            return partPrices;
        }

        public void CalculateOneInstallPrice(int installId)
        {
            throw new NotImplementedException();
        }

        public void CalculateOneProductPrice(int productId)
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
