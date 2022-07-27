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
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Forms = System.Windows.Forms;

namespace RevitAddinAcademy02
{
    internal class myClass
    {
    }

    public class Employee
    {
        public string Name { get; set; }
        public int Age { get; set; }  
        public List<string> FavColors { get; set; }

        public Employee(string name, int age, string favColors)
        {
            this.Name = name;
            this.Age = age;
            this.FavColors = FormatColorList(string colorList);

            private  List<string> FormatColorList(string colorList)
            {
                List<string> returnList = colorList.Split(',').ToList();
                return returnList;
            }
        }   
    }
    public class Employees
    {
        public List<Employee> EmployeeList { get; set; }

        public 
    }

    public static class Utilities
    {
        public static string GetTextFromClass()
        {
            return "I got this text from a static class";
        }

        public static List<SpatialElement> GetAllRooms(Document doc)                          //Spatial Elements include rooms, areas, and spaces
        {
            FilteredElementCollector collector = new FiltredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_Rooms);

            List<SpatialElement> roomList = new List<SpatialElement>();

            foreach(Element curElem in collector)
            {
                SpatialElement curRoom = curElem as SpatialElement;
                roomList.Add(curRoom);
            }
        }

        public static FamilySymbol GetFamilySymbolByName(Document doc, string familyName, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach(Element element in collector)
            {
                Family curFamily = element as Family;

                if(curFamily.Name == familyName)
                {
                    ISet<ElementId> famSymbolIdList = curFamily.GetFamilySymbolIds();

                    foreach(ElementId famSymbolId in famSymbolIdList)
                    {
                        FamilySymbol curFS = doc.GetElement(famSymbolId) as FamilySymbol;

                        if (curFS.Name == typeName)
                            return curFS;
                    }
                }
            }

            return null;
        }

        public static string GetParamValue(Element curElem, string paramName)
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
            foreach (Parameter curParam in curElem.Parameters)
            {
                if (curParam.Definition.Name == paramName)
                    return curParam.AsDouble();
            }
            return 0;
        }

        public static void SetParamValue(Element curElem, string paramName, string paramValue)
        {
            foreach (Parameter curParam in curElem.Parameters)
            {
                if (curParam.Definition.Name == paramName)
                    curParam.Set(paramValue);
            }

            foreach (Parameter curParam in curElem.Parameters)
            {
                if (curParam.Definition.Name == paramName)
                    return curParam.AsDouble();
            }
            return 0;
        }
    }
}
