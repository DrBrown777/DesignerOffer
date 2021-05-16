using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Designer_Offer.Services.Interfaces
{
    internal interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T> Items { get; }

        T Get(int id);

        Task<T> GetAsync(int id, CancellationToken Cancel = default);

        T Add(T item);

        Task<T> AddAsync(T item, CancellationToken Cancel = default);

        void Update(T item);

        Task UpdateAsync(T item, CancellationToken Cancel = default);

        void Remove(int id);
    }
}
