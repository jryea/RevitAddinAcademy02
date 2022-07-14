#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Forms = System.Windows.Forms;

#endregion

namespace RevitAddinAcademy02
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
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

            Forms.OpenFileDialog dialog = new Forms.OpenFileDialog();               //Creates a Open File Dialog
            dialog.InitialDirectory = @"C:\";                                    //Creates an initial path location
            dialog.Multiselect = false;                                          //File Dialogs can be single file or multiple files
            dialog.Filter = "Revit Files | *.rvt; *.rfa";                       //Limits file selection to a certain file type

            string filePath = "";                                                //Empty string variable for the file path
            string[] filePaths;

            if (dialog.ShowDialog() == Forms.DialogResult.OK)                    //Confirmed on user hitting the Ok button when selecting file
            {
                filePath = dialog.FileName;                                      //sets file path variable to file name selected
                //filePaths = dialog.FileNames;                                  //Option for setting multiple file selection paths in a variable
            } 

            Forms.FolderBrowserDialog folderDialog = new Forms.FolderBrowserDialog();       //Creates a Folder Browser Dialog

            string folderPath = "";
            if(folderDialog.ShowDialog() == Forms.DialogResult.OK)
            {

            }

            Tuple<string, int> t1 = new Tuple<string, int>("string 1", 55);        //Creating a tuple      Can hold multiple data types
            Tuple<string, int> t2 = new Tuple<string, int>("string 2", 155);

            TestStruct struct1;
            struct1.Name = "Structure 1";
            struct1.Value = 100;
            struct1.Value2 = 10.5;

            TestStruct struct2 = new TestStruct("Name", 17, 14.8);
            
            List<TestStruct> structList = new List<TestStruct>();
            structList.Add(struct1);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(ViewFamilyType));

            ViewFamilyType curVFT = null;
            ViewFamilyType curRCPVFT = null;

            foreach(ViewFamilyType curElem in collector)
            {
                if(curElem.ViewFamily == ViewFamily.FloorPlan)
                {
                    curVFT = curElem;
                }
                else if(curElem.ViewFamily == ViewFamily.CeilingPlan)
                {
                    curRCPVFT = curElem;
                }
            }



            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);
            collector2.WhereElementIsElementType();
            
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Revit stuff");

                Level newLevel = Level.Create(doc, 100);
                ViewPlan curPlan = ViewPlan.Create(doc, curVFT.Id, newLevel.Id);
                ViewPlan curRCP = ViewPlan.Create(doc, curRCPVFT.Id, newLevel.Id);
                curRCP.Name = curRCP.Name + " RCP";

                View existingView = GetViewByName(doc, "Level 1");

                ViewSheet newSheet = new ViewSheet.Create(doc, collector2.FirstElementId());

                Viewport newVP = Viewport.Create(doc, newSheet.ID, curPlan.Id, new XYZ(0, 0, 0));           //XYZ of 0,0,0 of a sheet should be the lower left corner

                newSheet.Name = "TEST SHEET";
                newSheet.SheetNumber = "A100101010";

                foreach(Parameter curParam in newSheet.Parameters)               //Loops through every parameter of that element
                {
                    if(curParam.Definition.Name == "Drawn By")
                    {
                        curParam.Set("JR");
                    }
                }
                
                t.Commit();
            }
            
            return Result.Succeeded;
        }

        internal View GetViewByName(Document doc, string viewName)                          //Method creation to simplify code
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(View));

            foreach(View curView in collector)
            {
                if(curView.Name == viewName)
                {
                    return curView;
                }

            }
            return null;                                                                    //Good practice to return default value of 'Null' if nothing found
        }
        
        internal struct TestStruct
        {
            public string Name;
            public int Value;
            public double Value2;  

            public TestStruct(string name, int value, double value2)
            {
                Name = name;
                Value = value;
                Value2 = value2;
            }
        }
    }
}
