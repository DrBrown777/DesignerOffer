using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer_Offer.Services
{
    internal class ExportFileService : IExportService
    {
        public bool ExportToExcel(Offers offer, bool summarySheet = true, bool internalUse = true)
        {
            if (offer == null)
            {
                throw new ArgumentNullException(nameof(offer.Name));
            }

            ExportWindow export_window = App.Host.Services
                                        .GetRequiredService<ExportWindow>();
            ExportViewModel export_model = App.Host.Services
                                        .GetRequiredService<ExportViewModel>();

            export_model.ProjectName = offer.Projects.Name;

            export_model.OfferName = offer.Name;

            export_model.SummarySheet = summarySheet;

            export_model.InternalUse = internalUse;

            export_window.DataContext = export_model;

            if (export_window.ShowDialog() != true) return false;

            ///Логика экспорта

            return true;
        }
    }
}
