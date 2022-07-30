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
    internal class Furniture
    {
  
        //Properties
        public string Name {get; set;}
        public string FamilyName {get; set;}
        public string TypeName {get; set;}  

        //Constructor
        internal Furniture (string name, string familyName, string typeName)
        {
            this.Name = name;
            this.FamilyName = familyName;
            this.TypeName = typeName;
        }

        //Methods
        public FamilyInstance CreateFamilyInstance(Document doc, SpatialElement room)
        {
            FamilySymbol curFS = Utilities.GetFamilyTypeByName(doc,FamilyName,TypeName);
            curFS.Activate();
            LocationPoint roomLocation = room.Location as LocationPoint;
            XYZ roomPoint = roomLocation.Point;
            FamilyInstance curFI = doc.Create.NewFamilyInstance(roomPoint, curFS, StructuralType.NonStructural);
            return curFI;
        }

    }
}
