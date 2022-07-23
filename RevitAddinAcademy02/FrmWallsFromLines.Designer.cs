namespace RevitAddinAcademy02
{
    partial class FrmWallsFromLines
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chbxStructural = new System.Windows.Forms.CheckBox();
            this.cbxLineStyles = new System.Windows.Forms.ComboBox();
            this.cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelWallType = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbxWallHeight = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.cbxWallTypes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // chbxStructural
            // 
            this.chbxStructural.AutoSize = true;
            this.chbxStructural.Location = new System.Drawing.Point(12, 196);
            this.chbxStructural.Name = "chbxStructural";
            this.chbxStructural.Size = new System.Drawing.Size(125, 17);
            this.chbxStructural.TabIndex = 0;
            this.chbxStructural.Text = "Make Wall Structural";
            this.chbxStructural.UseVisualStyleBackColor = true;
            this.chbxStructural.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cbxLineStyles
            // 
            this.cbxLineStyles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxLineStyles.FormattingEnabled = true;
            this.cbxLineStyles.Location = new System.Drawing.Point(12, 39);
            this.cbxLineStyles.Name = "cbxLineStyles";
            this.cbxLineStyles.Size = new System.Drawing.Size(523, 21);
            this.cbxLineStyles.TabIndex = 1;
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(460, 250);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 30);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select Line Styles:";
            // 
            // labelWallType
            // 
            this.labelWallType.AutoSize = true;
            this.labelWallType.Location = new System.Drawing.Point(12, 74);
            this.labelWallType.Name = "labelWallType";
            this.labelWallType.Size = new System.Drawing.Size(91, 13);
            this.labelWallType.TabIndex = 5;
            this.labelWallType.Text = "Select Wall Type:";
            this.labelWallType.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Enter Wall Height:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtbxWallHeight
            // 
            this.txtbxWallHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbxWallHeight.Location = new System.Drawing.Point(12, 149);
            this.txtbxWallHeight.Name = "txtbxWallHeight";
            this.txtbxWallHeight.Size = new System.Drawing.Size(523, 20);
            this.txtbxWallHeight.TabIndex = 6;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(379, 250);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 30);
            this.ok.TabIndex = 8;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbxWallTypes
            // 
            this.cbxWallTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxWallTypes.FormattingEnabled = true;
            this.cbxWallTypes.Location = new System.Drawing.Point(12, 91);
            this.cbxWallTypes.Name = "cbxWallTypes";
            this.cbxWallTypes.Size = new System.Drawing.Size(523, 21);
            this.cbxWallTypes.TabIndex = 9;
            // 
            // FrmWallsFromLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 292);
            this.Controls.Add(this.cbxWallTypes);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtbxWallHeight);
            this.Controls.Add(this.labelWallType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.cbxLineStyles);
            this.Controls.Add(this.chbxStructural);
            this.Name = "FrmWallsFromLines";
            this.Text = "Walls from Lines";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chbxStructural;
        private System.Windows.Forms.ComboBox cbxLineStyles;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelWallType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbxWallHeight;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.ComboBox cbxWallTypes;
    }
}