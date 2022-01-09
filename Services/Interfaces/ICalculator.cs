using Designer_Offer.Data;
using Designer_Offer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Designer_Offer.Services.Interfaces
{
    internal interface ICalculator
    {
        OfferPrice CalculateOfferPrice(int offerId);

        ObservableCollection<PartPrice> CalculatePartPrice(ICollection<Part> parts);

        void CalculateOneProductPrice(int productId);

        void CalculateOneInstallPrice(int installId);
    }
}
