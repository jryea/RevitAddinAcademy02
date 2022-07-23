#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Mechanical;

#endregion

namespace RevitAddinAcademy02
{
    [Transaction(TransactionMode.Manual)]
    public class cmdElementsFromLines : IExternalCommand
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

            IList<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select lines for element creation: ");
            List<CurveElement> curveList = new List<CurveElement>();

            WallType glazWallType = GetWallType(doc, "Storefront");
            WallType genWallType = GetWallType(doc, @"Generic - 8""");
            DuctType ductType = GetDuctType(doc, "Default");
            PipeType pipeType = GetPipeType(doc, "Default");
            MEPSystemType pipeSystemType = GetSystemType(doc, "Other");
            MEPSystemType ductSystemType = GetSystemType(doc, "Supply Air");
            Level curLevel = GetLevelByName(doc, "Level 1");


            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Revit stuff");

                foreach(Element curElem in pickList)
                {
                    if (curElem is CurveElement)
                    {
                        //CurveElement curve = (CurveElement)curElem;
                        CurveElement curve = curElem as CurveElement;

                        curveList.Add(curve);

                        GraphicsStyle curGS = curve.LineStyle as GraphicsStyle;

                        Curve curCurve = curve.GeometryCurve;

                        XYZ startPoint = null;
                        XYZ endPoint = null;

                        try
                        {
                            startPoint = curCurve.GetEndPoint(0);
                            endPoint = curCurve.GetEndPoint(1);
                        }
                        catch
                        {
                            Debug.Print("No endpoints found");   
                        }

                        switch (curGS.Name)
                        {
                            case "A-GLAZ":
                                Wall glazWall = Wall.Create(doc,curCurve,glazWallType.Id,curLevel.Id,10,0,false,false);
                                break;
                            case "A-WALL":
                                Wall genWall = Wall.Create(doc,curCurve,genWallType.Id,curLevel.Id,10,0,false,false);
                                break;
                            case "P-PIPE":
                                Pipe newPipe = Pipe.Create(doc, pipeSystemType.Id, pipeType.Id, curLevel.Id, startPoint, endPoint);
                                break;
                            case "M-DUCT":
                                Duct newDuct = Duct.Create(doc,ductSystemType.Id,ductType.Id,curLevel.Id,startPoint,endPoint);
                                break;
                            default:
                                doc.Delete(curve.Id);
                                break;
                        }
                    }
                }


                t.Commit();
            }
            
            return Result.Succeeded;
        }
        private WallType GetWallType(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(WallType));
            foreach (Element curElem in collector)
            {
                WallType wallType = curElem as WallType;
                if (wallType.Name == typeName)
                    return wallType;
            }
            return null;    
        }
    
        private PipeType GetPipeType(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(PipeType));
            foreach (Element curElem in collector)
            {
                PipeType pipeType = curElem as PipeType;
                if (curElem.Name == typeName)
                    return pipeType;
            }
            return null;
        }
        private MEPSystemType GetSystemType(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(MEPSystemType));
            foreach (Element curElem in collector)
            {
                MEPSystemType mepSystemType = curElem as MEPSystemType;
                if (mepSystemType.Name == typeName)
                    return mepSystemType;
            }
            return null;
        }
        private DuctType GetDuctType(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(DuctType));
            foreach (Element curElem in collector)
            {
                DuctType ductType = curElem as DuctType;
                if (ductType.Name == typeName)
                    return ductType;
            }
            return null;
        }
       
        private Level GetLevelByName(Document doc, string levelName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Level));
            foreach(Element curElement in collector)
            {
                Level level = curElement as Level;
                if (level.Name == levelName)
                    return level;
            }
            return null;
        }
    }
}
