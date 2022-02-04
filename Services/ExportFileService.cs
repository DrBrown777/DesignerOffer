using ClosedXML.Excel;
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
using System.Drawing;
using Microsoft.Win32;

namespace Designer_Offer.Services
{
    internal class ExportFileService : IExportService
    {
        private readonly XLWorkbook WorkBook;

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
            if(summarySheet)
            {
                IXLWorksheet ws = WorkBook.AddWorksheet("ИТОГО");

                string logoPath = @"..\..\Resources\logo.png";

                ws.AddPicture(logoPath).MoveTo(ws.Cell("I1")).Scale(0.25);

                IXLRange rngTitle = ws.Range("A8:K8");

                rngTitle.Style.Border.BottomBorder = XLBorderStyleValues.Double;
            }

            foreach (Parts item in offer.Parts)
            {
                WorkBook.AddWorksheet(item.Name);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Excel files|*.xlsx",
                Title = "Save an Excel File",
                FileName = offer.Name + ".xlsx"
            };

            saveFileDialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                WorkBook.SaveAs(saveFileDialog.FileName);

            //WorkBook.SaveAs(offer.Name + ".xlsx");

            return true;
        }

        public ExportFileService(XLWorkbook _wb)
        {
            WorkBook = _wb;
        }
    }
}
