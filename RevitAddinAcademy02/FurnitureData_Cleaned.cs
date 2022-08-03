using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinAcademy02
{
    public class FurnSet
    {
        public string SetType { get; set; }
        public string SetName { get; set; }
        public List<string> FurnList { get; private set; }

        public FurnSet(string setType, string setName, string furnList)
        {
            this.setType = setType;
            this.setName = setName;
            this.furnList = GetFurnListFromString(furnList);
        } 
        
        private List<string> GetFurnListFromString(string list)
        {
            List<string> returnList = list.Split(',').ToList();

            return returnList;  
        }

        public int FurnitureCount()
        {
            return FurnList.Count;
        }
    }

    public class FurnData
    {
        public string FurnName { get; set; }
        public string FamilyName { get; set; }
        public string TypeName { get; set; }
        public FamilySymbol FamilySymbol { get; private set; }
        public Document Doc { get; set; }

        public FurnData(Document doc, string furnName, string familyName, string typeName)
        {
            this.FurnName = furnName;
            this.FamilyName = familyName;
            this.TypeName = typeName;
            this.Doc = doc;
            this.FamilySymbol = GetFamilySymbol();
        }

        private FamilySymbol GetFamilySymbol()
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach(Family curFam in collector)
            {
                if(curFam.Name == FamilyName)
                {
                    ISet<ElementId> famSymbolList = curFam.GetFamilySymbolIds();

                    foreach(ElementId curID in famSymbolList)
                    {
                        FamilySymbol curFS = Doc.GetElement(curID) as FamilySymbol;

                        if (curFS.Name == typeName)
                            return curFS;
                    }
                }
            }
        }
    }
}
