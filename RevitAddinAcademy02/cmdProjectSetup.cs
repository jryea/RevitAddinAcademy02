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

            //Creating Open File Dialog
            string excelPath = "";
            Forms.OpenFileDialog fileDialog = new Forms.OpenFileDialog();
            fileDialog.Filter = "Excel File | *.xlsx; *.xls";
            fileDialog.Multiselect = false;
            fileDialog.InitialDirectory = @"C:\";

            if (fileDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                excelPath = fileDialog.FileName;    
            }

            //Opening Excel file
            Excel.Application excelapp = new Excel.Application();               //Opens Excel
            Excel.Workbook wkBook = excelapp.Workbooks.Open(excelPath);         //Opens Excel Workbook (File)
            Excel.Worksheet wkSheet1 = wkBook.Worksheets.Item[1];               //Opens first Worksheet (starts at index of 1)
            Excel.Worksheet wkSheet2 = wkBook.Worksheets.Item[2];

  
            Excel.Range excelRng1 = wkSheet1.UsedRange;
            Excel.Range excelRng2 = wkSheet2.UsedRange;
            int rowCount1 = excelRng1.Rows.Count;
            int rowCount2 = excelRng2.Rows.Count;


            //Collecting Titleblocks
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_TitleBlocks);                              //Collects the titleblocks
            collector.WhereElementIsElementType();                                              //Method that sets the element collection to Type
            
            //Collecting views
            FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
            viewCollector.OfClass(typeof(ViewFamilyType));                                                            

            ViewFamilyType floorVFT = null;
            ViewFamilyType rcpVFT = null;

            foreach (ViewFamilyType type in viewCollector)
            {
                if (type.ViewFamily == ViewFamily.FloorPlan)
                {
                    floorVFT = type;
                }
                else if (type.ViewFamily == ViewFamily.CeilingPlan)
                {
                    rcpVFT = type;
                }
            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Transaction");

               
                // Creating Levels and Views

                for (int i = 2; i <= rowCount1/2; i++)
                {
                    Excel.Range cellLevelName = wkSheet1.Cells[i, 1];                    //Sets the cell to a variable
                    Excel.Range cellElevation = wkSheet1.Cells[i, 2];
      
                    string levelName = cellLevelName.Value.ToString();                   //Returns the value of a cell
                    double elevation = cellElevation.Value;
                
                    Level level = Level.Create(doc,elevation);                           //Creating the level
                    level.Name = levelName;                                              //Setting the name of the level
                    ViewPlan curPlan = ViewPlan.Create(doc, floorVFT.Id, level.Id);     //Creating a floor plan of the current level
                    ViewPlan curRCP = ViewPlan.Create(doc, rcpVFT.Id, level.Id);        //Creating a RCP of the current level
                    curRCP.Name = curRCP.Name + " RCP";
                }

                // Creating Sheets

                                                              
                FilteredElementCollector collector3 = new FilteredElementCollector(doc);  //Collecting Views
                collector3.OfClass(typeof(View));

                for (int i = 2; i <= rowCount2; i++)
                {

                   
                    Excel.Range cellSheetNumber = wkSheet2.Cells[i, 1];
                    Excel.Range cellSheetName = wkSheet2.Cells[i,2];
                    Excel.Range cellView = wkSheet2.Cells[i, 3];
                    Excel.Range cellDrawnBy = wkSheet2.Cells[i, 4];
                    Excel.Range cellCheckedBy = wkSheet2.Cells[i, 5];

                    string sheetNumber = cellSheetNumber.Value.ToString();
                    string sheetName = cellSheetName.Value.ToString();
                    string viewName = cellView.Value.ToString();
                    string sheetDrawnBy = cellDrawnBy.Value.ToString();
                    string sheetCheckedBy = cellCheckedBy.Value.ToString();

                    ViewSheet sheet = ViewSheet.Create(doc, collector.FirstElementId());                //Creates a new sheet
                    sheet.SheetNumber = sheetNumber;
                    sheet.Name = sheetName;

                    foreach(Parameter curParam in sheet.Parameters)                                     //Cycles through all parameters of the current sheet
                    {
                        if(curParam.Definition.Name == "Drawn By")
                        {
                            curParam.Set(sheetDrawnBy);
                        }
                        if(curParam.Definition.Name == "Checked By")
                        {
                            curParam.Set(sheetCheckedBy);
                        }
                    }

                    //Setting Views on Sheets

                    View existingView = null;
                    foreach(View curView in collector3)
                    {
                        if(curView.Name == viewName)
                        {
                            existingView = curView;
                            TaskDialog.Show("existingView.Name", existingView.Name);
                            TaskDialog.Show("CurView.Name", curView.Name);
                            TaskDialog.Show("viewName", viewName);
                        }
                        
                    }

                    if (existingView != null)
                    {

                        Viewport newVP = Viewport.Create(doc, sheet.Id, existingView.Id, new XYZ(0, 0, 0));
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Could not find view");
                    }
                    


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
