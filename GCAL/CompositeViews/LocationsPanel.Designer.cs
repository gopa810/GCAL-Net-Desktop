namespace GCAL.CompositeViews
{
    partial class LocationsPanel
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
            this.m_wndCountry = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewLocations = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelLimitInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_wndCountry
            // 
            this.m_wndCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_wndCountry.FormattingEnabled = true;
            this.m_wndCountry.Location = new System.Drawing.Point(6, 77);
            this.m_wndCountry.Name = "m_wndCountry";
            this.m_wndCountry.Size = new System.Drawing.Size(277, 21);
            this.m_wndCountry.TabIndex = 10;
            this.m_wndCountry.SelectedIndexChanged += new System.EventHandler(this.m_wndCountry_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selected country";
            // 
            // m_wndLocs
            // 
            this.listViewLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listViewLocations.FullRowSelect = true;
            this.listViewLocations.Location = new System.Drawing.Point(6, 104);
            this.listViewLocations.Name = "m_wndLocs";
            this.listViewLocations.Size = new System.Drawing.Size(660, 320);
            this.listViewLocations.TabIndex = 2;
            this.listViewLocations.UseCompatibleStateImageBehavior = false;
            this.listViewLocations.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "City";
            this.columnHeader1.Width = 182;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Country";
            this.columnHeader2.Width = 82;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Latitude";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Longitude";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Timezone";
            this.columnHeader5.Width = 150;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(9, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "New";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(67, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 34);
            this.button2.TabIndex = 4;
            this.button2.Text = "Edit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(118, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(52, 34);
            this.button3.TabIndex = 5;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(207, 15);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(51, 34);
            this.button4.TabIndex = 6;
            this.button4.Text = "Import";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(264, 15);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(57, 34);
            this.button5.TabIndex = 7;
            this.button5.Text = "Export";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(327, 15);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(82, 34);
            this.button6.TabIndex = 8;
            this.button6.Text = "Google Maps";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(415, 15);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(49, 34);
            this.button7.TabIndex = 9;
            this.button7.Text = "Reset";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(289, 77);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(377, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.onTextFilterChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Search";
            // 
            // labelLimitInfo
            // 
            this.labelLimitInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLimitInfo.Location = new System.Drawing.Point(6, 427);
            this.labelLimitInfo.Name = "labelLimitInfo";
            this.labelLimitInfo.Size = new System.Drawing.Size(660, 30);
            this.labelLimitInfo.TabIndex = 12;
            this.labelLimitInfo.Text = "Limited";
            this.labelLimitInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LocationsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelLimitInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listViewLocations);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_wndCountry);
            this.Name = "LocationsPanel";
            this.Size = new System.Drawing.Size(669, 457);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_wndCountry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewLocations;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelLimitInfo;
    }
}
