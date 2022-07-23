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
    public class cmdWallsFromLines : IExternalCommand
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

            List<string> wallTypes = GetAllWallTypeNames(doc);
            List<string> lineStyles = GetAllLineStyleNames(doc);

            FrmWallsFromLines curForm = new FrmWallsFromLines(wallTypes, lineStyles);
            curForm.Height = 450;
            curForm.Width = 550;
            curForm.StartPosition = Forms.FormStartPosition.CenterScreen; 

            if(curForm.ShowDialog() == Forms.DialogResult.OK)
            {
                string selectedWallType = curForm.GetSelectedWallType();
                string selectedLineStyle = curForm.GetSelectedLineStyle();
                double wallHeight = curForm.GetWallHeight();
                bool isStructural = curForm.AreWallStructural();

                WallType curWallType = GetWallTypeByName(doc, selectedWallType);
                List<CurveElement> curves = GetLinesByStyle(doc, selectedLineStyle);
                Level curLevel = GetLevelFromView(doc);

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Create walls from lines");

                    foreach(CurveElement curve in curves)
                    {
                        Curve curCurve = curve.GeometryCurve;

                        Wall newWall = Wall.Create(
                            doc,
                            curCurve,
                            curWallType.Id,
                            curLevel.Id,
                            wallHeight,
                            0,
                            false,
                            isStructural);
                    }

                    t.Commit();
                }
            }

            
            
            return Result.Succeeded;
        }

        private List<CurveElement> GetLinesByStyle(Document doc, string selectedLineStyle)
        {
            List<CurveElement> results = new List<CurveElement>();

            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            collector.OfClass(typeof(CurveElement));

            foreach (CurveElement element in collector)
            {
                GraphicsStyle curGS = element.LineStyle as GraphicsStyle;

                if (curGS.Name == selectedLineStyle)
                {
                    results.Add(element);
                } 
            }
            return results;
        }

        private Level GetLevelFromView(Document doc)
        {
            View curView = doc.ActiveView;
            /*
            SketchPlane curSP = curView.SketchPlane;
            SketchPlane curSP = curView.SketchPlane;        //SketchPlane associates a plan view with the elevation its at
   
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Level));
            collector.WhereElementIsNotElementType();

            foreach (Level curLevel in collector)
            {
                if(curLevel.Name == curSP.Name)
                    return curLevel;
            }
            return collector.FirstElement() as Level;
            */
            return curView.GenLevel;
        }

        private WallType GetWallTypeByName(Document doc, string selectedWallType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(WallType));

            foreach (WallType wallType in collector)
            {
                if(wallType.Name == selectedWallType)
                    return wallType;
            }

            return null;   
        }

        private List<string> GetAllLineStyleNames(Document doc)
        {
            List<string> lineStyles = new List<string>();

            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            collector.OfClass(typeof(CurveElement));

            foreach(CurveElement element in collector)
            {
                GraphicsStyle curGS = element.LineStyle as GraphicsStyle;

                if(lineStyles.Contains(curGS.Name) == false)
                {
                    lineStyles.Add(curGS.Name);
                }
            }
            return lineStyles; 
        }

        private List<string> GetAllWallTypeNames(Document doc)
        {
            List<string> wallTypes = new List<string>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(WallType));

            foreach (WallType wallType in collector)
            {
                wallTypes.Add(wallType.Name);
            }
            return wallTypes;
        }
    }


}
