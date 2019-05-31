namespace GCAL
{
    partial class DlgSelectMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgSelectMap));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.listBoxMaps = new System.Windows.Forms.ListBox();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.buttonSetAnchorPoint = new System.Windows.Forms.Button();
            this.textBoxMapTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelImage = new System.Windows.Forms.Panel();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonImageFromClipboard = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonDelete);
            this.splitContainer1.Panel1.Controls.Add(this.buttonNew);
            this.splitContainer1.Panel1.Controls.Add(this.listBoxMaps);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonImageFromClipboard);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSelect);
            this.splitContainer1.Panel2.Controls.Add(this.buttonLoadImage);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSetAnchorPoint);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxMapTitle);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.panelImage);
            this.splitContainer1.Size = new System.Drawing.Size(978, 585);
            this.splitContainer1.SplitterDistance = 326;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(100, 512);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(95, 48);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNew.Location = new System.Drawing.Point(12, 512);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(82, 48);
            this.buttonNew.TabIndex = 1;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // listBoxMaps
            // 
            this.listBoxMaps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMaps.FormattingEnabled = true;
            this.listBoxMaps.IntegralHeight = false;
            this.listBoxMaps.ItemHeight = 20;
            this.listBoxMaps.Location = new System.Drawing.Point(12, 12);
            this.listBoxMaps.Name = "listBoxMaps";
            this.listBoxMaps.Size = new System.Drawing.Size(311, 482);
            this.listBoxMaps.TabIndex = 0;
            this.listBoxMaps.SelectedIndexChanged += new System.EventHandler(this.listBoxMaps_SelectedIndexChanged);
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLoadImage.Location = new System.Drawing.Point(18, 437);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(116, 40);
            this.buttonLoadImage.TabIndex = 4;
            this.buttonLoadImage.Text = "Load Image";
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // buttonSetAnchorPoint
            // 
            this.buttonSetAnchorPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSetAnchorPoint.Location = new System.Drawing.Point(305, 437);
            this.buttonSetAnchorPoint.Name = "buttonSetAnchorPoint";
            this.buttonSetAnchorPoint.Size = new System.Drawing.Size(172, 40);
            this.buttonSetAnchorPoint.TabIndex = 3;
            this.buttonSetAnchorPoint.Text = "Set Anchor Point";
            this.buttonSetAnchorPoint.UseVisualStyleBackColor = true;
            this.buttonSetAnchorPoint.Click += new System.EventHandler(this.buttonSetAnchorPoint_Click);
            // 
            // textBoxMapTitle
            // 
            this.textBoxMapTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxMapTitle.Location = new System.Drawing.Point(18, 534);
            this.textBoxMapTitle.Name = "textBoxMapTitle";
            this.textBoxMapTitle.Size = new System.Drawing.Size(261, 26);
            this.textBoxMapTitle.TabIndex = 2;
            this.textBoxMapTitle.TextChanged += new System.EventHandler(this.textBoxMapTitle_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 512);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Title";
            // 
            // panelImage
            // 
            this.panelImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelImage.Location = new System.Drawing.Point(12, 12);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(624, 419);
            this.panelImage.TabIndex = 0;
            this.panelImage.SizeChanged += new System.EventHandler(this.panelImage_SizeChanged);
            this.panelImage.Paint += new System.Windows.Forms.PaintEventHandler(this.panelImage_Paint);
            this.panelImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelImage_MouseDown);
            this.panelImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelImage_MouseMove);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSelect.Location = new System.Drawing.Point(542, 512);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(94, 50);
            this.buttonSelect.TabIndex = 5;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(430, 512);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 48);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // buttonImageFromClipboard
            // 
            this.buttonImageFromClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonImageFromClipboard.Location = new System.Drawing.Point(140, 437);
            this.buttonImageFromClipboard.Name = "buttonImageFromClipboard";
            this.buttonImageFromClipboard.Size = new System.Drawing.Size(159, 40);
            this.buttonImageFromClipboard.TabIndex = 7;
            this.buttonImageFromClipboard.Text = "From Clipboard";
            this.buttonImageFromClipboard.UseVisualStyleBackColor = true;
            this.buttonImageFromClipboard.Click += new System.EventHandler(this.buttonImageFromClipboard_Click);
            // 
            // DlgSelectMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 585);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSelectMap";
            this.Text = "Selection of the Map";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBoxMaps;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.Button buttonSetAnchorPoint;
        private System.Windows.Forms.TextBox textBoxMapTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonImageFromClipboard;
        private System.Windows.Forms.Button button1;
    }
}