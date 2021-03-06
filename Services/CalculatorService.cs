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
        public OfferPrice CalculateOfferPrice(Offers offer)
        {
            List<PartPrice> partPrice = CalculatePartPrice(offer.Parts);

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
        public List<PartPrice> CalculatePartPrice(ICollection<Parts> parts)
        {
            List<PartPrice> partPrices = new List<PartPrice>();

            foreach (Parts item in parts)
            {
                TotalProductPrice productPrice = CalculateTotalProductPrice(item.ProductPart);

                TotalInstallPrice installPrice = CalculateTotalInstallPrice(item.InstallPart);

                PartPrice partPrice = new PartPrice(item.Offers.Configs.Margin_Admin, item.Offers.Configs.Margin_Product,
                    installPrice.EntryCost, productPrice.EntryCost, installPrice.OutCost, productPrice.OutCost)
                { PartName = item.Name };

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
            foreach (ProductPart product in products)
            {
                PriceOneItem(product);
            }

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
            decimal mrgAdm = 0, mrgProd = 0;

            if (installs.Count != 0)
            { 
                mrgAdm = installs.First().Parts.Offers.Configs.Margin_Admin;
                mrgProd = installs.First().Parts.Offers.Configs.Margin_Product;
            }

            foreach (InstallPart install in installs)
            {
                PriceOneItem(install);
            }

            TotalInstallPrice installPrice = new TotalInstallPrice(mrgAdm, mrgProd)
            {
                EntryCost = installs.Sum(it => it.Entry_Summ),
                OutCost = installs.Sum(it => it.Out_Summ)
            };

            return installPrice;
        }
        /// <summary>
        /// Расчет цен одной позиции материалов или работ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void PriceOneItem<T>(T item)
        {
            if (item == null)
            {
                return;
            }
            if (item is ProductPart product)
            {
                decimal margin_product = product.Parts.Offers.Configs.Margin_Product;

                product.Out_Price = Price.RoundDecimal(product.Entry_Price * margin_product);
                product.Entry_Summ = Price.RoundDecimal(product.Amount * product.Entry_Price);
                product.Out_Summ = Price.RoundDecimal(product.Amount * product.Out_Price);
            }
            if (item is InstallPart install)
            {
                decimal margin_work = install.Parts.Offers.Configs.Margin_Work;

                install.Out_Price = Price.RoundDecimal(install.Entry_Price * margin_work);
                install.Entry_Summ = Price.RoundDecimal(install.Amount * install.Entry_Price);
                install.Out_Summ = Price.RoundDecimal(install.Amount * install.Out_Price);
            }
        }
    }
}
