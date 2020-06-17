using Base_Generator_Logic;
using Newtonsoft.Json;
using Sample_WinForms_Database_DAL_Generator.Helper;
using Sample_WinForms_Database_DAL_Generator.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample_WinForms_Database_DAL_Generator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void buildClassesFromExcels()
        {
            string dbName = txtDbName.Text.Trim();
            string appname = txtTopAppName.Text.Trim();

            if (dbName == "" | appname == "")
            {
                MessageBox.Show("DB and App names are a must");
                return;
            }

            string outFold = txtOutDir.Text; //Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fullFold = Path.Combine(outFold, dbName);

            if (!Directory.Exists(fullFold)) Directory.CreateDirectory(fullFold);

            string fileName = dbName + ".xlsx";
            string completefilePath = Path.Combine(fullFold, fileName);
            FileInfo file = new FileInfo(completefilePath);
            if (file.Exists == false)
            {
                MessageBox.Show("the file " + completefilePath + " not found");
                return;
            }

            sProject project = new sProject(dbName, fullFold, fileName);

            List<string> usings = new List<string>();

            foreach (var item in lbUsings.Items)
            {
                usings.Add(item.ToString());
            }

            //add db namespace
            usings.Add(project.Database.getDbNameSpace(appname));

            foreach (sTable table in project.Database.Tables)
            {
                string tblFold;
                if (cbIgnoreTblSchema.Checked) tblFold = Path.Combine(fullFold, "Output", "Tables");
                else tblFold = Path.Combine(fullFold, "Output", "Tables", table.SchemaName);
                if (!Directory.Exists(tblFold)) Directory.CreateDirectory(tblFold);

                var ns = "";
                if (cbNSPrefix.Checked) ns = txtNSPrefix.Text.Trim();
                else
                {
                    string topAppNS = txtTopAppName.Text.Trim() + ".";
                    string schem = (cbIgnoreTblSchema.Checked) ? "" : table.SchemaName.Trim() + ".";
                    ns = string.Format("{2}{0}.DAL.{1}DataObjects", dbName, schem, topAppNS);
                }

                string txtTblCls = table.BuildClassFileText(project.Database, usings, ns);
                sProject.WriteStringToFile(txtTblCls, tblFold, table.Name.Trim() + txtTblDbOutExten.Text);
            }

            string dbFold = Path.Combine(fullFold, "Output", "Database");
            if (!Directory.Exists(dbFold)) Directory.CreateDirectory(dbFold);
            //remove the last using since it is added before and it is the same database using also
            usings.RemoveAt(usings.Count - 1);
            string txtDbCls = project.Database.BuildDbClassFileText(usings, string.Format("{0}.{1}.DAL.Database", txtTopAppName.Text.Trim(), dbName.Trim()));
            sProject.WriteStringToFile(txtDbCls, dbFold, string.Format("{0}{1}", dbName.Trim(), txtTblDbOutExten.Text));

            MessageBox.Show("Done!");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lbUsings.Items.Add("System");
            lbUsings.Items.Add("System.Data");
            lbUsings.Items.Add("System.Data.SqlClient");
            lbUsings.Items.Add("System.Collections.Generic");

            txtOutDir.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).ToString();

            LoadConfig();




            //var all = GATAC.GATACDb.DAL.DataObjects.BadgeId_Extract_20191023.GetAllBadgeId_Extract_20191023();
            //var all2 = GATAC.GATACDb.DAL.DataObjects.Policies.GetAllPolicies();

            //var bookings = GATAC.GATACDb.DAL.DataObjects.Bookings.GetAllBookings();

        }


        private void lbUsings_DoubleClick(object sender, EventArgs e)
        {
            if (lbUsings.SelectedItem != null)
            {
                int idx = lbUsings.SelectedIndex;
                lbUsings.Items.RemoveAt(idx);
            }
        }

        private void cbNSPrefix_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                txtNSPrefix.Text = "ex: MYAPP.SomethingElse.SomethingExtra";
                txtNSPrefix.Enabled = true;
            }
            else
            {
                txtNSPrefix.Text = "db name and table schema will be used";
                txtNSPrefix.Enabled = false;
            }
        }

        private void btnAddUsing_Click(object sender, EventArgs e)
        {
            string nUsing = txtAddUsing.Text.ToString().Trim();
            txtAddUsing.Clear();

            lbUsings.Items.Add(nUsing);
        }

        private void btnBuildDALExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                buildClassesFromExcels();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnBuildDALSQL_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                buildClassesFromSQL();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void buildClassesFromSQL()
        {
            DataSet dsTbl = new DataSet();
            DataSet dsConstr = new DataSet();
            DataSet dsCols = new DataSet();
            using (SqlConnection cn = new SqlConnection(txtCnString.Text))
            {
                SqlCommand cm = cn.CreateCommand();
                cn.Open();

                cm.CommandText = cmTblString;
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dsTbl);

                cm.CommandText = cmConstrtString;
                da = new SqlDataAdapter(cm);
                da.Fill(dsConstr);

                cm.CommandText = cmColsString;
                da = new SqlDataAdapter(cm);
                da.Fill(dsCols);

                cn.Close();
            }





            string dbName = txtDbName.Text.Trim();
            string appname = txtTopAppName.Text.Trim();

            if (dbName == "" | appname == "")
            {
                MessageBox.Show("DB and App names are a must");
                return;
            }

            string outFold = txtOutDir.Text; //Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fullFold = Path.Combine(outFold, dbName);

            if (!Directory.Exists(fullFold)) Directory.CreateDirectory(fullFold);

            Cursor.Current = Cursors.WaitCursor;

            sProject project = new sProject(dbName, dsTbl.Tables[0], dsConstr.Tables[0], dsCols.Tables[0]);

            List<string> usings = new List<string>();

            foreach (var item in lbUsings.Items)
            {
                usings.Add(item.ToString());
            }

            //add db namespace
            usings.Add(project.Database.getDbNameSpace(appname));

            foreach (sTable table in project.Database.Tables)
            {
                string tblFold;
                if (cbIgnoreTblSchema.Checked) tblFold = Path.Combine(fullFold, "Output", "Tables");
                else tblFold = Path.Combine(fullFold, "Output", "Tables", table.SchemaName);
                if (!Directory.Exists(tblFold)) Directory.CreateDirectory(tblFold);

                var ns = "";
                if (cbNSPrefix.Checked) ns = txtNSPrefix.Text.Trim();
                else
                {
                    string topAppNS = txtTopAppName.Text.Trim() + ".";
                    string schem = (cbIgnoreTblSchema.Checked) ? "" : table.SchemaName.Trim() + ".";
                    ns = string.Format("{2}{0}.DAL.{1}DataObjects", dbName, schem, topAppNS);
                }

                string txtTblCls = table.BuildClassFileText(project.Database, usings, ns);
                sProject.WriteStringToFile(txtTblCls, tblFold, table.Name.Trim() + txtTblDbOutExten.Text);
            }

            string dbFold = Path.Combine(fullFold, "Output", "Database");
            if (!Directory.Exists(dbFold)) Directory.CreateDirectory(dbFold);
            //remove the last using since it is added before and it is the same database using also
            usings.RemoveAt(usings.Count - 1);
            string txtDbCls = project.Database.BuildDbClassFileText(usings, string.Format("{0}.{1}.DAL.Database", txtTopAppName.Text.Trim(), dbName.Trim()));
            sProject.WriteStringToFile(txtDbCls, dbFold, string.Format("{0}{1}", dbName.Trim(), txtTblDbOutExten.Text));


            Cursor.Current = Cursors.Default;

            MessageBox.Show("Done!");
        }











        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        void SaveConfig()
        {
            var conf = new AppConfig();
            conf.Usings = new List<string>();

            foreach (var item in lbUsings.Items)
            {
                conf.Usings.Add((string)item);
            }

            conf.OutputDirectory = (!string.IsNullOrEmpty(txtOutDir.Text)) ? txtOutDir.Text : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).ToString();
            conf.ConnectionString = txtCnString.Text;
            conf.AppName = txtTopAppName.Text;
            conf.DbName = txtDbName.Text;
            conf.OutputFilesExtension = txtTblDbOutExten.Text;
            conf.IgnoreTableSchema = cbIgnoreTblSchema.Checked;

            if (cbNSPrefix.Checked) conf.NamespacePrefix = txtNSPrefix.Text;

            Settings.Default.AppConfig = JsonConvert.SerializeObject(conf);
            Settings.Default.Save();
        }

        void LoadConfig()
        {
            var confTxt = Settings.Default.AppConfig;
            if (string.IsNullOrWhiteSpace(confTxt)) return;
            else
            {
                var conf = JsonConvert.DeserializeObject<AppConfig>(confTxt);
                lbUsings.Items.Clear();

                foreach (var item in conf.Usings)
                {
                    lbUsings.Items.Add(item);
                }

                txtOutDir.Text = conf.OutputDirectory;
                txtCnString.Text = conf.ConnectionString;
                txtTopAppName.Text = conf.AppName;
                txtDbName.Text = conf.DbName;
                txtTblDbOutExten.Text = conf.OutputFilesExtension;
                cbIgnoreTblSchema.Checked = conf.IgnoreTableSchema;

                if (!string.IsNullOrWhiteSpace(conf.NamespacePrefix))
                {
                    cbNSPrefix.Checked = true;
                    txtNSPrefix.Text = conf.NamespacePrefix;
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }












        #region sqlscripts

        string cmTblString = @"select TABLE_NAME, TABLE_SCHEMA
            from INFORMATION_SCHEMA.TABLES
            where TABLE_TYPE = 'Base Table'";

        string cmConstrtString = @"IF OBJECT_ID('tempdb..#tbl') IS NOT NULL DROP TABLE #tbl
            select * into #tbl
            from (select t1.CONSTRAINT_NAME, t1.CONSTRAINT_SCHEMA, t1.UNIQUE_CONSTRAINT_NAME, t1.UNIQUE_CONSTRAINT_SCHEMA, t2.TABLE_NAME, t2.TABLE_SCHEMA,
            t3.COLUMN_NAME
            from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as t1
            left join (
            select *
            from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
            where CONSTRAINT_TYPE = 'PRIMARY KEY'
            ) as t2
            on t2.CONSTRAINT_NAME = t1.UNIQUE_CONSTRAINT_NAME and t2.CONSTRAINT_SCHEMA = t1.UNIQUE_CONSTRAINT_SCHEMA
            left join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t3
            on t3.CONSTRAINT_NAME = t1.UNIQUE_CONSTRAINT_NAME and t3.CONSTRAINT_SCHEMA = t1.UNIQUE_CONSTRAINT_SCHEMA) as tbl
            --get constraints
            select t1.CONSTRAINT_SCHEMA, t1.CONSTRAINT_NAME, t1.TABLE_SCHEMA, t1.TABLE_NAME, t1.CONSTRAINT_TYPE, t2.COLUMN_NAME,
            (select #tbl.TABLE_NAME
            from #tbl
            where #tbl.CONSTRAINT_NAME = t1.CONSTRAINT_NAME
            and #tbl.CONSTRAINT_SCHEMA = t1.CONSTRAINT_SCHEMA) as PK_TABLE_NAME,
            (select #tbl.TABLE_SCHEMA
            from #tbl
            where #tbl.CONSTRAINT_NAME = t1.CONSTRAINT_NAME
            and #tbl.CONSTRAINT_SCHEMA = t1.CONSTRAINT_SCHEMA)as PK_TABLE_SCHEMA,
            (select #tbl.COLUMN_NAME
            from #tbl
            where #tbl.CONSTRAINT_NAME = t1.CONSTRAINT_NAME
            and #tbl.CONSTRAINT_SCHEMA = t1.CONSTRAINT_SCHEMA)as PK_COLUMN_NAME
            from INFORMATION_SCHEMA.TABLE_CONSTRAINTS as t1
            inner join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t2
            on t1.TABLE_SCHEMA = t2.TABLE_SCHEMA and t1.TABLE_NAME = t2.TABLE_NAME and t1.CONSTRAINT_SCHEMA = t2.CONSTRAINT_SCHEMA and t1.CONSTRAINT_NAME = t2.CONSTRAINT_NAME
            drop table #tbl";

        string cmColsString = @"select TABLE_NAME, TABLE_SCHEMA, COLUMN_NAME, ORDINAL_POSITION, DATA_TYPE, case when IS_NULLABLE = 'YES' then 'TRUE' else 'FALSE' end as IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE,
            (select case  when t2.Is_Identity = 1 Then 'TRUE' else 'FALSE' END as IS_IDENTITY
            from (select Object_Name(object_id) as TableName, OBJECT_SCHEMA_NAME(object_id) as TableSchema, name As ColumnName, is_identity As Is_Identity
		            from sys.columns) as t2
		            where t2.TableName = TABLE_NAME and t2.TableSchema = TABLE_SCHEMA and t2.ColumnName = COLUMN_NAME) as IS_IDENTITY
            from INFORMATION_SCHEMA.COLUMNS";

        #endregion



    }
}
