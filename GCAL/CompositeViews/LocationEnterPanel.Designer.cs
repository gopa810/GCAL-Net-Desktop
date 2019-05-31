namespace GCAL.CompositeViews
{
    partial class LocationEnterPanel
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbLatDeg = new System.Windows.Forms.TextBox();
            this.tbLongDeg = new System.Windows.Forms.TextBox();
            this.tbLatArc = new System.Windows.Forms.TextBox();
            this.btnLatDir = new System.Windows.Forms.Button();
            this.btnLongDir = new System.Windows.Forms.Button();
            this.tbLongArc = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbTimezones = new System.Windows.Forms.ComboBox();
            this.labelDstInfo = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Latitude";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Longitude";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "°";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(136, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "°";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(209, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "\'";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(209, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(9, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "\'";
            // 
            // tbLatDeg
            // 
            this.tbLatDeg.Location = new System.Drawing.Point(80, 64);
            this.tbLatDeg.Name = "tbLatDeg";
            this.tbLatDeg.Size = new System.Drawing.Size(50, 20);
            this.tbLatDeg.TabIndex = 9;
            this.tbLatDeg.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // tbLongDeg
            // 
            this.tbLongDeg.Location = new System.Drawing.Point(80, 104);
            this.tbLongDeg.Name = "tbLongDeg";
            this.tbLongDeg.Size = new System.Drawing.Size(50, 20);
            this.tbLongDeg.TabIndex = 10;
            this.tbLongDeg.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // tbLatArc
            // 
            this.tbLatArc.Location = new System.Drawing.Point(153, 64);
            this.tbLatArc.Name = "tbLatArc";
            this.tbLatArc.Size = new System.Drawing.Size(50, 20);
            this.tbLatArc.TabIndex = 11;
            this.tbLatArc.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // btnLatDir
            // 
            this.btnLatDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLatDir.Location = new System.Drawing.Point(233, 64);
            this.btnLatDir.Name = "btnLatDir";
            this.btnLatDir.Size = new System.Drawing.Size(75, 23);
            this.btnLatDir.TabIndex = 12;
            this.btnLatDir.Text = "North";
            this.btnLatDir.UseVisualStyleBackColor = true;
            this.btnLatDir.Click += new System.EventHandler(this.buttonLatDir_Click);
            // 
            // btnLongDir
            // 
            this.btnLongDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLongDir.Location = new System.Drawing.Point(233, 102);
            this.btnLongDir.Name = "btnLongDir";
            this.btnLongDir.Size = new System.Drawing.Size(75, 23);
            this.btnLongDir.TabIndex = 13;
            this.btnLongDir.Text = "East";
            this.btnLongDir.UseVisualStyleBackColor = true;
            this.btnLongDir.Click += new System.EventHandler(this.buttonLongDir_Click);
            // 
            // tbLongArc
            // 
            this.tbLongArc.Location = new System.Drawing.Point(153, 104);
            this.tbLongArc.Name = "tbLongArc";
            this.tbLongArc.Size = new System.Drawing.Size(50, 20);
            this.tbLongArc.TabIndex = 14;
            this.tbLongArc.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Timezone";
            // 
            // cbTimezones
            // 
            this.cbTimezones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTimezones.FormattingEnabled = true;
            this.cbTimezones.Location = new System.Drawing.Point(80, 152);
            this.cbTimezones.Name = "cbTimezones";
            this.cbTimezones.Size = new System.Drawing.Size(265, 21);
            this.cbTimezones.TabIndex = 16;
            this.cbTimezones.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // labelDstInfo
            // 
            this.labelDstInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDstInfo.Location = new System.Drawing.Point(77, 189);
            this.labelDstInfo.Name = "labelDstInfo";
            this.labelDstInfo.Size = new System.Drawing.Size(377, 45);
            this.labelDstInfo.TabIndex = 18;
            this.labelDstInfo.Text = "Sample";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(330, 106);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(129, 17);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "Auto update timezone";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(298, 237);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 20;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(379, 237);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 21;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Location Name";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(108, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(346, 20);
            this.textBox1.TabIndex = 8;
            // 
            // LocationEnterPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.labelDstInfo);
            this.Controls.Add(this.cbTimezones);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbLongArc);
            this.Controls.Add(this.btnLongDir);
            this.Controls.Add(this.btnLatDir);
            this.Controls.Add(this.tbLatArc);
            this.Controls.Add(this.tbLongDeg);
            this.Controls.Add(this.tbLatDeg);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "LocationEnterPanel";
            this.Size = new System.Drawing.Size(480, 284);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbLatDeg;
        private System.Windows.Forms.TextBox tbLongDeg;
        private System.Windows.Forms.TextBox tbLatArc;
        private System.Windows.Forms.Button btnLatDir;
        private System.Windows.Forms.Button btnLongDir;
        private System.Windows.Forms.TextBox tbLongArc;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbTimezones;
        private System.Windows.Forms.Label labelDstInfo;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
    }
}
