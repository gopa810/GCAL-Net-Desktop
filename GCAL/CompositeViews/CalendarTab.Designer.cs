namespace GCAL.CompositeViews
{
    partial class CalendarTab
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
            GCAL.CalendarDataView.CDVDocument cdvDocument7 = new GCAL.CalendarDataView.CDVDocument();
            GCAL.CalendarDataView.CDVParaStyle cdvParaStyle7 = new GCAL.CalendarDataView.CDVParaStyle();
            GCAL.CalendarDataView.CDVTextStyle cdvTextStyle7 = new GCAL.CalendarDataView.CDVTextStyle();
            GCAL.CalendarDataView.CDVVisibilityStyle cdvVisibilityStyle7 = new GCAL.CalendarDataView.CDVVisibilityStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarTab));
            this.panel1 = new System.Windows.Forms.Panel();
            this.calendarDataView1 = new GCAL.CalendarDataView.CalendarDataView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.calendarTableView1 = new GCAL.Views.CalendarTableView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.plainTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.smallTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallTextToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.normalTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largestTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.printToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.printMultipleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.ekadasiMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCompleteDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDocumentTable = new System.Drawing.Printing.PrintDocument();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.columnYogaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnNaksatraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnRasiOfTheMoonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnSunriseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnNoonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnSunsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnCoreEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.calendarDataView1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.calendarTableView1);
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 529);
            this.panel1.TabIndex = 2;
            // 
            // calendarDataView1
            // 
            this.calendarDataView1.BackColor = System.Drawing.Color.White;
            this.calendarDataView1.DataSource = null;
            cdvDocument7.Bounds = new System.Drawing.Rectangle(0, 0, 0, 0);
            cdvDocument7.Location = new System.Drawing.Point(0, 0);
            cdvDocument7.ParaStyle = cdvParaStyle7;
            cdvDocument7.Parent = null;
            cdvDocument7.Size = new System.Drawing.Size(0, 0);
            cdvDocument7.SpanWidth = GCAL.CalendarDataView.CDVSpan.Minimum;
            cdvDocument7.TextStyle = cdvTextStyle7;
            cdvVisibilityStyle7.Visible = true;
            cdvDocument7.Visibility = cdvVisibilityStyle7;
            cdvDocument7.Width = 0;
            cdvDocument7.X = 0;
            this.calendarDataView1.Document = cdvDocument7;
            this.calendarDataView1.Location = new System.Drawing.Point(496, 255);
            this.calendarDataView1.MainAtom = null;
            this.calendarDataView1.MainAtomPosition = 0;
            this.calendarDataView1.Name = "calendarDataView1";
            this.calendarDataView1.Size = new System.Drawing.Size(286, 259);
            this.calendarDataView1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GCAL.Properties.Resources.ajax_loader;
            this.pictureBox1.Location = new System.Drawing.Point(146, 310);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(115, 114);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // calendarTableView1
            // 
            this.calendarTableView1.BackColor = System.Drawing.Color.White;
            this.calendarTableView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.calendarTableView1.CurrentMonth = 9;
            this.calendarTableView1.CurrentYear = 2016;
            this.calendarTableView1.LiveRefresh = false;
            this.calendarTableView1.Location = new System.Drawing.Point(215, 25);
            this.calendarTableView1.Name = "calendarTableView1";
            this.calendarTableView1.SelectedCalendar = null;
            this.calendarTableView1.Size = new System.Drawing.Size(218, 224);
            this.calendarTableView1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripSeparator3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripDropDownButton2,
            this.toolStripDropDownButton1,
            this.toolStripSplitButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripButton1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(58, 22);
            this.toolStripButton1.Text = "Mayapur";
            this.toolStripButton1.Click += new System.EventHandler(this.onLocationClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripButton2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton2.Text = "Select Date";
            this.toolStripButton2.Click += new System.EventHandler(this.onDateRangeClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(24, 22);
            this.toolStripButton4.Text = "◄";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripButton5.ForeColor = System.Drawing.Color.RoyalBlue;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(62, 22);
            this.toolStripButton5.Text = "Set Today";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(24, 22);
            this.toolStripButton6.Text = "►";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plainTextToolStripMenuItem,
            this.tableToolStripMenuItem,
            this.toolStripMenuItem1,
            this.smallTextToolStripMenuItem,
            this.smallTextToolStripMenuItem1,
            this.normalTextToolStripMenuItem,
            this.largeTextToolStripMenuItem,
            this.largestTextToolStripMenuItem,
            this.toolStripSeparator2,
            this.columnYogaToolStripMenuItem,
            this.columnNaksatraToolStripMenuItem,
            this.columnRasiOfTheMoonToolStripMenuItem,
            this.columnSunriseToolStripMenuItem,
            this.columnNoonToolStripMenuItem,
            this.columnSunsetToolStripMenuItem,
            this.columnCoreEventsToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton2.Text = "View";
            this.toolStripDropDownButton2.DropDownOpening += new System.EventHandler(this.toolStripDropDownButton2_DropDownOpening);
            // 
            // plainTextToolStripMenuItem
            // 
            this.plainTextToolStripMenuItem.Name = "plainTextToolStripMenuItem";
            this.plainTextToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.plainTextToolStripMenuItem.Text = "Text";
            this.plainTextToolStripMenuItem.Click += new System.EventHandler(this.setTextModeView_Click);
            // 
            // tableToolStripMenuItem
            // 
            this.tableToolStripMenuItem.Name = "tableToolStripMenuItem";
            this.tableToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.tableToolStripMenuItem.Text = "Table";
            this.tableToolStripMenuItem.Click += new System.EventHandler(this.tableToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 6);
            // 
            // smallTextToolStripMenuItem
            // 
            this.smallTextToolStripMenuItem.Name = "smallTextToolStripMenuItem";
            this.smallTextToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.smallTextToolStripMenuItem.Text = "Smallest Text";
            this.smallTextToolStripMenuItem.Click += new System.EventHandler(this.smallTextToolStripMenuItem_Click);
            // 
            // smallTextToolStripMenuItem1
            // 
            this.smallTextToolStripMenuItem1.Name = "smallTextToolStripMenuItem1";
            this.smallTextToolStripMenuItem1.Size = new System.Drawing.Size(210, 22);
            this.smallTextToolStripMenuItem1.Text = "Small Text";
            this.smallTextToolStripMenuItem1.Click += new System.EventHandler(this.smallTextToolStripMenuItem1_Click);
            // 
            // normalTextToolStripMenuItem
            // 
            this.normalTextToolStripMenuItem.Name = "normalTextToolStripMenuItem";
            this.normalTextToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.normalTextToolStripMenuItem.Text = "Normal Text";
            this.normalTextToolStripMenuItem.Click += new System.EventHandler(this.normalTextToolStripMenuItem_Click);
            // 
            // largeTextToolStripMenuItem
            // 
            this.largeTextToolStripMenuItem.Name = "largeTextToolStripMenuItem";
            this.largeTextToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.largeTextToolStripMenuItem.Text = "Large Text";
            this.largeTextToolStripMenuItem.Click += new System.EventHandler(this.largeTextToolStripMenuItem_Click);
            // 
            // largestTextToolStripMenuItem
            // 
            this.largestTextToolStripMenuItem.Name = "largestTextToolStripMenuItem";
            this.largestTextToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.largestTextToolStripMenuItem.Text = "Largest Text";
            this.largestTextToolStripMenuItem.Click += new System.EventHandler(this.largestTextToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printToolStripMenuItem1,
            this.printToolStripMenuItem2,
            this.printMultipleToolStripMenuItem});
            this.toolStripDropDownButton1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripDropDownButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(55, 22);
            this.toolStripDropDownButton1.Text = "Action";
            // 
            // printToolStripMenuItem1
            // 
            this.printToolStripMenuItem1.Name = "printToolStripMenuItem1";
            this.printToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.printToolStripMenuItem1.Text = "Export";
            this.printToolStripMenuItem1.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem2
            // 
            this.printToolStripMenuItem2.Name = "printToolStripMenuItem2";
            this.printToolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.printToolStripMenuItem2.Text = "Print";
            this.printToolStripMenuItem2.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // printMultipleToolStripMenuItem
            // 
            this.printMultipleToolStripMenuItem.Name = "printMultipleToolStripMenuItem";
            this.printMultipleToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.printMultipleToolStripMenuItem.Text = "Print Multiple";
            this.printMultipleToolStripMenuItem.Click += new System.EventHandler(this.printMultipleToolStripMenuItem_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ekadasiMapToolStripMenuItem,
            this.exportCompleteDataToolStripMenuItem});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(60, 22);
            this.toolStripSplitButton1.Text = "Special";
            // 
            // ekadasiMapToolStripMenuItem
            // 
            this.ekadasiMapToolStripMenuItem.Name = "ekadasiMapToolStripMenuItem";
            this.ekadasiMapToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.ekadasiMapToolStripMenuItem.Text = "Ekadasi Map";
            this.ekadasiMapToolStripMenuItem.Click += new System.EventHandler(this.ekadasiMapToolStripMenuItem_Click);
            // 
            // exportCompleteDataToolStripMenuItem
            // 
            this.exportCompleteDataToolStripMenuItem.Name = "exportCompleteDataToolStripMenuItem";
            this.exportCompleteDataToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.exportCompleteDataToolStripMenuItem.Text = "Export Complete Data";
            this.exportCompleteDataToolStripMenuItem.Click += new System.EventHandler(this.exportCompleteDataToolStripMenuItem_Click);
            // 
            // printDocumentTable
            // 
            this.printDocumentTable.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(207, 6);
            // 
            // columnYogaToolStripMenuItem
            // 
            this.columnYogaToolStripMenuItem.Name = "columnYogaToolStripMenuItem";
            this.columnYogaToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.columnYogaToolStripMenuItem.Text = "Column Yoga";
            this.columnYogaToolStripMenuItem.Click += new System.EventHandler(this.columnYogaToolStripMenuItem_Click);
            // 
            // columnNaksatraToolStripMenuItem
            // 
            this.columnNaksatraToolStripMenuItem.Name = "columnNaksatraToolStripMenuItem";
            this.columnNaksatraToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.columnNaksatraToolStripMenuItem.Text = "Column Naksatra";
            this.columnNaksatraToolStripMenuItem.Click += new System.EventHandler(this.columnNaksatraToolStripMenuItem_Click);
            // 
            // columnRasiOfTheMoonToolStripMenuItem
            // 
            this.columnRasiOfTheMoonToolStripMenuItem.Name = "columnRasiOfTheMoonToolStripMenuItem";
            this.columnRasiOfTheMoonToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.columnRasiOfTheMoonToolStripMenuItem.Text = "Column Rasi of the Moon";
            this.columnRasiOfTheMoonToolStripMenuItem.Click += new System.EventHandler(this.columnRasiOfTheMoonToolStripMenuItem_Click);
            // 
            // columnSunriseToolStripMenuItem
            // 
            this.columnSunriseToolStripMenuItem.Name = "columnSunriseToolStripMenuItem";
            this.columnSunriseToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.columnSunriseToolStripMenuItem.Text = "Column Sunrise";
            this.columnSunriseToolStripMenuItem.Click += new System.EventHandler(this.columnSunriseToolStripMenuItem_Click);
            // 
            // columnNoonToolStripMenuItem
            // 
            this.columnNoonToolStripMenuItem.Name = "columnNoonToolStripMenuItem";
            this.columnNoonToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.columnNoonToolStripMenuItem.Text = "Column Noon";
            this.columnNoonToolStripMenuItem.Click += new System.EventHandler(this.columnNoonToolStripMenuItem_Click);
            // 
            // columnSunsetToolStripMenuItem
            // 
            this.columnSunsetToolStripMenuItem.Name = "columnSunsetToolStripMenuItem";
            this.columnSunsetToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.columnSunsetToolStripMenuItem.Text = "Column Sunset";
            this.columnSunsetToolStripMenuItem.Click += new System.EventHandler(this.columnSunsetToolStripMenuItem_Click);
            // 
            // columnCoreEventsToolStripMenuItem
            // 
            this.columnCoreEventsToolStripMenuItem.Name = "columnCoreEventsToolStripMenuItem";
            this.columnCoreEventsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.columnCoreEventsToolStripMenuItem.Text = "Column Core Events";
            this.columnCoreEventsToolStripMenuItem.Click += new System.EventHandler(this.columnCoreEventsToolStripMenuItem_Click);
            // 
            // CalendarTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "CalendarTab";
            this.Size = new System.Drawing.Size(800, 560);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Views.CalendarTableView calendarTableView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Drawing.Printing.PrintDocument printDocumentTable;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem plainTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tableToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem smallTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallTextToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem normalTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largeTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largestTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printMultipleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem ekadasiMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCompleteDataToolStripMenuItem;
        private CalendarDataView.CalendarDataView calendarDataView1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem columnYogaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnNaksatraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnRasiOfTheMoonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnSunriseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnNoonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnSunsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnCoreEventsToolStripMenuItem;
    }
}
