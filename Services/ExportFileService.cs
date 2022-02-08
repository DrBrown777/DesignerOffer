﻿using ClosedXML.Excel;
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
        /// <summary>
        /// Обьект closedXML;
        /// </summary>
        private XLWorkbook WorkBook;

        /// <summary>
        /// Сервис калькуляции цен
        /// </summary>
        private readonly ICalculator CalculatorService;

        public bool ExportToExcel(Offers offer)
        {
            bool isGenSumarySheet, isGenInternalUseColumns;

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

            export_window.DataContext = export_model;

            if (export_window.ShowDialog() != true) return false;

            isGenSumarySheet = export_model.SummarySheet;

            isGenInternalUseColumns = export_model.InternalUse;

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

            if (isGenSumarySheet)
            {
                IXLWorksheet ws = WorkBook.AddWorksheet("ИТОГО");

                ws.Column("A").Width = 5; ws.Column("B").Width = 40;
                ws.Column("C").Width = 11; ws.Column("D").Width = 15;
                ws.Column("E").Width = 15; ws.Column("E").Width = 11;
                ws.Column("G").Width = 15; ws.Column("H").Width = 15;
                ws.Column("I").Width = 19;

                ws.Columns(4, 5).Group();

                #region шапка
                string logoPath = @"..\..\Resources\logo.png";
                ws.AddPicture(logoPath).MoveTo(ws.Cell("I1")).Scale(0.21);

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
                IXLTable sumaryTable = ws.Cell(14, 1).InsertTable(GetSumaryTable(offer).AsEnumerable());
                sumaryTable.ShowAutoFilter = false;
                sumaryTable.ShowTotalsRow = true;
                sumaryTable.Rows().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                sumaryTable.Rows().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sumaryTable.Rows().Style.Font.FontSize = 12;
                

                foreach (var row in sumaryTable.Rows())
                {
                    ws.Row(row.RangeAddress.FirstAddress.RowNumber).Height = 30;

                    if (row.Equals(sumaryTable.HeadersRow())) continue;
                    var sum_in = row.Cell(5).AsRange();
                    var sum_out = row.Cell(8).AsRange();

                    row.Cells(4, 5).Style.Fill.BackgroundColor = XLColor.Apricot;

                    sum_in.FormulaA1 = $"={row.Cell(4).Address}*{row.Cell(6).Address}";
                    sum_out.FormulaA1 = $"={row.Cell(6).Address}*{row.Cell(7).Address}";
                }

                ws.Row(sumaryTable.TotalsRow().RowNumber()).Height = 25;

                sumaryTable.Field(4).TotalsRowFunction = XLTotalsRowFunction.Sum;
                sumaryTable.Field(7).TotalsRowFunction = XLTotalsRowFunction.Sum;
                sumaryTable.Field(7).TotalsRowFunction = XLTotalsRowFunction.Custom;
                sumaryTable.Field(1).TotalsRowLabel = "Итого грн с НДС:";
                sumaryTable.Field(3).TotalsRowFormulaA1 = $"=(" +
                    $"{sumaryTable.Field(7).TotalsCell.Address}-" +
                    $"{sumaryTable.Field(4).TotalsCell.Address})/" +
                    $"{sumaryTable.Field(7).TotalsCell.Address}";
                
                var percentCell = sumaryTable.Field(3).TotalsCell;
                percentCell.Style.NumberFormat.NumberFormatId = 10;

                var entryCost = sumaryTable.Field(4).TotalsCell;
                entryCost.Style.NumberFormat.Format = "# ##0.00";
                var outCost = sumaryTable.Field(7).TotalsCell;
                outCost.Style.NumberFormat.Format = "# ##0.00";
                #endregion

                #region примечания
                IXLRangeRow rngLastRow = sumaryTable.LastRow();

                var firstAddress = rngLastRow.RangeAddress.FirstAddress;
                var lastAddress = rngLastRow.RangeAddress.LastAddress;

                int firstCellRow = firstAddress.RowNumber;
                int firstCellColumn = firstAddress.ColumnNumber;
                int lastCellRow = lastAddress.RowNumber;
                int lastCellColumn = lastAddress.ColumnNumber;

                firstCellRow += 2; firstCellColumn++; lastCellRow += 4;

                IXLRange rngNote = ws.Range(firstCellRow, firstCellColumn, lastCellRow, lastCellColumn);
                rngNote.Style.Alignment.WrapText = true;
                rngNote.Style.Font.FontSize = 12;
                rngNote.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                rngNote.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rngNote.Row(1).Style.Font.Bold = true;
                rngNote.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
                rngNote.Row(1).Merge();
                rngNote.Row(1).Value = "Примечания:";
                rngNote.Row(2).Merge();
                rngNote.Row(2).Value = "1. Коммерческое предложение составлено для определения ценовой " +
                    "политики и ориентировочной стоимости работ и материалов.";
                rngNote.Row(3).Merge();
                rngNote.Row(3).Value = "2. Выполнение монтажных работ производится при использовании энергоресурсов заказчика.";
                ws.Row(firstCellRow).Height = 25;
                ws.Row(++firstCellRow).Height = 28;
                ws.Row(++firstCellRow).Height = 28;
                #endregion

                #region исполнитель
                lastCellRow += 2;
                int firstPosEmpRow = lastCellRow;
                int PosEmpCol = lastCellColumn;
                lastCellRow += 2;
                int lastPosEmpRow = lastCellRow;

                IXLRange rndEmploee = ws.Range(firstPosEmpRow, PosEmpCol, lastPosEmpRow, PosEmpCol);
                rndEmploee.Style.Font.FontSize = 11;
                rndEmploee.Style.Font.Italic = true;
                rndEmploee.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                rndEmploee.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rndEmploee.Row(1).Value = $"Исполнитель: " +
                    $"{offer.Projects.Employees.Last_Name} " +
                    $"{offer.Projects.Employees.First_Name}";
                rndEmploee.Row(2).Value = $"тел: {offer.Projects.Employees.Phone}";
                rndEmploee.Row(3).Value = $"e-mail: {offer.Projects.Employees.Mail}";
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

        private DataTable GetSumaryTable(Offers offer)
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
