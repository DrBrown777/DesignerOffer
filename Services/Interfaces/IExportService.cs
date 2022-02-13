using Designer_Offer.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Designer_Offer.Services.Interfaces
{
    internal interface IExportService
    {
        Task<bool> ExportToExcelAsync(Offers offer, CancellationToken Cancel = default);
    }
}
