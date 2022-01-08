using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer_Offer.Services.Interfaces
{
    internal interface ICalculator
    {
        OfferPrice CalculateOfferPrice(int offerId);
        void CalculateOneProductPrice(int productId, int installId);
        void CalculateOneInstallPrice(int productId, int installId);
        void CalculatePartPrice(int partId);
    }
}
