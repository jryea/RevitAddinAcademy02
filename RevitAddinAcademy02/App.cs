#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;        //Allows C# to know things about itself
                                //Add using statements from Presentation namespaces
#endregion

namespace RevitAddinAcademy02
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            //Step 1: Create Ribbon Tab
            try
            {
                a.CreateRibbonTab("Test Tab");
            }
            catch (Exception)
            {
                Debug.Print("Tab already exists");
            }

            //Step 2: Create Ribbon Panel
            //RibbonPanel curPanel = a.CreateRibbonPanel("Test Tab", "Test Panel");
            RibbonPanel curPanel = CreateRibbonPanel(a, "JRR Custom Tools", "Test Panel");

            //Step 3: Create button data instances
            PushButtonData pData1 = new PushButtonData("button1", "Button 1",  GetAssemblyName(), "RevitAddinAcademy02.Command");
            PushButtonData pData2 = new PushButtonData("button2", "Button 2",  GetAssemblyName(), "RevitAddinAcademy02.Command");

            SplitButtonData sData1 = new SplitButtonData("splitButton1", "Split Button 1");
            PulldownButtonData pbData = new PulldownButtonData("pulldownButton1", "Pulldown /rButton1");

            //Step 4: Add images
            pData1.Image = 

            //Step 5: add tooltip info

            //step 6: create buttons
            PushButton b1 curPanel.AddItem(pData1) as PushButton;
            PushButton b2 curPanel.AddItem(pData2) as PushButton;
            PushButton b3 curPanel.AddItem(pData3) as PushButton;
            PushButton b4 curPanel.AddItem(pData4) as PushButton;

            return Result.Succeeded;
        }

        private RibbonPanel CreateRibbonPanel(UIControlledApplication a, string tabName, string panelName)
        {
            foreach(RibbonPanel tmpPanel in a.GetRibbonPanels(tabName))
            {
                if(tmpPanel.Name == panelName)   //Looks for existing ribbon panel
                    return tmpPanel;
            }
            RibbonPanel returnPanel = a.CreateRibbonPanel(tabName, panelName);
        }

        private string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().Location;    
        }

        private BitmapImage BitmapToImageSource(System.Drawing.Bitmap bm)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                bm.Save(mem.System.Drawing.Imaging.ImageFormat.Png);
                mem.Position = 0;
                BitmapToImage bmi = new BitmapToImage();
                bmi.BeginInit();
                bmi.StreamSource = mem;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();

                return bmi;
            }
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
          }
    }
}
