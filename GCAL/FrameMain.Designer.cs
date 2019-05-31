namespace GCAL
{
    partial class FrameMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameMain));
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDocumentTable = new System.Drawing.Printing.PrintDocument();
            this.gvTabBanner1 = new GCAL.Views.GVTabBanner();
            this.SuspendLayout();
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // gvTabBanner1
            // 
            this.gvTabBanner1.Controller = null;
            this.gvTabBanner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvTabBanner1.Location = new System.Drawing.Point(0, 0);
            this.gvTabBanner1.Name = "gvTabBanner1";
            this.gvTabBanner1.Size = new System.Drawing.Size(650, 530);
            this.gvTabBanner1.TabIndex = 7;
            // 
            // FrameMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 530);
            this.Controls.Add(this.gvTabBanner1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrameMain";
            this.Text = "GCAL";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Drawing.Printing.PrintDocument printDocumentTable;
        private Views.GVTabBanner gvTabBanner1;
    }
}

