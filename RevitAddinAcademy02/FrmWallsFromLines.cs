using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAddinAcademy02
{
    public partial class FrmWallsFromLines : Form
    {
        public FrmWallsFromLines(List<string> wallTypes, List<string> lineStyles)
        {
            InitializeComponent();

            foreach(string wallType in wallTypes)           //Populate combo box with wall types
            {
                this.cbxWallTypes.Items.Add(wallType);
            }

            foreach(string lineStyle in lineStyles)         //Populate combo box with line styles
            {
                this.cbxLineStyles.Items.Add(lineStyle);
            }

            this.cbxWallTypes.SelectedIndex = 0;            //Displays first item in wall types
            this.cbxLineStyles.SelectedIndex = 0;           //Displays first item in line styles
        }

        
        public string GetSelectedWallType()
        { 
            return cbxWallTypes.SelectedItem.ToString();
        }

        public string GetSelectedLineStyle()
        {
            return cbxLineStyles.SelectedItem.ToString();
        }

        public double GetWallHeight()
        {
            double returnValue;
            if(double.TryParse(txtbxWallHeight.Text, out returnValue) == true)   //TryParse only returns a value if it can parse to a double
            {
                return returnValue;
            }
            return 20;
        }

        public bool AreWallStructural()
        {
            return chbxStructural.Checked;
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)   //This is considered an event
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
