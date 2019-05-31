namespace GCAL.CompositeViews
{
    partial class TimezoneDetails
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.offsetDST = new GCAL.CompositeViews.HourMinutesPanel();
            this.dstEndPanel = new GCAL.CompositeViews.DateSpecifyDstPanel();
            this.dstStartPanel = new GCAL.CompositeViews.DateSpecifyDstPanel();
            this.offsetNoDST = new GCAL.CompositeViews.HourMinutesPanel();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(22, 13);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(68, 35);
            this.buttonCancel.TabIndex = 14;
            this.buttonCancel.Text = "< Back";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Location = new System.Drawing.Point(238, 565);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 39);
            this.buttonSave.TabIndex = 13;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.AutoScroll = true;
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Location = new System.Drawing.Point(18, 156);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(295, 403);
            this.panel3.TabIndex = 12;
            this.panel3.SizeChanged += new System.EventHandler(this.panel1_SizeChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.offsetDST);
            this.panel2.Controls.Add(this.dstEndPanel);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.dstStartPanel);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(253, 387);
            this.panel2.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Silver;
            this.label7.Location = new System.Drawing.Point(5, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "End of DST";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Silver;
            this.label6.Location = new System.Drawing.Point(2, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(206, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Start of DST";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(58, 55);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(255, 20);
            this.textBox1.TabIndex = 11;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(22, 133);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(188, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Uses Daylight Saving Time system";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(96, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Timezone Details";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // offsetDST
            // 
            this.offsetDST.Location = new System.Drawing.Point(8, 3);
            this.offsetDST.Name = "offsetDST";
            this.offsetDST.NegativeOffsets = true;
            this.offsetDST.Size = new System.Drawing.Size(200, 49);
            this.offsetDST.TabIndex = 4;
            this.offsetDST.Title = "Offset to UTC for DST active";
            this.offsetDST.ValueMinutes = 0;
            // 
            // dstEndPanel
            // 
            this.dstEndPanel.Location = new System.Drawing.Point(3, 256);
            this.dstEndPanel.Name = "dstEndPanel";
            this.dstEndPanel.Size = new System.Drawing.Size(206, 141);
            this.dstEndPanel.TabIndex = 3;
            this.dstEndPanel.Value = new Base.TTimeZoneDst();
            // 
            // dstStartPanel
            // 
            this.dstStartPanel.Location = new System.Drawing.Point(2, 81);
            this.dstStartPanel.Name = "dstStartPanel";
            this.dstStartPanel.Size = new System.Drawing.Size(206, 141);
            this.dstStartPanel.TabIndex = 1;
            this.dstStartPanel.Value = new Base.TTimeZoneDst();
            // 
            // offsetNoDST
            // 
            this.offsetNoDST.Location = new System.Drawing.Point(18, 81);
            this.offsetNoDST.Name = "offsetNoDST";
            this.offsetNoDST.NegativeOffsets = true;
            this.offsetNoDST.Size = new System.Drawing.Size(179, 49);
            this.offsetNoDST.TabIndex = 10;
            this.offsetNoDST.Title = "Offset to UTC";
            this.offsetNoDST.ValueMinutes = 0;
            // 
            // TimezoneDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.offsetNoDST);
            this.Name = "TimezoneDetails";
            this.Size = new System.Drawing.Size(331, 621);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private HourMinutesPanel offsetNoDST;
        private System.Windows.Forms.Panel panel2;
        private HourMinutesPanel offsetDST;
        private DateSpecifyDstPanel dstEndPanel;
        private System.Windows.Forms.Label label7;
        private DateSpecifyDstPanel dstStartPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panel3;
    }
}
