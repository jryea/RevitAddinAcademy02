using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Architecture;
using System.Diagnostics;
using Forms = System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace RevitAddinAcademy02
{
    public static class Utilities
    {

        public static List<List<string>> GetExcelFileData(Document doc, string filePath, int wrkSheet)
        {
            string excelFile = filePath;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open(excelFile);
            Excel.Worksheet ws = wb.Worksheets[wrkSheet];
            Excel.Range excelRange = ws.UsedRange;

            int rowCount = excelRange.Rows.Count;
            int columnCount = excelRange.Columns.Count; 
            List<List<string>> sheetData = new List<List<string>>(); 
           

            for (int i = 2; i <= rowCount; i++)
            {
                List<string> rowData = new List<string>();
                for (int j = 1; j <= columnCount; j++)
                {
                    Excel.Range cellData = ws.Cells[i,j];
                    string cellString = cellData.Value;
                    rowData.Add(cellString);
                }
                sheetData.Add(rowData);
            }
            wb.Close();
            excelApp.Quit();
            return sheetData;
        }
        public static FamilySymbol GetFamilyTypeByName(Document doc, string familyName, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach(Element curElem in collector)
            {
                Family curFamily = curElem as Family;
                if (curFamily.Name == familyName)
                {
                    ISet<ElementId> famSymbolIdList = curFamily.GetFamilySymbolIds();

                    foreach(ElementId famSymbolId in famSymbolIdList)
                    {
                        FamilySymbol famSymbol = doc.GetElement(famSymbolId) as FamilySymbol;
                        if (famSymbol.Name == typeName)
                            return famSymbol;
                    }
                }
            }
            return null;
        }
        public static List<SpatialElement> GetAllRooms(Document doc)
        { 
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_Rooms);

            List<SpatialElement> allRooms = new List<SpatialElement>();

            foreach(Element curElem in collector)
            {
                SpatialElement curRoom = curElem as SpatialElement;
                allRooms.Add(curRoom);
            }
            return allRooms;
        }
        public static string GetParamValueAsString(Element curElem, string paramName)
        {
            foreach(Parameter curParam in curElem.Parameters)
            {
                if (curParam.Definition.Name == paramName)
                    return curParam.AsString();
            }
            return null;
        }
        public static double GetParamValueAsDouble(Element curElem, string paramName)
        {
            foreach(Parameter curParam in curElem.Parameters)
            {
                if (curParam.Definition.Name == paramName)
                    return curParam.AsDouble();
            }
            return 0;

        }
        public static int GetParamValueAsInt(Element curElem, string paramName)
        {
             foreach(Parameter curParam in curElem.Parameters)
            {
                if (curParam.Definition.Name == paramName)
                    return Convert.ToInt32(curParam);
            }
            return 0;
        }
        public static bool GetParamValueAsBool(Element curElem, string paramName)
        {
              foreach(Parameter curParam in curElem.Parameters)
            {
                if (curParam.Definition.Name == paramName)
                    return Convert.ToBoolean(curParam);
            }
            return false;
        }
        public static void SetParamValue(Element curElem, string paramName, string paramValue)
        {
            foreach(Parameter curParam in curElem.Parameters)
                if (curParam.Definition.Name == paramName)
                    curParam.Set(paramValue);
        }
        public static void SetParamValue(Element curElem, string paramName, double paramValue)
        {
            foreach (Parameter curParam in curElem.Parameters)
                if (curParam.Definition.Name == paramName)
                    curParam.Set(paramValue);
        }



    }
}
