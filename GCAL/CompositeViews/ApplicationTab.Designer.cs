namespace GCAL.CompositeViews
{
    partial class ApplicationTab
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
            this.verticalLineView1 = new GCAL.Views.VerticalLineView();
            this.gvListBanner1 = new GCAL.Views.GVListBanner();
            this.gvControlContainer1 = new GCAL.Views.GVControlContainer();
            this.SuspendLayout();
            // 
            // verticalLineView1
            // 
            this.verticalLineView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.verticalLineView1.Location = new System.Drawing.Point(177, 0);
            this.verticalLineView1.Name = "verticalLineView1";
            this.verticalLineView1.Size = new System.Drawing.Size(10, 526);
            this.verticalLineView1.TabIndex = 1;
            // 
            // gvListBanner1
            // 
            this.gvListBanner1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gvListBanner1.Controller = null;
            this.gvListBanner1.Location = new System.Drawing.Point(3, 0);
            this.gvListBanner1.Name = "gvListBanner1";
            this.gvListBanner1.SelectedIndex = -1;
            this.gvListBanner1.SelectedIndexNoResponse = -1;
            this.gvListBanner1.Size = new System.Drawing.Size(174, 529);
            this.gvListBanner1.TabIndex = 0;
            // 
            // gvControlContainer1
            // 
            this.gvControlContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvControlContainer1.Location = new System.Drawing.Point(193, 3);
            this.gvControlContainer1.Name = "gvControlContainer1";
            this.gvControlContainer1.Size = new System.Drawing.Size(481, 523);
            this.gvControlContainer1.TabIndex = 3;
            // 
            // ApplicationTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvControlContainer1);
            this.Controls.Add(this.verticalLineView1);
            this.Controls.Add(this.gvListBanner1);
            this.Name = "ApplicationTab";
            this.Size = new System.Drawing.Size(677, 529);
            this.ResumeLayout(false);

        }

        #endregion

        private Views.GVListBanner gvListBanner1;
        private Views.VerticalLineView verticalLineView1;
        private Views.GVControlContainer gvControlContainer1;

    }
}
