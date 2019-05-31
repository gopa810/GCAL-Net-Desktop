namespace GCAL.CompositeViews
{
    partial class DateTimePanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDay = new System.Windows.Forms.ComboBox();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTimezoneName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nddYear = new System.Windows.Forms.NumericUpDown();
            this.nddHour = new System.Windows.Forms.NumericUpDown();
            this.nddMinute = new System.Windows.Forms.NumericUpDown();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nddYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nddHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nddMinute)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Day";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Month";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(263, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Year";
            // 
            // cbDay
            // 
            this.cbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDay.FormattingEnabled = true;
            this.cbDay.Location = new System.Drawing.Point(58, 75);
            this.cbDay.Name = "cbDay";
            this.cbDay.Size = new System.Drawing.Size(55, 21);
            this.cbDay.TabIndex = 3;
            // 
            // cbMonth
            // 
            this.cbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Location = new System.Drawing.Point(119, 75);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(121, 21);
            this.cbMonth.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(215, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Hour";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(272, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Minute";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(98, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Timezone:";
            // 
            // labelTimezoneName
            // 
            this.labelTimezoneName.AutoEllipsis = true;
            this.labelTimezoneName.Location = new System.Drawing.Point(160, 174);
            this.labelTimezoneName.Name = "labelTimezoneName";
            this.labelTimezoneName.Size = new System.Drawing.Size(155, 16);
            this.labelTimezoneName.TabIndex = 11;
            this.labelTimezoneName.Text = "static";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Daylight Saving Time:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(160, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "N/A";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(114, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "24h Time Format:";
            // 
            // nddYear
            // 
            this.nddYear.Location = new System.Drawing.Point(246, 76);
            this.nddYear.Maximum = new decimal(new int[] {
            3999,
            0,
            0,
            0});
            this.nddYear.Minimum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.nddYear.Name = "nddYear";
            this.nddYear.Size = new System.Drawing.Size(71, 20);
            this.nddYear.TabIndex = 15;
            this.nddYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nddYear.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            // 
            // nddHour
            // 
            this.nddHour.Location = new System.Drawing.Point(209, 134);
            this.nddHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nddHour.Name = "nddHour";
            this.nddHour.Size = new System.Drawing.Size(53, 20);
            this.nddHour.TabIndex = 16;
            this.nddHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nddMinute
            // 
            this.nddMinute.Location = new System.Drawing.Point(268, 134);
            this.nddMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nddMinute.Name = "nddMinute";
            this.nddMinute.Size = new System.Drawing.Size(49, 20);
            this.nddMinute.TabIndex = 17;
            this.nddMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(265, 239);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 32);
            this.buttonCancel.TabIndex = 27;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(184, 239);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 32);
            this.buttonOK.TabIndex = 26;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(12, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(328, 29);
            this.label10.TabIndex = 28;
            this.label10.Text = "Date Time";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DateTimePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.nddMinute);
            this.Controls.Add(this.nddHour);
            this.Controls.Add(this.nddYear);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelTimezoneName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbMonth);
            this.Controls.Add(this.cbDay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(392, 320);
            this.MinimumSize = new System.Drawing.Size(292, 166);
            this.Name = "DateTimePanel";
            this.Size = new System.Drawing.Size(355, 287);
            ((System.ComponentModel.ISupportInitialize)(this.nddYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nddHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nddMinute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDay;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelTimezoneName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nddYear;
        private System.Windows.Forms.NumericUpDown nddHour;
        private System.Windows.Forms.NumericUpDown nddMinute;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label10;
    }
}
