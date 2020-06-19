using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Base_Generator_Logic
{
    public class sProject
    {

        public sDb Database { get; set; }



        // constructor that loads the project from excel
        public sProject(string dbName, string path, string fileName)
        {
            Database = new sDb();
            Database.Name = dbName;
            this.LoadExcel(path, fileName);
        }



        // constructor that's needed in direct sql build
        public sProject(string dbName, System.Data.DataTable tables, System.Data.DataTable constraints, System.Data.DataTable columns)
        {
            Database = new sDb();
            Database.Name = dbName;

            Database.Tables = new List<sTable>();
            foreach (DataRow row in tables.Rows)
            {
                sTable table = new sTable()
                {
                    // prepare columns list
                    Columns = new List<sColumn>(),

                    // prepare constraints list
                    Constraints = new List<sConstraint>(),

                    // fetch TABLE_NAME
                    Name = (row["TABLE_NAME"] != null) ? row["TABLE_NAME"].ToString() : "",

                    // fetch TABLE_SCHEMA
                    SchemaName = (row["TABLE_SCHEMA"] != null) ? row["TABLE_SCHEMA"].ToString() : ""
                };
                Database.Tables.Add(table);
            }

            foreach (DataRow row in columns.Rows)
            {
                //fetch TABLE_NAME of the column
                string tableName = (row["TABLE_NAME"] != null) ? row["TABLE_NAME"].ToString() : "";

                //fetch TABLE_SCHEMA of the column
                string tableSchema = (row["TABLE_SCHEMA"] != null) ? row["TABLE_SCHEMA"].ToString() : "";

                //find the table from the tables, then skip if no table matching
                sTable table = Database.Tables.Where(t => t.Name == tableName & t.SchemaName == tableSchema).FirstOrDefault();
                if (table == null) continue;

                //iterate and fill the columns
                sColumn column = new sColumn(table)
                {
                    //fetch COLUMN_NAME
                    Name = (row["COLUMN_NAME"] != null) ? row["COLUMN_NAME"].ToString() : "",

                    //fetch and convert ORDINAL_POSITION
                    Position = int.Parse(row["ORDINAL_POSITION"].ToString()),

                    //fetch and convert DATA_TYPE
                    SqlDatatype = sEnum.ParseStringToSqlDataType(row["DATA_TYPE"].ToString()),

                    //fetch and convert IS_NULLABLE
                    IsNullable = bool.Parse(row["IS_NULLABLE"].ToString()),

                    //fetch and convert IS_IDENTITY
                    IsIdentity = bool.Parse(row["IS_IDENTITY"].ToString()),

                    //prepare the options
                    Options = new List<Tuple<sEnum.SqlOption, dynamic>>()
                };

                //fetch CHARACTER_MAXIMUM_LENGTH
                string _mxchar = row["CHARACTER_MAXIMUM_LENGTH"].ToString();

                //fetch NUMERIC_PRECISION
                string _numprec = row["NUMERIC_PRECISION"].ToString();

                //fetch NUMERIC_SCALE
                string _numscl = row["NUMERIC_SCALE"].ToString();

                if (_mxchar != "")
                {
                    column.Options.Add(new Tuple<sEnum.SqlOption, dynamic>(sEnum.SqlOption.MaxChar, int.Parse(_mxchar)));
                }

                if (_numprec != "")
                {
                    column.Options.Add(new Tuple<sEnum.SqlOption, dynamic>(sEnum.SqlOption.NumPrecision, int.Parse(_numprec)));
                }

                if (_numscl != "")
                {
                    column.Options.Add(new Tuple<sEnum.SqlOption, dynamic>(sEnum.SqlOption.NumScale, int.Parse(_numscl)));
                }

                table.Columns.Add(column);
            }

            foreach (DataRow row in constraints.Rows)
            {
                //fetch COLUMN_NAME
                string colName = row["COLUMN_NAME"].ToString();

                //fetch TABLE_NAME
                string tblName = row["TABLE_NAME"].ToString();

                //fetch TABLE_SCHEMA
                string tblSchm = row["TABLE_SCHEMA"].ToString();

                //search for the appropriate table, then skip if not found
                sTable table = Database.Tables.Where(t => t.Name == tblName & t.SchemaName == tblSchm).FirstOrDefault();
                if (table == null) continue;

                //search for the appropriate column, then skip if not found
                sColumn column = table.Columns.Where(c => c.Name == colName).FirstOrDefault();
                if (column == null) continue;

                sConstraint constraint = new sConstraint(column)
                {
                    //fetch CONSTRAINT_SCHEMA
                    Schema = row["CONSTRAINT_SCHEMA"].ToString(),

                    //fetch CONSTRAINT_NAME
                    Name = row["CONSTRAINT_NAME"].ToString()
                };

                //get the Enum of the type of the constraint or else skip the whole thing
                string contrType = row["CONSTRAINT_TYPE"].ToString();
                if (contrType == "PRIMARY KEY") constraint.ConstraintType = sEnum.SqlConstraintType.PrimaryKey;
                else if (contrType == "FOREIGN KEY")
                {
                    constraint.ConstraintType = sEnum.SqlConstraintType.ForeignKey;

                    //fetch extra data related to the primary key
                    string pkTblName = row["PK_TABLE_NAME"].ToString();
                    string pkTblSchm = row["PK_TABLE_SCHEMA"].ToString();
                    string pkColumnName = row["PK_COLUMN_NAME"].ToString();
                    sTable pkTable = Database.Tables.Where(t => t.Name == pkTblName & t.SchemaName == pkTblSchm).FirstOrDefault();
                    sColumn pkColumn = pkTable.Columns.Where(c => c.Name == pkColumnName).FirstOrDefault();

                    constraint.PK_Table = pkTable;
                    constraint.PK_Column = pkColumn;
                }
                else continue;
                
                table.Constraints.Add(constraint);
            }

        }



        private void FetchTables(Range range, sDb database)
        {

            database.Tables = new List<sTable>();
            int i = 2;

            while (range.Cells[i, 1].Value2 != null)
            {
                sTable table = new sTable()
                {
                    //prepare columns list
                    Columns = new List<sColumn>(),

                    //prepare constraints list
                    Constraints = new List<sConstraint>(),

                    //fetch TABLE_NAME
                    Name = range.Cells[i, 1].Value2.ToString(),

                    //fetch TABLE_SCHEMA
                    SchemaName = range.Cells[i, 2].Value2.ToString()
                };
                database.Tables.Add(table);
                i++;
            }

        }



        private void FetchColumns(Range range, sDb database)
        {
            int i = 2;
            while (range.Cells[i, 1].Value2 != null)
            {
                //fetch TABLE_NAME of the column
                string tableName = range.Cells[i, 1].Value2.ToString();

                //fetch TABLE_SCHEMA of the column
                string tableSchema = range.Cells[i, 2].Value2.ToString();

                //find the table from the tables, then skip if no table matching
                sTable table = database.Tables.Where(t => t.Name == tableName & t.SchemaName == tableSchema).FirstOrDefault();
                if (table == null) continue;

                //iterate and fill the columns
                sColumn column = new sColumn(table)
                {
                    //fetch COLUMN_NAME
                    Name = range.Cells[i, 3].Value2.ToString(),

                    //fetch and convert ORDINAL_POSITION
                    Position = int.Parse(range.Cells[i, 4].Value2.ToString()),

                    //fetch and convert DATA_TYPE
                    SqlDatatype = sEnum.ParseStringToSqlDataType(range.Cells[i, 5].Value2.ToString()),

                    //fetch and convert IS_NULLABLE
                    IsNullable = bool.Parse(range.Cells[i, 6].Value2.ToString()),

                    //fetch and convert IS_IDENTITY
                    IsIdentity = bool.Parse(range.Cells[i, 10].Value2.ToString()),

                    //prepare the options
                    Options = new List<Tuple<sEnum.SqlOption, dynamic>>()
                };

                //fetch CHARACTER_MAXIMUM_LENGTH
                string _mxchar = range.Cells[i, 7].Value2.ToString();

                //fetch NUMERIC_PRECISION
                string _numprec = range.Cells[i, 8].Value2.ToString();

                //fetch NUMERIC_SCALE
                string _numscl = range.Cells[i, 9].Value2.ToString();

                if (_mxchar != "NULL")
                {
                    column.Options.Add(new Tuple<sEnum.SqlOption, dynamic>(sEnum.SqlOption.MaxChar, int.Parse(_mxchar)));
                }

                if (_numprec != "NULL")
                {
                    column.Options.Add(new Tuple<sEnum.SqlOption, dynamic>(sEnum.SqlOption.NumPrecision, int.Parse(_numprec)));
                }

                if (_numscl != "NULL")
                {
                    column.Options.Add(new Tuple<sEnum.SqlOption, dynamic>(sEnum.SqlOption.NumScale, int.Parse(_numscl)));
                }

                table.Columns.Add(column);

                i++;
            }
        }



        private void FetchConstraints(Range range, sDb database)
        {
            int i = 2;
            while (range.Cells[i, 1].Value2 != null)
            {
                //fetch COLUMN_NAME
                string colName = range.Cells[i, 6].Value2.ToString();

                //fetch TABLE_NAME
                string tblName = range.Cells[i, 4].Value2.ToString();

                //fetch TABLE_SCHEMA
                string tblSchm = range.Cells[i, 3].Value2.ToString();

                //search for the appropriate table, then skip if not found
                sTable table = database.Tables.Where(t => t.Name == tblName & t.SchemaName == tblSchm).FirstOrDefault();
                if (table == null) continue;

                //search for the appropriate column, then skip if not found
                sColumn column = table.Columns.Where(c => c.Name == colName).FirstOrDefault();
                if (column == null) continue;

                sConstraint constraint = new sConstraint(column)
                {
                    //fetch CONSTRAINT_SCHEMA
                    Schema = range.Cells[i, 1].Value2.ToString(),

                    //fetch CONSTRAINT_NAME
                    Name = range.Cells[i, 2].Value2.ToString()
                };

                //get the Enum of the type of the constraint or else skip the whole thing
                string contrType = range.Cells[i, 5].Value2.ToString();
                if (contrType == "PRIMARY KEY") constraint.ConstraintType = sEnum.SqlConstraintType.PrimaryKey;
                else if (contrType == "FOREIGN KEY")
                {
                    constraint.ConstraintType = sEnum.SqlConstraintType.ForeignKey;

                    //fetch extra data related to the primary key
                    string pkTblName = range.Cells[i, 7].Value2.ToString();
                    string pkTblSchm = range.Cells[i, 8].Value2.ToString();
                    string pkColumnName = range.Cells[i, 9].Value2.ToString();
                    sTable pkTable = database.Tables.Where(t => t.Name == pkTblName & t.SchemaName == pkTblSchm).FirstOrDefault();
                    sColumn pkColumn = pkTable.Columns.Where(c => c.Name == pkColumnName).FirstOrDefault();

                    constraint.PK_Table = pkTable;
                    constraint.PK_Column = pkColumn;
                }
                else continue;

                table.Constraints.Add(constraint);

                i++;
            }
        }



        private void LoadExcel(string path, string fileName)
        {
            Application excel = new Application();
            Workbooks workbooks = excel.Workbooks;
            Workbook workbook = workbooks.Open(Path.Combine(path, fileName));
            Worksheet exTables = workbook.Sheets[1];
            Worksheet exColumns = workbook.Sheets[2];
            Worksheet exConstraints = workbook.Sheets[3];

            Range rgTables = exTables.UsedRange;
            Range rgColumns = exColumns.UsedRange;
            Range rgConstraints = exConstraints.UsedRange;

            //get all tables first
            //get columns
            //get constraints
            FetchTables(rgTables, this.Database);
            FetchColumns(rgColumns, this.Database);
            FetchConstraints(rgConstraints, this.Database);

            //clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(rgTables);
            Marshal.ReleaseComObject(rgColumns);
            Marshal.ReleaseComObject(rgConstraints);

            Marshal.ReleaseComObject(exTables);
            Marshal.ReleaseComObject(exColumns);
            Marshal.ReleaseComObject(exConstraints);

            workbook.Close();
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(workbooks);

            //quit and release
            excel.Quit();
            Marshal.ReleaseComObject(excel);
        }



        public static void WriteStringToFile(string text, string path, string fileName)
        {
            string fullPath = Path.Combine(path, fileName);
            StreamWriter sr = new StreamWriter(fullPath);
            sr.Write(text);
            sr.Close();
        }
        
    }
}
