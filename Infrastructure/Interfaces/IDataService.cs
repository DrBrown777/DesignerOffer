using Designer_Offer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Designer_Offer.Infrastructure.Interfaces
{
    public interface IDataService
    {
       Task<List<Company>> GetAllAsync();
    }
}
