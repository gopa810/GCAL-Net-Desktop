namespace GCAL.CompositeViews
{
    partial class DateSpecifyDstPanel
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
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxDayOfWeek = new System.Windows.Forms.ComboBox();
            this.comboBoxWeekOfMonth = new System.Windows.Forms.ComboBox();
            this.comboBoxMonth = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxDayOfMonth = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type";
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "DayOfWeek / Week / Month",
            "DayOfMonth / Month"});
            this.comboBoxType.Location = new System.Drawing.Point(40, 0);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(157, 21);
            this.comboBoxType.TabIndex = 1;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Day of Week";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Week of Month";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Month";
            // 
            // comboBoxDayOfWeek
            // 
            this.comboBoxDayOfWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDayOfWeek.FormattingEnabled = true;
            this.comboBoxDayOfWeek.Location = new System.Drawing.Point(79, 28);
            this.comboBoxDayOfWeek.Name = "comboBoxDayOfWeek";
            this.comboBoxDayOfWeek.Size = new System.Drawing.Size(118, 21);
            this.comboBoxDayOfWeek.TabIndex = 5;
            // 
            // comboBoxWeekOfMonth
            // 
            this.comboBoxWeekOfMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWeekOfMonth.FormattingEnabled = true;
            this.comboBoxWeekOfMonth.Items.AddRange(new object[] {
            "First week",
            "Second week",
            "Third week",
            "Fourth week",
            "Last week"});
            this.comboBoxWeekOfMonth.Location = new System.Drawing.Point(90, 55);
            this.comboBoxWeekOfMonth.Name = "comboBoxWeekOfMonth";
            this.comboBoxWeekOfMonth.Size = new System.Drawing.Size(107, 21);
            this.comboBoxWeekOfMonth.TabIndex = 6;
            // 
            // comboBoxMonth
            // 
            this.comboBoxMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonth.FormattingEnabled = true;
            this.comboBoxMonth.Location = new System.Drawing.Point(46, 109);
            this.comboBoxMonth.Name = "comboBoxMonth";
            this.comboBoxMonth.Size = new System.Drawing.Size(151, 21);
            this.comboBoxMonth.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Day Of Month";
            // 
            // comboBoxDayOfMonth
            // 
            this.comboBoxDayOfMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDayOfMonth.FormattingEnabled = true;
            this.comboBoxDayOfMonth.Location = new System.Drawing.Point(82, 82);
            this.comboBoxDayOfMonth.Name = "comboBoxDayOfMonth";
            this.comboBoxDayOfMonth.Size = new System.Drawing.Size(115, 21);
            this.comboBoxDayOfMonth.TabIndex = 9;
            // 
            // DateSpecifyDstPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxDayOfMonth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxMonth);
            this.Controls.Add(this.comboBoxWeekOfMonth);
            this.Controls.Add(this.comboBoxDayOfWeek);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.label1);
            this.Name = "DateSpecifyDstPanel";
            this.Size = new System.Drawing.Size(206, 141);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxDayOfWeek;
        private System.Windows.Forms.ComboBox comboBoxWeekOfMonth;
        private System.Windows.Forms.ComboBox comboBoxMonth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxDayOfMonth;
    }
}
