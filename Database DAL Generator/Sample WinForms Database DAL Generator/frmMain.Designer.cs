namespace Sample_WinForms_Database_DAL_Generator
{
    partial class frmMain
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
            this.lbUsings = new System.Windows.Forms.ListBox();
            this.btnBuildDALExcel = new System.Windows.Forms.Button();
            this.txtAddUsing = new System.Windows.Forms.TextBox();
            this.btnAddUsing = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbNSPrefix = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNSPrefix = new System.Windows.Forms.TextBox();
            this.txtDbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTblDbOutExten = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbIgnoreTblSchema = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTopAppName = new System.Windows.Forms.TextBox();
            this.txtOutDir = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCnString = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBuildDALSQL = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbUsings
            // 
            this.lbUsings.FormattingEnabled = true;
            this.lbUsings.ItemHeight = 16;
            this.lbUsings.Location = new System.Drawing.Point(12, 57);
            this.lbUsings.Name = "lbUsings";
            this.lbUsings.Size = new System.Drawing.Size(334, 196);
            this.lbUsings.TabIndex = 1;
            this.lbUsings.DoubleClick += new System.EventHandler(this.lbUsings_DoubleClick);
            // 
            // btnBuildDALExcel
            // 
            this.btnBuildDALExcel.Location = new System.Drawing.Point(596, 589);
            this.btnBuildDALExcel.Name = "btnBuildDALExcel";
            this.btnBuildDALExcel.Size = new System.Drawing.Size(140, 54);
            this.btnBuildDALExcel.TabIndex = 2;
            this.btnBuildDALExcel.Text = "Build DAL From Excel Files";
            this.btnBuildDALExcel.UseVisualStyleBackColor = true;
            this.btnBuildDALExcel.Click += new System.EventHandler(this.btnBuildDALExcel_Click);
            // 
            // txtAddUsing
            // 
            this.txtAddUsing.Location = new System.Drawing.Point(12, 29);
            this.txtAddUsing.Name = "txtAddUsing";
            this.txtAddUsing.Size = new System.Drawing.Size(253, 22);
            this.txtAddUsing.TabIndex = 4;
            // 
            // btnAddUsing
            // 
            this.btnAddUsing.Location = new System.Drawing.Point(271, 29);
            this.btnAddUsing.Name = "btnAddUsing";
            this.btnAddUsing.Size = new System.Drawing.Size(75, 23);
            this.btnAddUsing.TabIndex = 5;
            this.btnAddUsing.Text = "Add";
            this.btnAddUsing.UseVisualStyleBackColor = true;
            this.btnAddUsing.Click += new System.EventHandler(this.btnAddUsing_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "(usings, double click to remove)";
            // 
            // cbNSPrefix
            // 
            this.cbNSPrefix.AutoSize = true;
            this.cbNSPrefix.Location = new System.Drawing.Point(15, 338);
            this.cbNSPrefix.Name = "cbNSPrefix";
            this.cbNSPrefix.Size = new System.Drawing.Size(168, 21);
            this.cbNSPrefix.TabIndex = 7;
            this.cbNSPrefix.Text = "use namespace prefix";
            this.cbNSPrefix.UseVisualStyleBackColor = true;
            this.cbNSPrefix.CheckedChanged += new System.EventHandler(this.cbNSPrefix_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 318);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "(or will use db name + table schema)";
            // 
            // txtNSPrefix
            // 
            this.txtNSPrefix.Enabled = false;
            this.txtNSPrefix.Location = new System.Drawing.Point(15, 365);
            this.txtNSPrefix.Name = "txtNSPrefix";
            this.txtNSPrefix.Size = new System.Drawing.Size(331, 22);
            this.txtNSPrefix.TabIndex = 9;
            this.txtNSPrefix.Text = "db name and table schema will be used";
            // 
            // txtDbName
            // 
            this.txtDbName.Location = new System.Drawing.Point(212, 491);
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Size = new System.Drawing.Size(134, 22);
            this.txtDbName.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 494);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "DB Name";
            // 
            // txtTblDbOutExten
            // 
            this.txtTblDbOutExten.Location = new System.Drawing.Point(212, 463);
            this.txtTblDbOutExten.Name = "txtTblDbOutExten";
            this.txtTblDbOutExten.Size = new System.Drawing.Size(134, 22);
            this.txtTblDbOutExten.TabIndex = 12;
            this.txtTblDbOutExten.Text = ".txt";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 466);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Table + Db File Extension";
            // 
            // cbIgnoreTblSchema
            // 
            this.cbIgnoreTblSchema.AutoSize = true;
            this.cbIgnoreTblSchema.Checked = true;
            this.cbIgnoreTblSchema.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIgnoreTblSchema.Location = new System.Drawing.Point(15, 393);
            this.cbIgnoreTblSchema.Name = "cbIgnoreTblSchema";
            this.cbIgnoreTblSchema.Size = new System.Drawing.Size(158, 21);
            this.cbIgnoreTblSchema.TabIndex = 14;
            this.cbIgnoreTblSchema.Text = "Ignore table schema";
            this.cbIgnoreTblSchema.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 522);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "Top App Name (Namespace)";
            // 
            // txtTopAppName
            // 
            this.txtTopAppName.Location = new System.Drawing.Point(212, 519);
            this.txtTopAppName.Name = "txtTopAppName";
            this.txtTopAppName.Size = new System.Drawing.Size(134, 22);
            this.txtTopAppName.TabIndex = 15;
            // 
            // txtOutDir
            // 
            this.txtOutDir.Location = new System.Drawing.Point(386, 29);
            this.txtOutDir.Name = "txtOutDir";
            this.txtOutDir.Size = new System.Drawing.Size(334, 22);
            this.txtOutDir.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(383, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "output directory";
            // 
            // txtCnString
            // 
            this.txtCnString.Location = new System.Drawing.Point(12, 649);
            this.txtCnString.Name = "txtCnString";
            this.txtCnString.Size = new System.Drawing.Size(724, 22);
            this.txtCnString.TabIndex = 20;
            this.txtCnString.Text = "Data Source=SERVERADDRESS;Initial Catalog=DBNAME;User Id=USERNAME;Password=USERPW" +
    "D;";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 629);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 17);
            this.label7.TabIndex = 21;
            this.label7.Text = "Connection String";
            // 
            // btnBuildDALSQL
            // 
            this.btnBuildDALSQL.Location = new System.Drawing.Point(450, 589);
            this.btnBuildDALSQL.Name = "btnBuildDALSQL";
            this.btnBuildDALSQL.Size = new System.Drawing.Size(140, 54);
            this.btnBuildDALSQL.TabIndex = 22;
            this.btnBuildDALSQL.Text = "Build DAL From SQL Server";
            this.btnBuildDALSQL.UseVisualStyleBackColor = true;
            this.btnBuildDALSQL.Click += new System.EventHandler(this.btnBuildDALSQL_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(619, 217);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(101, 36);
            this.btnSaveConfig.TabIndex = 23;
            this.btnSaveConfig.Text = "Save Config";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(619, 268);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(101, 36);
            this.btnLoad.TabIndex = 23;
            this.btnLoad.Text = "Load Config";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 683);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.btnBuildDALSQL);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCnString);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtOutDir);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTopAppName);
            this.Controls.Add(this.cbIgnoreTblSchema);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTblDbOutExten);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDbName);
            this.Controls.Add(this.txtNSPrefix);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbNSPrefix);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddUsing);
            this.Controls.Add(this.txtAddUsing);
            this.Controls.Add(this.btnBuildDALExcel);
            this.Controls.Add(this.lbUsings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lbUsings;
        private System.Windows.Forms.Button btnBuildDALExcel;
        private System.Windows.Forms.TextBox txtAddUsing;
        private System.Windows.Forms.Button btnAddUsing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbNSPrefix;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNSPrefix;
        private System.Windows.Forms.TextBox txtDbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTblDbOutExten;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbIgnoreTblSchema;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTopAppName;
        private System.Windows.Forms.TextBox txtOutDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCnString;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnBuildDALSQL;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnLoad;
    }
}

