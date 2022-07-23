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

            TaskDialog.Show("Project Setup", "This is the Project Setup");

            string excelPath = @"C:\Users\Jon.R.Ryea\Documents\Revit Addin Academy\Excel\Session02_Challenge.xlsx";

            Excel.Application excelapp = new Excel.Application();               //Opens Excel
            Excel.Workbook wkBook = excelapp.Workbooks.Open(excelPath);         //Opens Excel Workbook (File)
            Excel.Worksheet wkSheet1 = wkBook.Worksheets.Item[1];                //Opens first Worksheet (starts at index of 1)
            Excel.Worksheet wkSheet2 = wkBook.Worksheets.Item[2];

            Excel.Range excelRng1 = wkSheet1.UsedRange;
            int rowCount1 = excelRng1.Rows.Count;
            Excel.Range excelRng2 = wkSheet2.UsedRange;
            int rowCount2 = excelRng2.Rows.Count;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Transaction");

                // Creating Levels

                for (int i = 2; i <= rowCount1; i++)
                {
                    Excel.Range cellLevelName = wkSheet1.Cells[i, 1];       //Sets the cell to a variable
                    Excel.Range cellElevation = wkSheet1.Cells[i, 2];

                    string levelName = cellLevelName.Value.ToString();      //Returns the value of a cell
                    double elevation = cellElevation.Value;
                
                    Level level = Level.Create(doc,elevation);              //Creating the level
                    level.Name = levelName;                                 //Setting the name of the level
                }

                // Creating Sheets

                for (int i = 2; i <= rowCount2; i++)
                {

                    FilteredElementCollector collector = new FilteredElementCollector(doc);
                    collector.OfCategory(BuiltInCategory.OST_TitleBlocks);                              //Collects the titleblocks
                    collector.WhereElementIsElementType();                                              //Method that sets the element collection to Type

                    Excel.Range cellSheetNumber = wkSheet2.Cells[i, 1];
                    Excel.Range cellSheetName = wkSheet2.Cells[i,2];

                    string sheetNumber = cellSheetNumber.Value.ToString();
                    string sheetName = cellSheetName.Value.ToString(); 
                      

                    ViewSheet sheet = ViewSheet.Create(doc, collector.FirstElementId());                //Creates a new sheet
                    sheet.SheetNumber = sheetNumber;
                    sheet.Name = sheetName;
                }

                t.Commit();
            }

            return Result.Succeeded;

            // do some stuff in Excel

            wkBook.Close();                             //Close File
            excelapp.Quit();                            //Close Excel Application
        }
    }
}
