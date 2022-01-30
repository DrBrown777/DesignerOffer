using Designer_Offer.Data;
using Designer_Offer.Models;
using System.Collections.Generic;

namespace Designer_Offer.Services.Interfaces
{
    internal interface ICalculator
    {
        OfferPrice CalculateOfferPrice(Offers offer);

        List<PartPrice> CalculatePartPrice(ICollection<Parts> parts);

        TotalProductPrice CalculateTotalProductPrice(ICollection<ProductPart> products);

        TotalInstallPrice CalculateTotalInstallPrice(ICollection<InstallPart> installs);

        void PriceOneItem<T>(T item);
    }
}
