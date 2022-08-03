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
    public class CmdInsertFurnitureCleaned : IExternalCommand
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

            int counter = 0;

            string excelFile = "";

            Forms.OpenFileDialog ofd = new Forms.OpenFileDialog();
            ofd.Title = "Select Furniture Excel File";
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "Excel files (*.xlsx)|*.xlsx";

            if (ofd.ShowDialog() != Forms.DialogResult.OK)
                return Result.Failed;

            excelFile = ofd.FileName;

            List<string[]> excelFurnSetData = GetDataFromExcel(excelFile, "Furniture sets", 3);
            List<string[]> excelFurnData = GetDataFromExcel(excelFile, "Furniture types", 3);

            excelFurnSetData.RemoveAt(0);
            excelFurnSetData.RemoveAt(0);

            List<FurnSet> furnSetList = new List<FurnSet>();
            List<FurnData> furnDataList = new List<FurnData>();

            foreach (string[] curRow in excelFurnSetData)
            {
                FurnSet tmpFurnSet = new FurnSet(curRow[0].Trim(), curRow[1].Trim(), curRow[2].Trim());
                furnSetList.Add(tmpFurnSet);
            }

            foreach (string[] curRow in excelFurnData)
            {
                FurnData tmpFurnData = new FurnData(curRow[0].Trim(), curRow[1].Trim(), curRow[2].Trim());
                furnDataList.Add(tmpFurnData);
            }

            List<SpatialElement> roomList = GetAllRooms(doc);

            foreach (SpatialElement room in roomList)
            {
                string curFurnSet = GetParamValue(room, "Furniture Set");

                foreach(FurnSet tmpFurnSet in furnSetList)
                {
                    if (tmpFurnSet.SetName == curFurnSet)
                    {
                        foreach(string curFurn in tmpFurnSet.FurnList)
                        {
                            FurnData fd = GetFamilyInfo(curFurn, furnDataList);

                            if(fd != null)
                            {
                                LocationPoint roomPoint = Room.Location as LocationPoint;
                                XYZ insPoint = roomPoint.Point;

                                FamilyInstance newFamINst = doc.Create.NewFamilyInstance(insPoint, fd.FamilySymbol, StructuralType.NonStructural);
                                counter++;
                            }
                            
                        }
                    }
                    SetParamValueAsInt(curRoom, "Furniture Count", tmpFurnSet.FurnList.Count);
                }

            }

            return Result.Succeeded;
        }

        private FurnData GetFamilyInfo(string curFurn, List<FurnData> furnDataList)
        {
            foreach(FurnData furn in furnDataList)
                if (FurnData != null)
                {
                    
                }
            }

        private string GetParamValue(Element curElem, string paramName)
        {
            foreach (Parameter curParam in curElem.Parameters())
            {
                if (curParam.Definition.Name == paramName)
                {
                    return curParam.AsString();
                }
            }
        }

        private string GetParamValue(Element curElem, string paramName)
        {
            foreach(Parameter curParam in curElem.Parameters())
            {
                if(curParam.Definition.Name == paramName)
                {
                    return curParam.AsString();
                }
            }
        }

        private List<SpatialElement> GetAllRooms(Document doc)
        {
            List<SpatialElement> returnList = new List<SpatialElement>;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_Rooms);
            collector.WhereElementIsNotElementType();

            foreach(Element curElem in collector)
            {
                SpatialElement curRoom = curElem as SpatialElement;
                returnList.Add(curRoom);
            }

        }

        private List<string[]> GetDataFromExcel(string excelFile, string wsName, int numColumns)
        {
           Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWb = excelApp.Workbooks.Open(excelFile);

            Excel.Worksheet excelWs = GetExcelWorksheetByName(excelWb, wsName);
            Excel.Range excelRng = excelWs.UsedRange as Excel.Range;

            int rowCount = excelRng.Rows.Count;

            List<string[]> data = new List<string[]>(); 

            for(int i = 1; i <= rowCount; i++)
            {
                string[] rowData = new string[numColumns];

                for(int j = 1; j <= numColumns; j++)
                {
                    Excel.Range cellData = excelWs.Cells[i, j];
                    sheetData[j - 1] = cellData.Value.ToString();
                }

                data.Add(rowData);
            }

            excelWb.Close();
            excelApp.Quit();
        }

        private Excel.Worksheet GetExcelWorksheetByName(Excel.Workbook excelWb, string wsName)
        {
           foreach(Excel.Worksheet sheet in excelWb.Worksheets)
            {
                if (sheet.Name == wsName)
                    return sheet;
            }
        }
    }


}
