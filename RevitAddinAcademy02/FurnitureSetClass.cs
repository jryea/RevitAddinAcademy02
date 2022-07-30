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
using System.Diagnostics;
using Forms = System.Windows.Forms;
using Autodesk.Revit.DB.Structure;

namespace RevitAddinAcademy02
{
    internal class FurnitureSet
    {
        //Fields
        //List<Furniture> includedFurniture = new List<Furniture>();  
        
        //Properties
        public string FurnSetName { get; set; }
        public string RoomType { get; set; }
        public string AllFurn { get; set; }
        public double RoomCount { get; set; }
        public List<string> FurnList { get; set; }

        public List<Furniture> IncludedFurniture { get; set; }

        
        internal FurnitureSet(string furnSetName, string roomType, string allFurn, List<List<string>> furnTypes)
        {
            this.FurnSetName = furnSetName;
            this.RoomType = roomType;
            this.AllFurn = allFurn;
            this.FurnList = FormatStringList(allFurn);
            this.IncludedFurniture = CreateIncludedFurnList(FurnList);
            this.RoomCount = FurnList.Count; 

            List<Furniture> CreateIncludedFurnList (List<string> incFurnTypes)
            {
                List<Furniture> includedFurniture = new List<Furniture>();
                foreach (string furn in incFurnTypes)
                {
                    foreach (List<string> furnType in furnTypes)
                    {
                        if (furnType[0] == furn)
                        {
                            Furniture curFurn = new Furniture(furnType[0], furnType[1], furnType[2]);
                            includedFurniture.Add(curFurn);
                        }
                    }
                }
                return includedFurniture;
            }

            List<string> FormatStringList (string inputString)
            {
                List<string> stringToList = inputString.Split(',').ToList();
                List<string> cleanStringList = new List<string>();
                foreach(string str in stringToList)
                {
                    if (str[0] == ' ')
                    {
                        cleanStringList.Add(str.Remove(0, 1)); 
                    }
                    else
                    {
                        cleanStringList.Add(str);
                    }
                }
                return cleanStringList;
            }
        }
    }
}
