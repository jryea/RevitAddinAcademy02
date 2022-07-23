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

            

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Revit stuff");

                SearchTitleBlocks(doc);


                t.Commit();
            }
            
            return Result.Succeeded;
        }
        internal void SearchTitleBlocks(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            collector.WhereElementIsElementType();

            foreach (ElementType element in collector)
            {
                TaskDialog.Show("Element Type", element.Name);
            }
        }
    }


}
