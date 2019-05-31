namespace GCAL.Views
{
    partial class GVTabBanner
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
            this.gvControlContainer1 = new GCAL.Views.GVControlContainer();
            this.SuspendLayout();
            // 
            // gvControlContainer1
            // 
            this.gvControlContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvControlContainer1.Controller = null;
            this.gvControlContainer1.Location = new System.Drawing.Point(3, 105);
            this.gvControlContainer1.Name = "gvControlContainer1";
            this.gvControlContainer1.Size = new System.Drawing.Size(615, 323);
            this.gvControlContainer1.TabIndex = 0;
            // 
            // GVTabBanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvControlContainer1);
            this.DoubleBuffered = true;
            this.Name = "GVTabBanner";
            this.Size = new System.Drawing.Size(621, 431);
            this.SizeChanged += new System.EventHandler(this.GVTabBanner_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GVTabBanner_MouseDown);
            this.MouseLeave += new System.EventHandler(this.GVTabBanner_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GVTabBanner_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GVTabBanner_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private GVControlContainer gvControlContainer1;


    }
}
