﻿namespace GCAL.CalendarDataView
{
    partial class CalendarDataView
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
            this.SuspendLayout();
            // 
            // CalendarDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CalendarDataView";
            this.Size = new System.Drawing.Size(762, 788);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CalendarDataView_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CalendarDataView_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
