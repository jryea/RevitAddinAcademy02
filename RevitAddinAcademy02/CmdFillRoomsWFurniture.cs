#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Forms = System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

#endregion

namespace RevitAddinAcademy02
{
    [Transaction(TransactionMode.Manual)]
    public class CmdFillRoomsWFurniture : IExternalCommand
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

            //Collect data from Excel file
            List<List<string>> excelDataFurnSets = Utilities.GetExcelFileData(doc, @"D:\Learning\RevitAddinAcademy\Session05_Furniture-220729-130011.xlsx", 1);
            List<List<string>> excelDataFurnTypes = Utilities.GetExcelFileData(doc, @"D:\Learning\RevitAddinAcademy\Session05_Furniture-220729-130011.xlsx", 2);

            //Creating lists of Furniture and Furniture Sets
           List<Furniture> furnitureList = new List<Furniture>();
           List<FurnitureSet> furnitureSetList = new List<FurnitureSet>();
            
            foreach (List<string> row in excelDataFurnTypes)
            {
                Furniture curFurn = new Furniture(row[0],row[1],row[2]);
                furnitureList.Add(curFurn);
            }
            foreach (List<string> row in excelDataFurnSets)
            {
                FurnitureSet curFurnSet = new FurnitureSet(row[0], row[1], row[2], excelDataFurnTypes);
                furnitureSetList.Add(curFurnSet);
            }

            List<SpatialElement> allRooms = Utilities.GetAllRooms(doc);

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Insert furniture");

                foreach (SpatialElement room in allRooms)
                {
                    string curFurnSetParam = Utilities.GetParamValueAsString(room, "Furniture Set");
                    foreach (FurnitureSet curFurnSet in furnitureSetList)
                    {
                        if (curFurnSetParam == curFurnSet.FurnSetName)
                        {
                            foreach (Furniture furn in curFurnSet.IncludedFurniture)
                            {
                                furn.CreateFamilyInstance(doc, room);
                            }
                            Utilities.SetParamValue(room, "Furniture Count", curFurnSet.RoomCount);
                        }

                    }
                }


                t.Commit();
            }



            return Result.Succeeded;
        }
    }


}
