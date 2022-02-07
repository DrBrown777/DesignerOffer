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
using System.Data;
using Designer_Offer.Models;

namespace Designer_Offer.Services
{
    internal class ExportFileService : IExportService
    {
        private XLWorkbook WorkBook;
        private readonly ICalculator CalculatorService;

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

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Сохранить как",
                Filter = "Книга Excel (*.xlsx)|*.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = offer.Name + ".xlsx"
            };

            if (saveFileDialog.ShowDialog() != true)
            {
                return false;
            }

            WorkBook = App.Host.Services.GetRequiredService<XLWorkbook>();

            if (summarySheet)
            {
                IXLWorksheet ws = WorkBook.AddWorksheet("ИТОГО");

                ws.Column("A").Width = 5; ws.Column("B").Width = 40;
                ws.Column("C").Width = 11; ws.Column("D").Width = 15;
                ws.Column("E").Width = 15; ws.Column("E").Width = 11;
                ws.Column("G").Width = 15; ws.Column("H").Width = 15;
                ws.Column("I").Width = 15;

                ws.Columns(4, 5).Group();

                #region шапка
                string logoPath = @"..\..\Resources\logo.png";
                ws.AddPicture(logoPath).MoveTo(ws.Cell("H1")).Scale(0.21);

                IXLRange rngBorder = ws.Range("A6:I6");
                rngBorder.Style.Border.BottomBorder = XLBorderStyleValues.Double;

                IXLRange rndText = ws.Range("A1:A6");
                rndText.DataType = XLDataType.Text;
                rndText.Style.Font.SetFontSize(12);
                ws.Cell("A2").Style.Font.SetFontSize(14);
                ws.Cell("A2").Style.Font.Bold = true;
                ws.Cell("A2").Value = offer.Projects.Employees.Companies.Name;
                ws.Cell("A3").Value = offer.Projects.Employees.Companies.Adress;
                ws.Cell("A5").Value = offer.Projects.Employees.Companies.Phone;
                ws.Cell("A6").Value = offer.Projects.Employees.Companies.Mail;
                #endregion

                #region дата
                IXLRange rngDate = ws.Range("H8:I8");
                rngDate.Style.Border.BottomBorder = XLBorderStyleValues.Dashed;
                rngDate.Row(1).Merge();
                rngDate.Cell(1, 1).Style.Font.Bold = true;
                rngDate.Cell(1, 1).Style.Font.FontSize = 12;
                rngDate.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rngDate.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngDate.Style.DateFormat.Format = "@";
                rngDate.Value = offer.Date.ToString("MMMM yyyy");
                #endregion

                #region раздел
                IXLRange rngSection = ws.Range("F8:G8");
                rngSection.Style.Border.BottomBorder = XLBorderStyleValues.Dashed;
                rngSection.Row(1).Merge();
                rngSection.Cell(1, 1).Style.Font.FontSize = 12;
                rngSection.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rngSection.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngSection.Style.DateFormat.Format = "@";
                rngSection.Value = "Раздел: " + offer.Sections.Name;
                #endregion

                #region название КП, обьекта и адрес
                ws.Row(10).Height = 20;
                ws.Row(11).Height = 40;
                ws.Row(12).Height = 20;
                IXLRange rngNameOffer = ws.Range(10, 1, 12, 9);
                rngNameOffer.Row(1).Style.Font.Bold = true;
                rngNameOffer.Row(1).Style.Font.FontSize = 14;
                rngNameOffer.Row(2).Style.Font.FontSize = 12;
                rngNameOffer.Row(3).Style.Font.FontSize = 11;
                rngNameOffer.Row(1).Merge();
                rngNameOffer.Row(1).Style.Alignment.WrapText = true;
                rngNameOffer.Row(2).Merge();
                rngNameOffer.Row(2).Style.Alignment.WrapText = true;
                rngNameOffer.Row(3).Merge();
                rngNameOffer.Row(3).Style.Alignment.WrapText = true;
                rngNameOffer.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rngNameOffer.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngNameOffer.Row(1).Value = offer.Name;
                rngNameOffer.Row(2).Value = "для обьекта: " + offer.Projects.Builds.Name;
                rngNameOffer.Row(3).Value = "расположенного по адресу: " + offer.Projects.Builds.Adress;
                #endregion

                #region итоговая таблица систем
                IXLTable summaryTable = ws.Cell(14, 1).InsertTable(GetTable(offer).AsEnumerable());
                summaryTable.ShowAutoFilter = false;
                summaryTable.ShowTotalsRow = true;
                summaryTable.Rows().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                summaryTable.Rows().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                summaryTable.Rows().Style.Font.FontSize = 12;
                

                foreach (var row in summaryTable.Rows())
                {
                    ws.Row(row.RangeAddress.FirstAddress.RowNumber).Height = 30;

                    if (row.Equals(summaryTable.HeadersRow())) continue;
                    var sum_in = row.Cell(5).AsRange();
                    var sum_out = row.Cell(8).AsRange();

                    row.Cells(4, 5).Style.Fill.BackgroundColor = XLColor.Apricot;

                    sum_in.FormulaA1 = $"={row.Cell(4).Address}*{row.Cell(6).Address}";
                    sum_out.FormulaA1 = $"={row.Cell(6).Address}*{row.Cell(7).Address}";
                }

                ws.Row(summaryTable.TotalsRow().RowNumber()).Height = 25;

                summaryTable.Field(4).TotalsRowFunction = XLTotalsRowFunction.Sum;
                summaryTable.Field(7).TotalsRowFunction = XLTotalsRowFunction.Sum;
                summaryTable.Field(7).TotalsRowFunction = XLTotalsRowFunction.Custom;
                summaryTable.Field(1).TotalsRowLabel = "Итого грн с НДС:";
                summaryTable.Field(3).TotalsRowFormulaA1 = $"=(" +
                    $"{summaryTable.Field(7).TotalsCell.Address}-" +
                    $"{summaryTable.Field(4).TotalsCell.Address})/" +
                    $"{summaryTable.Field(7).TotalsCell.Address}";
                
                var percentCell = summaryTable.Field(3).TotalsCell;
                percentCell.Style.NumberFormat.NumberFormatId = 10;

                var entryCost = summaryTable.Field(4).TotalsCell;
                entryCost.Style.NumberFormat.Format = "# ##0.00";
                var outCost = summaryTable.Field(7).TotalsCell;
                outCost.Style.NumberFormat.Format = "# ##0.00";
                #endregion
            }

            foreach (Parts item in offer.Parts)
            {
                WorkBook.AddWorksheet(item.Name);
            }

            if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                WorkBook.SaveAs(saveFileDialog.FileName);

            return true;
        }

        private DataTable GetTable(Offers offer)
        {
            DataTable table = new DataTable();

            table.Columns.Add("№", typeof(int));
            table.Columns.Add("Наименование", typeof(string));
            table.Columns.Add("Ед.изм.", typeof(string));
            table.Columns.Add("Цена", typeof(decimal));
            table.Columns.Add("Cумма", typeof(decimal));
            table.Columns.Add("Кол-во", typeof(int));
            table.Columns.Add("Цена, грн", typeof(decimal));
            table.Columns.Add("Cумма, грн", typeof(decimal));
            table.Columns.Add("Примечание", typeof(string));

            List<PartPrice> parts = CalculatorService.CalculatePartPrice(offer.Parts);

            for (int i = 0; i < parts.Count(); i++)
            {
                table.Rows.Add(i + 1, "Система " + parts[i].PartName, "сист.", parts[i].EntryCost, 0, 1, parts[i].OutCost, 0, "");
            }

            return table;
        }

        public ExportFileService(ICalculator _calcService)
        {
            CalculatorService = _calcService;
        }
    }
}
