using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Designer_Offer.Services
{
    internal class DataService : IDataService
    {
        private Func<PrimeContext> ContextCreator;

        public async Task<List<Company>> GetAllAsync()
        {
            using(var context = ContextCreator())
            {
                return await context.Company.AsNoTracking().ToListAsync();
            }
        }

        public DataService(Func<PrimeContext> _contextCreator)
        {
            ContextCreator = _contextCreator;
        }
    }
}
