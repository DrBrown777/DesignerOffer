using Designer_Offer.Data;
using Designer_Offer.Models;
using System.Collections.Generic;

namespace Designer_Offer.Services.Interfaces
{
    internal interface ICalculator
    {
        OfferPrice CalculateOfferPrice(Offer offer);

        List<PartPrice> CalculatePartPrice(ICollection<Part> parts);

        TotalProductPrice CalculateTotalProductPrice(ICollection<ProductPart> products);

        TotalInstallPrice CalculateTotalInstallPrice(ICollection<InstallPart> installs);
    }
}
