using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using ClosedXML.Excel;

using Core.Models;

using DocumentFormat.OpenXml.Spreadsheet;
using DAL;

namespace MeltProperties
{
    public class ExcelCreator
    {
        private static ExcelModel Model;

        //public static void Create()
        //{
        //    ////create new xls file
        //    //string file = "C:\\newdoc.xls";
        //    ////Workbook workbook = new Workbook();
        //    ////Worksheet worksheet = new Worksheet("First Sheet");
        //    ////worksheet.Cells[0, 1] = new Cell((short)1);
        //    ////worksheet.Cells[2, 0] = new Cell(9999999);
        //    ////worksheet.Cells[3, 3] = new Cell((decimal)3.45);
        //    ////worksheet.Cells[2, 2] = new Cell("Text string");
        //    ////worksheet.Cells[2, 4] = new Cell("Second string");
        //    ////worksheet.Cells[4, 0] = new Cell(32764.5, "#,##0.00");
        //    ////worksheet.Cells[5, 1] = new Cell(DateTime.Now, @"YYYY\-MM\-DD");
        //    ////worksheet.Cells.ColumnWidth[0, 1] = 3000;
        //    ////workbook.Worksheets.Add(worksheet);
        //    ////workbook.Save(file);

        //    ////// open xls file
        //    ////Workbook book = Workbook.Load(file);
        //    ////Worksheet sheet = book.Worksheets[0];
            
            
        //    //var wb = new XLWorkbook();
        //    //var ws = wb.Worksheets.Add("Contacts");
        //    //// Title
        //    //ws.Cell("B2").Value = "Contacts";

        //    //// First Names
        //    //ws.Cell("B3").Value = "FName";
        //    //ws.Cell("B4").Value = "John";
        //    //ws.Cell("B5").Value = "Hank";
        //    //ws.Cell("B6").Value = "Dagny";

        //    //// Last Names
        //    //ws.Cell("C3").Value = "LName";
        //    //ws.Cell("C4").Value = "Galt";
        //    //ws.Cell("C5").Value = "Rearden";
        //    //ws.Cell("C6").Value = "Taggart";

        //    //// Boolean
        //    //ws.Cell("D3").Value = "Outcast";
        //    //ws.Cell("D4").Value = true;
        //    //ws.Cell("D5").Value = false;
        //    //ws.Cell("D6").Value = false;

        //    //// DateTime
        //    //ws.Cell("E3").Value = "DOB";
        //    //ws.Cell("E4").Value = new DateTime(1919, 1, 21);
        //    //ws.Cell("E5").Value = new DateTime(1907, 3, 4);
        //    //ws.Cell("E6").Value = new DateTime(1921, 12, 15);

        //    //// Numeric
        //    //ws.Cell("F3").Value = "Income";
        //    //ws.Cell("F4").Value = 2000;
        //    //ws.Cell("F5").Value = 40000;
        //    //ws.Cell("F6").Value = 10000;

        //    //// From worksheet
        //    //var rngTable = ws.Range("B2:F6");

        //    //// From another range
        //    //var rngDates = rngTable.Range("D3:D5"); // The address is relative to rngTable (NOT the worksheet)
        //    //var rngNumbers = rngTable.Range("E3:E5"); // The address is relative to rngTable (NOT the worksheet)

        //    //// Using OpenXML's predefined formats
        //    //rngDates.Style.NumberFormat.NumberFormatId = 15;

        //    //// Using a custom format
        //    //rngNumbers.Style.NumberFormat.Format = "$ #,##0";

        //    //var rngHeaders = rngTable.Range("A2:E2"); // The address is relative to rngTable (NOT the worksheet)
        //    //rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //    //rngHeaders.Style.Font.Bold = true;
        //    //rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

        //    //rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

        //    //rngTable.Cell(1, 1).Style.Font.Bold = true;
        //    //rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
        //    //rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //    //rngTable.Row(1).Merge(); // We could've also used: rngTable.Range("A1:E1").Merge()

        //    ////Add a thick outside border
        //    //rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

        //    //// You can also specify the border for each side with:
        //    //// rngTable.FirstColumn().Style.Border.LeftBorder = XLBorderStyleValues.Thick;
        //    //// rngTable.LastColumn().Style.Border.RightBorder = XLBorderStyleValues.Thick;
        //    //// rngTable.FirstRow().Style.Border.TopBorder = XLBorderStyleValues.Thick;
        //    //// rngTable.LastRow().Style.Border.BottomBorder = XLBorderStyleValues.Thick;

        //    //ws.Columns(2, 6).AdjustToContents();

        //    //wb.SaveAs("BasicTable.xlsx");

        //    //CreateResult();
        //}

        public static void CreateResult(ExcelModel model, bool needToSave = true)
        {
            model.SetMeltsOxides();
            Model = model;
            var disk = AppDomain.CurrentDomain.BaseDirectory.ToCharArray().First();
            string fileName = string.Format(@"{1}:\result{0}.xlsx", DateTime.Now.ToString("dd-MM-yy_hhmm"), disk);
            string template = "template.xlsx";
            var wb = new XLWorkbook(template);
            var ws = wb.Worksheet("result");
            CreateHeader(ws);
            SetRows(ws);
            SetData(ws);
            wb.SaveAs(fileName);
            if (needToSave) 
            {
                Sqlite.SaveProject(Model);
            }
            Model = null;
        }

        private const string moduleHeaderTemplateString = "Коефіцієнт {0}/R2O";

        private static void CreateHeader(IXLWorksheet ws)
        {
            //var systems = new List<string>() { "CaO-Al2O3-SiO2", "К2O-Al2O3-SiO2" };
            //var firstSystemPhases = new List<string>() { "A", "NAS2", "NAS6", "S", "A3S2" };
            ////ws.Column("J").InsertColumnsAfter(3);
            //var cell = ws.Cell("B5");//.Value = "CaO-Al2O3-SiO2";
            //SetChemicalFormula(cell.RichText, "Na2O-Al2O3-SiO2");
            //ws.Cell("A10").Value = Model.NameOfMaterial;
            SetChemicalFormula(ws.Cell("B5").RichText, Model.FirstSystemName);
            SetChemicalFormula(ws.Cell("T5").RichText, Model.SecondSystemName);
            SetChemicalFormula(ws.Cell("D7").RichText, string.Format(moduleHeaderTemplateString, Model.FirstSystemOxide));
            SetChemicalFormula(ws.Cell("T7").RichText, string.Format(moduleHeaderTemplateString, Model.SecondSystemOxide));

            var firstSystemOxides = Model.ResultTemperaturesModels.First().FirstSystem.SolidSumOxides.Select(x => x.Oxide.Formula).ToList();
            var secondSystemOxides = Model.ResultTemperaturesModels.First().SecondSystem.SolidSumOxides.Select(x => x.Oxide.Formula).ToList();

            SetChemicalFormula(ws.Cell("Q9").RichText, firstSystemOxides[0]);
            SetChemicalFormula(ws.Cell("R9").RichText, firstSystemOxides[1]);
            SetChemicalFormula(ws.Cell("S9").RichText, firstSystemOxides[2]);

            SetChemicalFormula(ws.Cell("AG9").RichText, secondSystemOxides[0]);
            SetChemicalFormula(ws.Cell("AH9").RichText, secondSystemOxides[1]);
            SetChemicalFormula(ws.Cell("AI9").RichText, secondSystemOxides[2]);
            
            var oxidesForCalculation = Model.OxidesResultModel.First().OxidesResult.Select(x=>x.Oxide.Formula).ToList();

            SetChemicalFormula(ws.Cell("AJ9").RichText, oxidesForCalculation[0]);
            SetChemicalFormula(ws.Cell("AK9").RichText, oxidesForCalculation[1]);
            SetChemicalFormula(ws.Cell("AL9").RichText, oxidesForCalculation[2]);
            SetChemicalFormula(ws.Cell("AM9").RichText, oxidesForCalculation[3]);

            SetChemicalFormula(ws.Cell("AN9").RichText, oxidesForCalculation[0]);
            SetChemicalFormula(ws.Cell("AO9").RichText, oxidesForCalculation[1]);
            SetChemicalFormula(ws.Cell("AP9").RichText, oxidesForCalculation[2]);
            SetChemicalFormula(ws.Cell("AQ9").RichText, oxidesForCalculation[3]);

            SetChemicalFormula(ws.Cell("AR9").RichText, oxidesForCalculation[0]);
            SetChemicalFormula(ws.Cell("AS9").RichText, oxidesForCalculation[1]);
            SetChemicalFormula(ws.Cell("AT9").RichText, oxidesForCalculation[2]);
            SetChemicalFormula(ws.Cell("AU9").RichText, oxidesForCalculation[3]);

            SetChemicalFormula(ws.Cell("AR19").RichText, oxidesForCalculation[0]);
            SetChemicalFormula(ws.Cell("AS19").RichText, oxidesForCalculation[1]);
            SetChemicalFormula(ws.Cell("AT19").RichText, oxidesForCalculation[2]);
            SetChemicalFormula(ws.Cell("AU19").RichText, oxidesForCalculation[3]);

            SetOxidesTable(ws);
            SetSystemsPhasesHeader(ws);

            //ws.Column("J").InsertColumnsBefore(3);
            //var a = ws.Cell("F7");
            //ws.Range(7, 6, 8, 13).Merge();
            //ws.Range(5, 2, 6, 22).Merge();
        }

        private static void SetOxidesTable(IXLWorksheet ws)
        {
            var mainOxides = Model.OxidesResultModel.First().OxidesResult.Select(x => x.Oxide.Formula);
            var oxides = Model.Oxides.Where(x => !mainOxides.Contains(x.Composition.Formula));
            var countInsert = oxides.Count() - 3;
            if (countInsert > 0)
            {
                ws.Column("AV").InsertColumnsAfter(oxides.Count() - 3);//44 - 51 template column
                ws.Range(5, 44, 8, 51 + countInsert).Merge();
                ws.Range(18, 44, 18, 50 + countInsert).Merge();
            }
            var currentCol = 48;
            foreach (var oxide in oxides)
            {
                var currentCell = ws.Cell(9, currentCol);
                currentCell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                currentCell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                SetChemicalFormula(currentCell.RichText, oxide.Composition.Formula);

                var currentResCell = ws.Cell(19, currentCol);
                SetChemicalFormula(currentResCell.RichText, oxide.Composition.Formula);
                currentCol++;
            }
        }

        private static void SetSystemsPhasesHeader(IXLWorksheet ws)
        {
            var startColumnFirstSystemOrig = 6;
            var startColumnFirstSystemCalc = 12;
            var countPhases = 5;
            var startColumnSecondSystemOrig = 22;
            var startColumnSecondSystemCalc = 28;

            var firstSystemPhasesCount = Model.FirstSystem.Phases.Count;
            var secondSystemPhasesCount = Model.SecondSystem.Phases.Count;

            var countToInsert = secondSystemPhasesCount - countPhases;
            if (countToInsert > 0)
            {
                ws.Column(startColumnSecondSystemCalc).InsertColumnsAfter(countToInsert);
                ws.Range(7, startColumnSecondSystemCalc, 8, startColumnSecondSystemCalc + countToInsert + countPhases - 1).Merge();
                ws.Column(startColumnSecondSystemOrig).InsertColumnsAfter(countToInsert);
                ws.Range(7, startColumnSecondSystemOrig, 8, startColumnSecondSystemOrig + countToInsert + countPhases - 1).Merge();
                ws.Range(5, 20, 6, 35 + countToInsert + countToInsert).Merge();
            }


            countToInsert = firstSystemPhasesCount - countPhases;
            if (countToInsert > 0)
            {
                ws.Column(startColumnFirstSystemCalc).InsertColumnsAfter(countToInsert);
                ws.Range(7, startColumnFirstSystemCalc, 8, startColumnFirstSystemCalc + countToInsert + countPhases - 1).Merge();
                ws.Column(startColumnFirstSystemOrig).InsertColumnsAfter(countToInsert);
                ws.Range(7, startColumnFirstSystemOrig, 8, startColumnFirstSystemOrig + countToInsert + countPhases - 1).Merge();
                ws.Range(5, 2, 6, 19 + countToInsert + countToInsert).Merge();
            }

            var firstSystemPhases = Model.FirstSystem.Phases.Select(x => x.Formula);
            var secondSystemPhases = Model.SecondSystem.Phases.Select(x => x.Formula);
            var currentCol = 6;
            foreach (var phase in firstSystemPhases)
            {
                SetChemicalFormula(ws.Cell(9, currentCol).RichText, phase);
                currentCol++;
            }
            currentCol++;
            foreach (var phase in firstSystemPhases)
            {
                SetChemicalFormula(ws.Cell(9, currentCol).RichText, phase);
                currentCol++;
            }
            currentCol += 5;
            foreach (var phase in secondSystemPhases)
            {
                SetChemicalFormula(ws.Cell(9, currentCol).RichText, phase);
                currentCol++;
            }
            currentCol++;
            foreach (var phase in secondSystemPhases)
            {
                SetChemicalFormula(ws.Cell(9, currentCol).RichText, phase);
                currentCol++;
            }
        }

        private static void SetRows(IXLWorksheet ws)
        {
            ws.Cell(10, 1).Value = Model.NameOfMaterial;
            ws.Cell(20, 43).Value = Model.NameOfMaterial;
            var rowCount = Model.ResultTemperaturesModels.Count;
            if (rowCount > 1)
            {
                ws.Row(20).InsertRowsBelow(rowCount - 1);
                ws.Range(20, 43, 19 + rowCount, 44).Merge();

                ws.Row(10).InsertRowsBelow(rowCount - 1);
                ws.Range(10, 1, 9 + rowCount, 1).Merge();
            }
        }

        private static void SetData(IXLWorksheet ws)
        {
            var currentRow = 10;
            for (int i = 0; i < Model.ResultTemperaturesModels.Count; i++)
            {
                var currentColumn = 2;
                ws.Cell(currentRow, currentColumn).Value = Model.ResultTemperaturesModels[i].Temperature;
                currentColumn++;
                ws.Cell(currentRow, currentColumn).Value = Model.SumR2O;
                currentColumn++;
                SetDataBySystem(
                    ws,
                    Model.FirstSystemModule,
                    Model.TemperaturesModels[i].FirstSystem,
                    Model.ResultTemperaturesModels[i].FirstSystem,
                    currentRow,
                    ref currentColumn);
                SetDataBySystem(
                    ws,
                    Model.SecondSystemModule,
                    Model.TemperaturesModels[i].SecondSystem,
                    Model.ResultTemperaturesModels[i].SecondSystem,
                    currentRow,
                    ref currentColumn);
                SetSolidPhaseOxide(ws, Model.OxidesResultModel[i], currentRow, ref currentColumn);
                SetOriginalOxides(ws, Model.OxidesResultModel[i], currentRow, ref currentColumn);
                SetLiquidOxides(ws, Model.OxidesInMelt[i], currentRow, currentColumn);
                SetResLiquidOxides(ws, Model.OxidesInMeltRes[i], currentRow, currentColumn);
                currentRow++;
            }
        }

        private static void SetSolidPhaseOxide(
            IXLWorksheet ws,
            OxidesByTemperatureModel model,
            int currentRow,
            ref int currentColumn)
        {
            foreach (var oxide in model.OxidesResult)
            {
                ws.Cell(currentRow, currentColumn).Value = oxide.Percentage;
                currentColumn++;
            }
        }

        private static void SetOriginalOxides(
            IXLWorksheet ws,
            OxidesByTemperatureModel model,
            int currentRow,
            ref int currentColumn)
        {
            foreach (var oxide in model.OxidesResult)
            {
                ws.Cell(currentRow, currentColumn).Value =
                    Model.Oxides.First(x => x.Composition.Formula == oxide.Oxide.Formula).Percentage;
                currentColumn++;
            }
        }

        private static void SetLiquidOxides(
           IXLWorksheet ws,
           OxidesByTemperatureModel model,
           int currentRow,
           int currentColumn)
        {
            float sum = 0;
            foreach (var oxide in model.OxidesResult)
            {
                sum += oxide.Percentage;
                ws.Cell(currentRow, currentColumn).Value = oxide.Percentage;

                currentColumn++;
            }
            ws.Cell(currentRow, currentColumn).Value = sum;
        }

        private static void SetResLiquidOxides(
           IXLWorksheet ws,
           OxidesByTemperatureModel model,
           int currentRow,
           int currentColumn)
        {
            currentRow = currentRow + Model.OxidesResultModel.Count + 9;
            ws.Cell(currentRow, currentColumn - 1).Value = model.Temperature;
            float sum = 0;
            foreach (var oxide in model.OxidesResult)
            {
                sum += oxide.Percentage;
                ws.Cell(currentRow, currentColumn).Value = oxide.Percentage;

                currentColumn++;
            }

            ws.Cell(currentRow, currentColumn).Value = model.OxidesResult.Sum(x => x.Percentage);
        }

        private static void SetDataBySystem(IXLWorksheet ws, float systemModule, PhaseSystemModel systemOrig, PhaseSystemModel systemCalc, int currentRow, ref int currentColumn)
        {
            ws.Cell(currentRow, currentColumn).Value = systemModule;
            currentColumn++;
            ws.Cell(currentRow, currentColumn).Value = systemOrig.SumLiquid;
            currentColumn++;
            SetPhasesData(ws, systemOrig.Phases, currentRow, ref currentColumn);
            ws.Cell(currentRow, currentColumn).Value = systemCalc.SumLiquid;
            currentColumn++;
            SetPhasesData(ws, systemCalc.Phases, currentRow, ref currentColumn);
            SetOxidesBySystem(ws, systemCalc.SolidSumOxides, currentRow, ref currentColumn);
        }

        private static void SetOxidesBySystem(
            IXLWorksheet ws,
            List<OxideResultModel> oxides,
            int currentRow,
            ref int currentColumn)
        {
            foreach (var oxide in oxides)
            {
                ws.Cell(currentRow, currentColumn).Value = oxide.Percentage;
                currentColumn++;
            }
        }

        private static void SetPhasesData(IXLWorksheet ws, List<PhaseModel> phases, int currentRow, ref int currentColumn)
        {
            foreach (var phase in phases)
            {
                ws.Cell(currentRow, currentColumn).Value = phase.Percentage;
                currentColumn++;
            }
        }

        private static void SetChemicalFormula(IXLRichText text, string formula)
        {
            text.ClearText();
            foreach (var item in formula.ToCharArray())
            {
                var current = text.AddText(item.ToString());
                if (Regex.IsMatch(item.ToString(), @"\d"))
                {
                    current.VerticalAlignment = XLFontVerticalTextAlignmentValues.Subscript;
                }
            }
        }
    }
}