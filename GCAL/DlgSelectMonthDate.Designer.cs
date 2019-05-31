namespace GCAL
{
    partial class DlgSelectMonthDate
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
            this.selectCalendarMonthControl1 = new GCAL.CompositeViews.SelectCalendarMonthControl();
            this.SuspendLayout();
            // 
            // selectCalendarMonthControl1
            // 
            this.selectCalendarMonthControl1.BackColor = System.Drawing.SystemColors.Window;
            this.selectCalendarMonthControl1.Location = new System.Drawing.Point(0, 1);
            this.selectCalendarMonthControl1.Name = "selectCalendarMonthControl1";
            this.selectCalendarMonthControl1.Size = new System.Drawing.Size(321, 287);
            this.selectCalendarMonthControl1.TabIndex = 0;
            this.selectCalendarMonthControl1.MonthSelected += new GCAL.CompositeViews.OnMonthSelectedDelegate(this.selectCalendarMonthControl1_MonthSelected);
            // 
            // DlgSelectMonthDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 292);
            this.Controls.Add(this.selectCalendarMonthControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSelectMonthDate";
            this.Text = "Select Month Date";
            this.ResumeLayout(false);

        }

        #endregion

        private CompositeViews.SelectCalendarMonthControl selectCalendarMonthControl1;
    }
}