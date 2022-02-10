using ClosedXML.Excel;
using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Data;
using Designer_Offer.Models;

namespace Designer_Offer.Services
{
    internal class ExportFileService : IExportService
    {
        #region ПОЛЯ
        /// <summary>
        /// Обьект closedXML;
        /// </summary>
        private XLWorkbook WorkBook;

        /// <summary>
        /// Сервис калькуляции цен
        /// </summary>
        private readonly ICalculator CalculatorService;

        /// <summary>
        /// Сервис Диалогов
        /// </summary>
        private readonly IUserDialog UserDialog;
        #endregion

        #region МЕТОДЫ
        /// <summary>
        /// Генерация КП в xlcel
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
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
                IXLWorksheet ws;

                try
                {
                   ws = WorkBook.AddWorksheet("ИТОГО");
                }
                catch (Exception e)
                {
                    UserDialog.ShowError(e.Message, "Ошибка");
                    return false;
                }

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

                    row.Cells(4, 5).Style.Fill.BackgroundColor = XLColor.Apricot;
                    row.Cell(2).Style.Alignment.WrapText = true;
                    row.Cell(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    var sum_in = row.Cell(5).AsRange();
                    var sum_out = row.Cell(8).AsRange();

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

                ws.PageSetup.PrintAreas.Add(ws.FirstCell().Address, ws.LastCell().Address);
                ws.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                ws.PageSetup.FitToPages(1, 0);
                
                if (!isGenInternalUseColumns)
                {
                    ws.Columns(4, 5).Hide();
                }
            }
            
            foreach (Parts item in offer.Parts)
            {
                IXLWorksheet ss;

                try
                {
                    ss = WorkBook.AddWorksheet(item.Name);
                }
                catch (Exception e)
                {
                    UserDialog.ShowError(e.Message, "Ошибка");
                    return false;
                }
                
                ss.Column("A").Width = 5; ss.Column("B").Width = 50;
                ss.Column("C").Width = 18; ss.Column("D").Width = 11;
                ss.Column("E").Width = 15; ss.Column("F").Width = 15;
                ss.Column("G").Width = 9; ss.Column("H").Width = 15;
                ss.Column("I").Width = 15; ss.Column("J").Width = 19;

                ss.Columns(5, 6).Group();

                #region шапка
                IXLRange rngNameSys = ss.Range("A1:J1");
                rngNameSys.Merge();
                rngNameSys.DataType = XLDataType.Text;
                rngNameSys.Style.Font.SetFontSize(12);
                rngNameSys.Style.Font.Bold = true;
                rngNameSys.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngNameSys.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rngNameSys.Row(1).Value = $"Система {item.Name}";
                #endregion

                #region таблица материалов
                IXLTable productTable = ss.Cell(2, 1).InsertTable(GetProductTable(item).AsEnumerable());
                productTable.ShowAutoFilter = false;
                productTable.ShowTotalsRow = true;
                productTable.Rows().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                productTable.Rows().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                productTable.Rows().Style.Font.FontSize = 12;

                foreach (var row in productTable.Rows())
                {
                    ss.Row(row.RangeAddress.FirstAddress.RowNumber).Height = 30;

                    if (row.Equals(productTable.HeadersRow())) continue;

                    row.Cells(5, 6).Style.Fill.BackgroundColor = XLColor.Apricot;
                    row.Cell(2).Style.Alignment.WrapText = true;
                    row.Cell(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    var sum_in = row.Cell(6).AsRange();
                    var sum_out = row.Cell(9).AsRange();

                    sum_in.FormulaA1 = $"={row.Cell(5).Address}*{row.Cell(7).Address}";
                    sum_out.FormulaA1 = $"={row.Cell(7).Address}*{row.Cell(8).Address}";
                }

                ss.Row(productTable.TotalsRow().RowNumber()).Height = 25;

                productTable.Field(5).TotalsRowFunction = XLTotalsRowFunction.Sum;
                productTable.Field(8).TotalsRowFunction = XLTotalsRowFunction.Sum;
                productTable.Field(8).TotalsRowFunction = XLTotalsRowFunction.Custom;
                productTable.Field(1).TotalsRowLabel = "Итого материалы грн с НДС:";
                productTable.Field(4).TotalsRowFormulaA1 = $"=(" +
                    $"{productTable.Field(8).TotalsCell.Address}-" +
                    $"{productTable.Field(5).TotalsCell.Address})/" +
                    $"{productTable.Field(8).TotalsCell.Address}";

                var percentCell = productTable.Field(4).TotalsCell;
                percentCell.Style.NumberFormat.NumberFormatId = 10;

                IXLCell entryCostProduct = productTable.Field(5).TotalsCell;
                entryCostProduct.Style.NumberFormat.Format = "# ##0.00";
                IXLCell outCostProduct = productTable.Field(8).TotalsCell;
                outCostProduct.Style.NumberFormat.Format = "# ##0.00";
                var lastCell = productTable.LastRow().FirstCell();
                #endregion

                #region таблица работ
                var nextCell = lastCell.CellBelow().Address;

                IXLTable installTable = ss.Cell(nextCell).InsertTable(GetInstallTable(item).AsEnumerable());
                installTable.ShowAutoFilter = false;
                installTable.ShowTotalsRow = true;
                installTable.ShowHeaderRow = false;
                installTable.Rows().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                installTable.Rows().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                installTable.Rows().Style.Font.FontSize = 12;
                
                foreach (var row in installTable.Rows())
                {
                    ss.Row(row.RangeAddress.FirstAddress.RowNumber).Height = 30;

                    row.Cells(5, 6).Style.Fill.BackgroundColor = XLColor.Apricot;
                    row.Cell(2).Style.Alignment.WrapText = true;
                    row.Cell(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    var sum_in = row.Cell(6).AsRange();
                    var sum_out = row.Cell(9).AsRange();

                    sum_in.FormulaA1 = $"={row.Cell(5).Address}*{row.Cell(7).Address}";
                    sum_out.FormulaA1 = $"={row.Cell(7).Address}*{row.Cell(8).Address}";
                }
                
                ss.Row(installTable.TotalsRow().RowNumber()).Height = 25;

                installTable.Field(5).TotalsRowFunction = XLTotalsRowFunction.Sum;
                installTable.Field(8).TotalsRowFunction = XLTotalsRowFunction.Sum;
                installTable.Field(8).TotalsRowFunction = XLTotalsRowFunction.Custom;
                installTable.Field(1).TotalsRowLabel = "Итого работы грн с НДС:";
                installTable.Field(4).TotalsRowFormulaA1 = $"=(" +
                    $"{installTable.Field(8).TotalsCell.Address}-" +
                    $"{installTable.Field(5).TotalsCell.Address})/" +
                    $"{installTable.Field(8).TotalsCell.Address}";

                percentCell = installTable.Field(4).TotalsCell;
                percentCell.Style.NumberFormat.NumberFormatId = 10;

                IXLCell entryCostInstall = installTable.Field(5).TotalsCell;
                entryCostInstall.Style.NumberFormat.Format = "# ##0.00";
                IXLCell outCostInstall = installTable.Field(8).TotalsCell;
                outCostInstall.Style.NumberFormat.Format = "# ##0.00";
                lastCell = installTable.LastRow().FirstCell().CellRight();
                #endregion

                #region вывод админ расходов и общей стоимости системы
                nextCell = lastCell.CellBelow().Address;

                IXLCell adminDesc = ss.Cell(nextCell);
                adminDesc.Value = "Административные расходы:";

                ss.Row(adminDesc.Address.RowNumber).Height = 25;
                ss.Row(adminDesc.Address.RowNumber).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ss.Row(adminDesc.Address.RowNumber).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ss.Row(adminDesc.Address.RowNumber).Style.Font.FontSize = 12;

                adminDesc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                adminDesc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                lastCell = nextCell.Worksheet.Cell(nextCell);

                IXLCell sumSystemDesc = ss.Cell(lastCell.CellBelow().Address);
                sumSystemDesc.Value = $"Всего по системе {item.Name}:";

                ss.Row(sumSystemDesc.Address.RowNumber).Height = 25;
                ss.Row(sumSystemDesc.Address.RowNumber).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ss.Row(sumSystemDesc.Address.RowNumber).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ss.Row(sumSystemDesc.Address.RowNumber).Style.Font.FontSize = 12;

                sumSystemDesc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                sumSystemDesc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                sumSystemDesc.Style.Font.Bold = true;

                TotalInstallPrice totalInstallPrice = CalculatorService.CalculateTotalInstallPrice(item.InstallPart);

                IXLCell adminIn = entryCostInstall.CellBelow();
                adminIn.Style.Fill.BackgroundColor = XLColor.Apricot;
                adminIn.CellLeft().Style.Fill.BackgroundColor = XLColor.Apricot;
                adminIn.Style.NumberFormat.Format = "# ##0.00";

                adminIn.Value = totalInstallPrice.AdminEntryCost;

                IXLCell adminOut = outCostInstall.CellBelow();
                adminOut.Style.NumberFormat.Format = "# ##0.00";

                adminOut.Value = totalInstallPrice.AdminOutCost;

                IXLCell sumSystemIn = adminIn.CellBelow();
                sumSystemIn.Style.Fill.BackgroundColor = XLColor.Apricot;
                sumSystemIn.Style.NumberFormat.Format = "# ##0.00";

                IXLCell sumSystemPerc = sumSystemIn.CellLeft();
                sumSystemPerc.Style.Fill.BackgroundColor = XLColor.Apricot;
                sumSystemPerc.Style.NumberFormat.NumberFormatId = 10;
                sumSystemPerc.Style.Font.Bold = true;

                IXLCell sumSystemOut = adminOut.CellBelow();
                sumSystemOut.Style.NumberFormat.Format = "# ##0.00";
                sumSystemOut.Style.Font.Bold = true;

                sumSystemIn.FormulaA1 = $"={entryCostProduct} + {entryCostInstall} + {adminIn}";
                sumSystemOut.FormulaA1 = $"={outCostProduct} + {outCostInstall} + {adminOut}";
                sumSystemPerc.FormulaA1 = $"=({sumSystemOut} - {sumSystemIn}) / {sumSystemOut}";
                #endregion

                ss.PageSetup.PrintAreas.Add(ss.FirstCell().Address, ss.LastCell().Address);
                ss.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                ss.PageSetup.FitToPages(1, 0);
                
                if (!isGenInternalUseColumns)
                {
                    ss.Columns(5, 6).Hide();
                }
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                    WorkBook.SaveAs(saveFileDialog.FileName);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
                return false;
            }
            
            return true;
        }
        /// <summary>
        /// Генерация итоговой таблицы систем
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Генерация таблицы материалов для систем
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private DataTable GetProductTable(Parts part)
        {
            DataTable table = new DataTable();

            table.Columns.Add("№", typeof(int));
            table.Columns.Add("Наименование", typeof(string));
            table.Columns.Add("Тип", typeof(string));
            table.Columns.Add("Ед.изм.", typeof(string));
            table.Columns.Add("Цена", typeof(decimal));
            table.Columns.Add("Cумма", typeof(decimal));
            table.Columns.Add("Кол-во", typeof(decimal));
            table.Columns.Add("Цена, грн", typeof(decimal));
            table.Columns.Add("Cумма, грн", typeof(decimal));
            table.Columns.Add("Примечание", typeof(string));

            List<ProductPart> products = part.ProductPart.ToList();

            for (int i = 0; i < products.Count(); i++)
            {
                table.Rows.Add(i+1, products[i].Products.Name, 
                                    products[i].Products.Model,
                                    products[i].Products.Units.Name,
                                    products[i].Entry_Price, 0,
                                    products[i].Amount,
                                    products[i].Out_Price, 0,
                                    products[i].Note);
            }

            return table;
        }
        /// <summary>
        /// Генерация таблицы работ для систем
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private DataTable GetInstallTable(Parts part)
        {
            DataTable table = new DataTable();

            table.Columns.Add("№", typeof(int));
            table.Columns.Add("Наименование", typeof(string));
            table.Columns.Add("Тип", typeof(string));
            table.Columns.Add("Ед.изм.", typeof(string));
            table.Columns.Add("Цена", typeof(decimal));
            table.Columns.Add("Cумма", typeof(decimal));
            table.Columns.Add("Кол-во", typeof(decimal));
            table.Columns.Add("Цена, грн", typeof(decimal));
            table.Columns.Add("Cумма, грн", typeof(decimal));
            table.Columns.Add("Примечание", typeof(string));

            List<InstallPart> installs = part.InstallPart.ToList();

            for (int i = 0, j = part.ProductPart.Count(); i < installs.Count(); i++, j++)
            {
                table.Rows.Add(j + 1, installs[i].Installs.Name, "",
                                    installs[i].Installs.Units.Name,
                                    installs[i].Entry_Price, 0,
                                    installs[i].Amount,
                                    installs[i].Out_Price, 0,
                                    installs[i].Note);
            }

            return table;
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ExportFileService(ICalculator _calcService, IUserDialog _userDialog)
        {
            CalculatorService = _calcService;
            UserDialog = _userDialog;
        }
        #endregion
    }
}
