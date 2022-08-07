#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;                                //Allows C# to know things about itself
using System.Windows.Media.Imaging;                             //Add using statements from Presentation namespaces
using System.IO;
#endregion

namespace RevitAddinAcademy02
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            //Step 1: Create Ribbon Tab
            try
            {
                app.CreateRibbonTab("Revit Add-in Academy");
            }
            catch (Exception)
            {
                Debug.Print("Tab already exists");
            }

            //Step 2: Create Ribbon Panels
            RibbonPanel pushButtonPanel = CreateRibbonPanel(app, "Revit Add-in Academy", "Push Buttons");
            RibbonPanel stackedButtonPanel = CreateRibbonPanel(app, "Revit Add-in Academy", "Stacked Buttons");
            RibbonPanel splitButtonPanel = CreateRibbonPanel(app, "Revit Add-in Academy", "Split Buttons");
            RibbonPanel pulldownPanel = CreateRibbonPanel(app, "Revit Add-in Academy", "Pulldown Buttons");

            //Step 3: Create button data instances
            PushButtonData pData1 = new PushButtonData("button1", "Fill Rooms \nwith Furniture",  GetAssemblyName(), "RevitAddinAcademy02.CmdFillRoomsWFurniture");
            PushButtonData pData2 = new PushButtonData("button2", "Project Setup",  GetAssemblyName(), "RevitAddinAcademy02.cmdProjectSetup");
            PushButtonData pData3 = new PushButtonData("button3", "Button 3", GetAssemblyName(), "RevitAddinAcademy02.Command");
            PushButtonData pData4 = new PushButtonData("button4", "Button 4", GetAssemblyName(), "RevitAddinAcademy02.Command");
            PushButtonData pData5 = new PushButtonData("button5", "Button 5", GetAssemblyName(), "RevitAddinAcademy02.Command");
            PushButtonData pData6 = new PushButtonData("button6", "Button 6", GetAssemblyName(), "RevitAddinAcademy02.Command");
            PushButtonData pData7 = new PushButtonData("button7", "Button 7", GetAssemblyName(), "RevitAddinAcademy02.Command");
            PushButtonData pData8 = new PushButtonData("button8", "Button 8", GetAssemblyName(), "RevitAddinAcademy02.Command");
            PushButtonData pData9 = new PushButtonData("button9", "Button 9", GetAssemblyName(), "RevitAddinAcademy02.Command");
            PushButtonData pData10 = new PushButtonData("button10", "Button 10", GetAssemblyName(), "RevitAddinAcademy02.Command");
           
            SplitButtonData sbData1 = new SplitButtonData("splitButton1", "Split Button 1");
            PulldownButtonData pdData1 = new PulldownButtonData("pulldownButton1", "Pulldown \nButton1");


            //Step 4: Add images    Images need to be 96dpi, one at 16x16 and another at 32x32. Add images through Properties:Resources.  Image properties should be set to 'Embedded in .resx to set as part of .dll file     
            pData1.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Red_16);                                //Image needs to be converted to an image source          
            pData1.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Red_32);  
            
            pData2.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Blue_16);                     
            pData2.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Blue_32);

            pData3.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Green_16);                                      
            pData3.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Green_32);

            pData4.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Yellow_16);
            pData4.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Yellow_32);

            pData5.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Red_16);
            pData5.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Red_32);

            pData6.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Blue_16);
            pData6.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Blue_32);

            pData7.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Green_16);
            pData7.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Green_32);

            pData8.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Yellow_16);
            pData8.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Yellow_32);

            pData9.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Red_16);
            pData9.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Red_32);

            pData10.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Blue_16);
            pData10.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Blue_32);

            pdData1.Image = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Yellow_16);                                 
            pdData1.LargeImage = BitmapToImageSource(RevitAddinAcademy02.Properties.Resources.Yellow_32);

            //Step 5: add tooltip info
            pData1.ToolTip = "Button 1 tool tip";
            pData2.ToolTip = "Button 2 tool tip";
            pData3.ToolTip = "Button 3 tool tip";
            pData4.ToolTip = "Button 4 tool tip";
            pData5.ToolTip = "Button 5 tool tip";
            pData6.ToolTip = "Button 6 tool tip";
            pData7.ToolTip = "Button 7 tool tip";
            pData8.ToolTip = "Button 8 tool tip";
            pData9.ToolTip = "Button 9 tool tip";
            pData10.ToolTip = "Button 10 tool tip";


            //step 6: create buttons
            PushButton pButton1 = pushButtonPanel.AddItem(pData1) as PushButton;
            PushButton pButton2 = pushButtonPanel.AddItem(pData2) as PushButton;

            stackedButtonPanel.AddStackedItems(pData3, pData4, pData5);

            SplitButton splitButton1 = splitButtonPanel.AddItem(sbData1) as SplitButton;
            splitButton1.AddPushButton(pData6);
            splitButton1.AddPushButton(pData7);

            PulldownButton pulldownButton1 = pulldownPanel.AddItem(pdData1) as PulldownButton;
            pulldownButton1.AddPushButton(pData8);
            pulldownButton1.AddPushButton(pData9);
            pulldownButton1.AddPushButton(pData10);

            return Result.Succeeded;
        }

        private RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            foreach(RibbonPanel curPanel in app.GetRibbonPanels(tabName))
            {
                if(curPanel.Name == panelName)  //Looks for existing ribbon panel
                    return curPanel;
            }
            RibbonPanel returnPanel = app.CreateRibbonPanel(tabName, panelName);
            return returnPanel;
        }

        private string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().Location;    
        }

        private BitmapImage BitmapToImageSource(System.Drawing.Bitmap bm)
        {
            using (MemoryStream mem = new MemoryStream())                   //Allocation of Memory
            {
                bm.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                mem.Position = 0;
                BitmapImage bmi = new BitmapImage();
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
