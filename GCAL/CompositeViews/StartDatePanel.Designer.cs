namespace GCAL.CompositeViews
{
    partial class StartDatePanel
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDay = new System.Windows.Forms.ComboBox();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.cbTithi = new System.Windows.Forms.ComboBox();
            this.cbMasa = new System.Windows.Forms.ComboBox();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.tbGaurabdaYear = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkStartGaurabda = new System.Windows.Forms.LinkLabel();
            this.linkStartMasa = new System.Windows.Forms.LinkLabel();
            this.linkYearMinus = new System.Windows.Forms.LinkLabel();
            this.linkYearPlus = new System.Windows.Forms.LinkLabel();
            this.linkStartMonth = new System.Windows.Forms.LinkLabel();
            this.linkStartYear = new System.Windows.Forms.LinkLabel();
            this.labelNote = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Day";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Month";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Year";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tithi";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(101, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Masa";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Gaurabda Year";
            // 
            // cbDay
            // 
            this.cbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDay.FormattingEnabled = true;
            this.cbDay.Location = new System.Drawing.Point(10, 93);
            this.cbDay.Name = "cbDay";
            this.cbDay.Size = new System.Drawing.Size(88, 21);
            this.cbDay.TabIndex = 6;
            this.cbDay.SelectedIndexChanged += new System.EventHandler(this.cbDay_SelectedIndexChanged);
            // 
            // cbMonth
            // 
            this.cbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Location = new System.Drawing.Point(104, 93);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(115, 21);
            this.cbMonth.TabIndex = 7;
            this.cbMonth.SelectedIndexChanged += new System.EventHandler(this.cbMonth_SelectedIndexChanged);
            // 
            // cbTithi
            // 
            this.cbTithi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTithi.FormattingEnabled = true;
            this.cbTithi.Location = new System.Drawing.Point(10, 157);
            this.cbTithi.Name = "cbTithi";
            this.cbTithi.Size = new System.Drawing.Size(88, 21);
            this.cbTithi.TabIndex = 8;
            this.cbTithi.SelectedIndexChanged += new System.EventHandler(this.cbTithi_SelectedIndexChanged);
            // 
            // cbMasa
            // 
            this.cbMasa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMasa.FormattingEnabled = true;
            this.cbMasa.Location = new System.Drawing.Point(104, 157);
            this.cbMasa.Name = "cbMasa";
            this.cbMasa.Size = new System.Drawing.Size(115, 21);
            this.cbMasa.TabIndex = 9;
            this.cbMasa.SelectedIndexChanged += new System.EventHandler(this.cbMasa_SelectedIndexChanged);
            // 
            // tbYear
            // 
            this.tbYear.Location = new System.Drawing.Point(225, 94);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(57, 20);
            this.tbYear.TabIndex = 10;
            this.tbYear.TextChanged += new System.EventHandler(this.tbYear_TextChanged);
            // 
            // tbGaurabdaYear
            // 
            this.tbGaurabdaYear.Location = new System.Drawing.Point(225, 158);
            this.tbGaurabdaYear.Name = "tbGaurabdaYear";
            this.tbGaurabdaYear.Size = new System.Drawing.Size(57, 20);
            this.tbGaurabdaYear.TabIndex = 11;
            this.tbGaurabdaYear.TextChanged += new System.EventHandler(this.tbGaurabdaYear_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkStartGaurabda);
            this.groupBox1.Controls.Add(this.linkStartMasa);
            this.groupBox1.Controls.Add(this.linkYearMinus);
            this.groupBox1.Controls.Add(this.linkYearPlus);
            this.groupBox1.Controls.Add(this.linkStartMonth);
            this.groupBox1.Controls.Add(this.linkStartYear);
            this.groupBox1.Location = new System.Drawing.Point(310, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(103, 143);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shortcuts";
            // 
            // linkStartGaurabda
            // 
            this.linkStartGaurabda.AutoSize = true;
            this.linkStartGaurabda.Location = new System.Drawing.Point(7, 106);
            this.linkStartGaurabda.Name = "linkStartGaurabda";
            this.linkStartGaurabda.Size = new System.Drawing.Size(91, 13);
            this.linkStartGaurabda.TabIndex = 5;
            this.linkStartGaurabda.TabStop = true;
            this.linkStartGaurabda.Text = "Start of Gaurabda";
            this.linkStartGaurabda.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkStartGaurabda_LinkClicked);
            // 
            // linkStartMasa
            // 
            this.linkStartMasa.AutoSize = true;
            this.linkStartMasa.Location = new System.Drawing.Point(7, 85);
            this.linkStartMasa.Name = "linkStartMasa";
            this.linkStartMasa.Size = new System.Drawing.Size(70, 13);
            this.linkStartMasa.TabIndex = 4;
            this.linkStartMasa.TabStop = true;
            this.linkStartMasa.Text = "Start of Masa";
            this.linkStartMasa.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkStartMasa_LinkClicked);
            // 
            // linkYearMinus
            // 
            this.linkYearMinus.AutoSize = true;
            this.linkYearMinus.Location = new System.Drawing.Point(50, 53);
            this.linkYearMinus.Name = "linkYearMinus";
            this.linkYearMinus.Size = new System.Drawing.Size(35, 13);
            this.linkYearMinus.TabIndex = 3;
            this.linkYearMinus.TabStop = true;
            this.linkYearMinus.Text = "Year -";
            this.linkYearMinus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkYearMinus_LinkClicked);
            // 
            // linkYearPlus
            // 
            this.linkYearPlus.AutoSize = true;
            this.linkYearPlus.Location = new System.Drawing.Point(6, 53);
            this.linkYearPlus.Name = "linkYearPlus";
            this.linkYearPlus.Size = new System.Drawing.Size(38, 13);
            this.linkYearPlus.TabIndex = 2;
            this.linkYearPlus.TabStop = true;
            this.linkYearPlus.Text = "Year +";
            this.linkYearPlus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkYearPlus_LinkClicked);
            // 
            // linkStartMonth
            // 
            this.linkStartMonth.AutoSize = true;
            this.linkStartMonth.Location = new System.Drawing.Point(6, 34);
            this.linkStartMonth.Name = "linkStartMonth";
            this.linkStartMonth.Size = new System.Drawing.Size(74, 13);
            this.linkStartMonth.TabIndex = 1;
            this.linkStartMonth.TabStop = true;
            this.linkStartMonth.Text = "Start of Month";
            this.linkStartMonth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkStartMonth_LinkClicked);
            // 
            // linkStartYear
            // 
            this.linkStartYear.AutoSize = true;
            this.linkStartYear.Location = new System.Drawing.Point(6, 16);
            this.linkStartYear.Name = "linkStartYear";
            this.linkStartYear.Size = new System.Drawing.Size(66, 13);
            this.linkStartYear.TabIndex = 0;
            this.linkStartYear.TabStop = true;
            this.linkStartYear.Text = "Start of Year";
            this.linkStartYear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkStartYear_LinkClicked);
            // 
            // labelNote
            // 
            this.labelNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNote.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelNote.Location = new System.Drawing.Point(7, 215);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(404, 23);
            this.labelNote.TabIndex = 13;
            this.labelNote.Text = "Choose day, month and year or tithi, masa and Gaurabda year.";
            this.labelNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(335, 252);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 32);
            this.buttonCancel.TabIndex = 25;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Visible = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(254, 252);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 32);
            this.buttonOK.TabIndex = 24;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Visible = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(403, 34);
            this.label7.TabIndex = 26;
            this.label7.Text = "Enter Start Date";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StartDatePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbGaurabdaYear);
            this.Controls.Add(this.tbYear);
            this.Controls.Add(this.cbMasa);
            this.Controls.Add(this.cbTithi);
            this.Controls.Add(this.cbMonth);
            this.Controls.Add(this.cbDay);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "StartDatePanel";
            this.Size = new System.Drawing.Size(423, 305);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbDay;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.ComboBox cbTithi;
        private System.Windows.Forms.ComboBox cbMasa;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.TextBox tbGaurabdaYear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel linkStartGaurabda;
        private System.Windows.Forms.LinkLabel linkStartMasa;
        private System.Windows.Forms.LinkLabel linkYearMinus;
        private System.Windows.Forms.LinkLabel linkYearPlus;
        private System.Windows.Forms.LinkLabel linkStartMonth;
        private System.Windows.Forms.LinkLabel linkStartYear;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label7;
    }
}
