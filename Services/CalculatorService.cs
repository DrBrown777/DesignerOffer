using Designer_Offer.Data;
using Designer_Offer.Models;
using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Designer_Offer.Services
{
    internal class CalculatorService : ICalculator
    {
        /// <summary>
        /// Расчет общей стоимости одного отдельного КП
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        public OfferPrice CalculateOfferPrice(Offer offer)
        {
            List<PartPrice> partPrice = CalculatePartPrice(offer.Part);

            OfferPrice offerPrice = new OfferPrice()
            {
                EntryCost = partPrice.Sum(it => it.EntryCost),
                OutCost = partPrice.Sum(it => it.OutCost)
            };

            return offerPrice;
        }

        /// <summary>
        /// Расчет общей стоимости одной отдельной системы
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public List<PartPrice> CalculatePartPrice(ICollection<Part> parts)
        {
            List<PartPrice> partPrices = new List<PartPrice>();

            foreach (Part item in parts)
            {
               PartPrice partPrice = new PartPrice(item.Offer.Config.Margin_Admin, item.Offer.Config.Margin_Product,
                    item.InstallPart.Sum(it => it.Entry_Summ), item.ProductPart.Sum(it => it.Entry_Summ),
                    item.InstallPart.Sum(it => it.Out_Summ), item.ProductPart.Sum(it => it.Out_Summ)) { PartName = item.Name };

                partPrices.Add(partPrice);
            }

            return partPrices;
        }
        /// <summary>
        /// Расчет общей стоимости материалов в системе
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public TotalProductPrice CalculateTotalProductPrice(ICollection<ProductPart> products)
        {
            TotalProductPrice productPrice = new TotalProductPrice()
            {
                EntryCost = products.Sum(it => it.Entry_Summ),
                OutCost = products.Sum(it => it.Out_Summ)
            };

            return productPrice;
        }
        /// <summary>
        /// Расчет общей стоимости работ в системе
        /// </summary>
        /// <param name="installs"></param>
        /// <returns></returns>
        public TotalInstallPrice CalculateTotalInstallPrice(ICollection<InstallPart> installs)
        {
            decimal mrgAdm = installs.First().Part.Offer.Config.Margin_Admin;
            decimal mrgProd = installs.First().Part.Offer.Config.Margin_Product;

            TotalInstallPrice installPrice = new TotalInstallPrice(mrgAdm, mrgProd)
            {
                EntryCost = installs.Sum(it => it.Entry_Summ),
                OutCost = installs.Sum(it => it.Out_Summ)
            };

            return installPrice;
        }
    }
}
