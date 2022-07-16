#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using Forms = System.Windows.Forms;

#endregion

namespace RevitAddinAcademy02
{
    [Transaction(TransactionMode.Manual)]
    public class cmdProjectSetup : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            string filePath = GetExcelWorksheet();
            LevelData levelData = new LevelData(filePath);
            SheetData sheetData = new SheetData(filePath);

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Transaction");

                //Create Levels and Views

                ViewFamilyType curVFT = CollectVFType(doc);
                ViewFamilyType curRCPVFT = CollectVFType(doc, "ceiling");

                for (int i = 0; i<(levelData.Name.Length); i++)
                {
                    Level newLevel = Level.Create(doc, levelData.Elevation[i]);
                    newLevel.Name = levelData.Name[i];

                    ViewPlan newFloorPlan = ViewPlan.Create(doc, curVFT.Id, newLevel.Id);
                    ViewPlan newRCP = ViewPlan.Create(doc, curRCPVFT.Id, newLevel.Id);
                    newRCP.Name = newRCP.Name + " RCP";
                }


                //Create Sheets and Place Views on Sheets
                Element titleblock = GetTitleblock(doc, "E1 30x42 Horizontal");
                for (int i = 0; i < (sheetData.Name.Length); i++)
                {
                    ViewSheet newSheet = ViewSheet.Create(doc, titleblock.Id);
                    newSheet.Name = sheetData.Name[i];
                    newSheet.SheetNumber = sheetData.Number[i];
                    SetSheetParam(newSheet, "Drawn By", sheetData.DrawnBy[i]);
                    SetSheetParam(newSheet, "Checked By", sheetData.CheckedBy[i]);

                    View existingView = GetViewByName(doc, sheetData.View[i]);
                    if (existingView != null)
                    {
                        Viewport newVP = Viewport.Create(doc, newSheet.Id, existingView.Id, new XYZ(0,0,0));
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Could not find view");
                    }
                }
                    t.Commit();
            }

            return Result.Succeeded;

        }

        internal string GetExcelWorksheet()
        {
            Forms.OpenFileDialog dialog = new Forms.OpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            dialog.Multiselect = true;
            dialog.Filter = "Excel Files | *.xlsx; *.xls; *.xlsm";

            string filePath = "";
            if (dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                filePath = dialog.FileName;
            }
            return filePath;
        }

        internal ElementType GetTitleblock(Document doc, string tbName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_TitleBlocks);                         
            collector.WhereElementIsElementType(); 
            
            foreach (ElementType element in collector)
            {
                if (element.Name == tbName)
                {
                    return element;
                }  
            }
            return null;
        }

        internal View GetViewByName(Document doc, string viewName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(View));
           
            foreach (View curView in collector)
            {
                if (curView.Name == viewName)
                {
                    return curView;
                }
            }
            return null;
        }

        internal void SetSheetParam (ViewSheet sheet, string paramName, string value)
        {
            bool err = false;
            foreach (Parameter curParam in sheet.Parameters)
            {
                if (curParam.Definition.Name == paramName)
                {
                    curParam.Set(value);
                }
            }
        }

        internal ViewFamilyType CollectVFType (Document doc, string planVFT = "floor")
        {
            FilteredElementCollector collectorVFT = new FilteredElementCollector(doc);
            collectorVFT.OfClass(typeof(ViewFamilyType));

            ViewFamilyType curVFT = null;
            ViewFamilyType curRCPVFT = null;
            foreach (ViewFamilyType element in collectorVFT)
            {
                if (element.ViewFamily == ViewFamily.FloorPlan)
                {
                    curVFT = element;
                }
                else if (element.ViewFamily == ViewFamily.CeilingPlan)
                {
                    curRCPVFT = element;
                }
            }
            if (planVFT == "floor")
            {
                return curVFT;
            }
            else if (planVFT == "ceiling")
            {
                return curRCPVFT;
            }
            else
            {
                return null;
            }
        }

        internal struct LevelData                             
        {
            public Excel.Application Excelapp;
            public Excel.Workbook WkBook;
            public Excel.Worksheet WkSheet;
            public Excel.Range ExcelRng;
            public int RowCount;
            public string[] Name;
            public double[] Elevation;
            
            public LevelData(string filePath)                          
            {
                Excelapp = new Excel.Application();
                WkBook = Excelapp.Workbooks.Open(filePath);
                WkSheet = WkBook.Worksheets.Item[1];
                ExcelRng = WkSheet.UsedRange;
                RowCount = ExcelRng.Rows.Count;
                Name = new string[RowCount-1];
                Elevation = new double[RowCount-1];

                for (int i = 2; i <= RowCount; i++)
                {
                    Excel.Range cellLevelName = WkSheet.Cells[i,1];
                    Excel.Range cellElevation = WkSheet.Cells[i,2];
                    Name[i-2] = cellLevelName.Value.ToString();
                    Elevation[i-2] = cellElevation.Value;                             
                }
                WkBook.Close();
                Excelapp.Quit();
            }
        }
        internal struct SheetData
        {
            public Excel.Application Excelapp;
            public Excel.Workbook WkBook;
            public Excel.Worksheet WkSheet;
            public Excel.Range ExcelRng;
            public int RowCount;
            public string[] Number;
            public string[] Name;
            public string[] View;
            public string[] DrawnBy;
            public string[] CheckedBy;

            public SheetData(string filePath)
            {
                Excelapp = new Excel.Application();
                WkBook = Excelapp.Workbooks.Open(filePath);
                WkSheet = WkBook.Worksheets.Item[2];
                ExcelRng = WkSheet.UsedRange;
                RowCount = ExcelRng.Rows.Count;
                Number = new string[RowCount - 1];
                Name = new string[RowCount - 1];
                View = new string[RowCount - 1];
                DrawnBy = new string[RowCount - 1];
                CheckedBy = new string[RowCount - 1];


                for (int i = 2; i <= RowCount; i++)
                {
                    Excel.Range cellSheetNum = WkSheet.Cells[i, 1];
                    Excel.Range cellSheetName = WkSheet.Cells[i, 2];
                    Excel.Range cellView = WkSheet.Cells[i, 3];
                    Excel.Range cellDrawnBy = WkSheet.Cells[i, 4];
                    Excel.Range cellCheckedBy = WkSheet.Cells[i, 5];

                    Number[i - 2] = cellSheetNum.Value.ToString();
                    Name[i - 2] = cellSheetName.Value.ToString();
                    View[i - 2] = cellView.Value.ToString();
                    DrawnBy[i - 2] = cellDrawnBy.Value.ToString();
                    CheckedBy[i - 2] = cellCheckedBy.Value.ToString();
                }
                WkBook.Close();
                Excelapp.Quit();
            }
        }
    }
}
