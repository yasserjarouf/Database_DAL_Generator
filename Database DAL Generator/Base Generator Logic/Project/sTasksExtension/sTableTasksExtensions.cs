using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Generator_Logic.Extensions
{
    public static class sTableTasksExtensions
    {

        const string t1 = "\t";
        const string t2 = "\t\t";
        const string t3 = "\t\t\t";
        const string t4 = "\t\t\t\t";
        const string t5 = "\t\t\t\t\t";
        const string t6 = "\t\t\t\t\t\t";



        #region Commands

        // Clean - Add command
        public static string getBuildAddCommand(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();

            sColumn column = table.Constraints
                .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            // means no primary key
            if (column == null) return "";

            string tblShNa = table.Name;

            sb.AppendLine(t2 + string.Format("public static SqlCommand BuildAdd{0}Command(SqlConnection cn, SqlTransaction trn, {0} _{0})", tblShNa));
            sb.AppendLine(t2 + "{");
            sb.AppendLine(t3 + "if (cn == null | trn == null) throw new NoNullAllowedException();");
            sb.AppendLine(t3 + "SqlCommand cm = new SqlCommand();");
            sb.AppendLine(t3 + "cm.Connection = cn;");
            sb.AppendLine(t3 + "cm.Transaction = trn;");
            sb.AppendLine();

            StringBuilder cmdText = new StringBuilder();
            StringBuilder parms = new StringBuilder();

            // building command text start
            cmdText.AppendLine(t3 + string.Format("cm.CommandText = @\"INSERT INTO {0}", table.FQN));
            cmdText.AppendLine(t4 + "(");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                sColumn col = table.Columns[i];
                if (col.IsIdentity) continue;

                string sfx = (i < table.Columns.Count - 1) ? "," : "";
                cmdText.AppendLine(t5 + col.FQN + sfx);
            }
            cmdText.AppendLine(t4 + ")");
            cmdText.AppendLine(t4 + "VALUES");
            cmdText.AppendLine(t4 + "(");

            // values
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sColumn col = table.Columns[i];
                if (col.IsIdentity) continue;

                string sfx = (i < table.Columns.Count - 1) ? "," : "";
                cmdText.AppendLine(t5 + col.getBasicParameterName() + sfx);
            }
            cmdText.AppendLine(t4 + ");");

            // the return of id
            if (table.HasIdentity())
                cmdText.AppendLine(t4 + "SELECT CONVERT(int, scope_identity())\";");
            else
                cmdText.AppendLine(t4 + "\";");
            // building command text end

            foreach (sColumn col in table.Columns)
            {
                if (col.IsIdentity) continue;
                parms.AppendLine(t3 + col.getAndSetCommandParameter("cm", string.Format("_{0}", tblShNa), null));
            }

            sb.Append(cmdText.ToString());
            sb.AppendLine();
            sb.Append(parms.ToString());

            sb.AppendLine();
            sb.AppendLine(t3 + "return cm;");

            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }



        // Clean - Update command
        public static string getBuildUpdateCommand(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();

            sColumn column = table.Constraints
                .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            // means no primary key
            if (column == null) return "";

            string tblShNa = table.Name;

            sb.AppendLine(t2 + string.Format("public static SqlCommand BuildUpdate{0}Command(SqlConnection cn, SqlTransaction trn, {0} _{0})", tblShNa));
            sb.AppendLine(t2 + "{");
            sb.AppendLine(t3 + "if (cn == null | trn == null) throw new NoNullAllowedException();");
            sb.AppendLine(t3 + "SqlCommand cm = new SqlCommand();");
            sb.AppendLine(t3 + "cm.Connection = cn;");
            sb.AppendLine(t3 + "cm.Transaction = trn;");
            sb.AppendLine();

            StringBuilder cmdText = new StringBuilder();
            StringBuilder parms = new StringBuilder();

            // building command text start
            cmdText.AppendLine(t4 + string.Format("cm.CommandText = @\"UPDATE {0}", table.FQN));
            cmdText.AppendLine(t5 + "SET");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                sColumn col = table.Columns[i];
                if (col.IsIdentity) continue;

                string sfx = (i < table.Columns.Count - 1) ? "," : "";
                cmdText.AppendLine(t6 + string.Format("{0} = {1}{2}", col.FQN, col.getBasicParameterName(), sfx));
            }
            cmdText.AppendLine(t5 + string.Format("WHERE {0} = {1}\";", column.FQN, column.getBasicParameterName()));
            // building command text end

            foreach (sColumn col in table.Columns)
            {
                parms.AppendLine(t4 + col.getAndSetCommandParameter("cm", string.Format("_{0}", tblShNa), null));
            }

            sb.Append(cmdText.ToString());
            sb.AppendLine();
            sb.Append(parms.ToString());

            sb.AppendLine();
            sb.AppendLine(t3 + "return cm;");

            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }



        // Clean - Delete command
        public static string getBuildDeleteCommand(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();

            sColumn column = table.Constraints
                .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            // means no primary key
            if (column == null) return "";

            string tblShNa = table.Name;
            string ColNa = column.Name;
            string ColPTy = column.SqlDatatype.GetCsTypeString();

            sb.AppendLine(t2 + string.Format("public static SqlCommand BuildDeleteBy{0}IdCommand(SqlConnection cn, SqlTransaction trn, {1} {0}{2})", tblShNa, ColPTy, ColNa));
            sb.AppendLine(t2 + "{");
            sb.AppendLine(t3 + "if (cn == null | trn == null) throw new NoNullAllowedException();");
            sb.AppendLine(t3 + "SqlCommand cm = new SqlCommand();");
            sb.AppendLine(t3 + "cm.Connection = cn;");
            sb.AppendLine(t3 + "cm.Transaction = trn;");
            sb.AppendLine();

            sb.AppendLine(t3 + string.Format("cm.CommandText = \"DELETE FROM {0} WHERE {1} = {2}\";", table.FQN, column.FQN, column.getBasicParameterName()));
            sb.AppendLine(t3 + column.getAndSetCommandParameter("cm", null, tblShNa + ColNa));

            sb.AppendLine();
            sb.AppendLine(t3 + "return cm;");

            sb.AppendLine(t2 + "}");
            return sb.ToString();

        }

        #endregion



        #region CRUD

        // Clean - Add method
        public static string getAddDeclarationText(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();

            sColumn column = table.Constraints
                .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            // means no primary key
            if (column == null) return "";

            string tblShNa = table.Name;
            string PkColNa = column.Name;
            string PkColPTy = column.SqlDatatype.GetCsTypeString();
            string PkclTpNulbl = column.getCsAndIfNullTypeWithIgnoreString();

            sb.AppendLine(t2 + string.Format("public static {0} Add{1}({1} _{1})", PkclTpNulbl, tblShNa));
            sb.AppendLine(t2 + "{");

            sb.Append(db.getConnectionBlock(true, t3));
            sb.AppendLine();

            StringBuilder cmdText = new StringBuilder();
            StringBuilder parms = new StringBuilder();

            // building command text start
            cmdText.AppendLine(t3 + string.Format("cm.CommandText = @\"INSERT INTO {0}", table.FQN));
            cmdText.AppendLine(t4 + "(");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                sColumn col = table.Columns[i];
                if (col.IsIdentity) continue;

                string sfx = (i < table.Columns.Count - 1) ? "," : "";
                cmdText.AppendLine(t5 + col.FQN + sfx);
            }
            cmdText.AppendLine(t4 + ")");
            cmdText.AppendLine(t4 + "VALUES");
            cmdText.AppendLine(t4 + "(");

            // values
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sColumn col = table.Columns[i];
                if (col.IsIdentity) continue;

                string sfx = (i < table.Columns.Count - 1) ? "," : "";
                cmdText.AppendLine(t5 + col.getBasicParameterName() + sfx);
            }
            cmdText.AppendLine(t4 + ");");

            if (table.HasIdentity())
                cmdText.AppendLine(t4 + "SELECT CONVERT(int, scope_identity())\";");
            else
                cmdText.AppendLine(t4 + "\";");
            // building command text end

            foreach (sColumn col in table.Columns)
            {
                if (col.IsIdentity) continue;
                parms.AppendLine(t3 + col.getAndSetCommandParameter("cm", string.Format("_{0}", tblShNa), null));
            }

            sb.Append(cmdText.ToString());
            sb.AppendLine();
            sb.Append(parms.ToString());

            sb.AppendLine();
            sb.AppendLine(t3 + "var res = cm.ExecuteScalar();");
            if (column.IsNullable) sb.AppendLine(t3 + "if (res == null) return null;");
            sb.AppendLine(t3 + string.Format("return ({0})res;", PkColPTy));

            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }



        // Clean - Get all method
        public static string getGetAllDeclarationText(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();
            string tblShNa = table.CsRefName;

            sb.AppendLine(t2 + string.Format("public static List<{0}> GetAll{0}(string whereOrder = null)", tblShNa));
            sb.AppendLine(t2 + "{");

            sb.AppendLine(t3 + string.Format("List<{0}> list_{0} = new List<{0}>();", tblShNa));
            sb.AppendLine(t3 + "DataSet ds = new DataSet();");

            // connection block with open
            sb.Append(db.getConnectionBlock(true, t3));
            sb.AppendLine(t3 + string.Format("cm.CommandText = \"SELECT * FROM {0}\";", table.FQN));
            sb.AppendLine(t3 + "if (whereOrder != null) cm.CommandText += \" \" + whereOrder;");
            sb.AppendLine(t3 + "SqlDataAdapter da = new SqlDataAdapter(cm);");
            sb.AppendLine(t3 + "da.Fill(ds);");
            sb.AppendLine(t3 + "cm.Connection.Close();");


            sb.AppendLine(t3 + "foreach (DataRow row in ds.Tables[0].Rows)");
            sb.AppendLine(t3 + "{");
            sb.AppendLine(t4 + string.Format("list_{0}.Add({0}.Parse{0}(row));", tblShNa));
            sb.AppendLine(t3 + "}");
            sb.AppendLine(t3 + string.Format("return list_{0};", tblShNa));
            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }



        // Clean - Update method
        public static string getUpdateDeclarationText(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();

            sColumn column = table.Constraints
                .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            if (column == null) return "";

            string tblFuNa = table.Name;
            string tblShNa = table.Name;
            string ColNa = column.Name;
            string ColPTy = column.SqlDatatype.GetCsTypeString();

            sb.AppendLine(t2 + string.Format("public static Boolean Update{0}({0} _{0})", tblShNa));
            sb.AppendLine(t2 + "{");

            sb.AppendLine(t3 + string.Format("if (Get{0}By{1}(_{0}.{1}) != null)", tblShNa, ColNa));
            sb.AppendLine(t3 + "{");

            //connection block with open
            sb.Append(db.getConnectionBlock(true, t4));
            sb.AppendLine();

            StringBuilder cmdText = new StringBuilder();
            StringBuilder parms = new StringBuilder();

            // building command text start
            cmdText.AppendLine(t4 + string.Format("cm.CommandText = @\"UPDATE {0}", table.FQN));
            cmdText.AppendLine(t5 + "SET");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                sColumn col = table.Columns[i];
                if (col.IsIdentity) continue;

                string sfx = (i < table.Columns.Count - 1) ? "," : "";
                cmdText.AppendLine(t6 + string.Format("{0} = {1}{2}", col.FQN, col.getBasicParameterName(), sfx));
            }
            cmdText.AppendLine(t5 + string.Format("WHERE {0} = {1}\";", column.FQN, column.getBasicParameterName()));
            // building command text end

            foreach (sColumn col in table.Columns)
            {
                parms.AppendLine(t4 + col.getAndSetCommandParameter("cm", string.Format("_{0}", tblShNa), null));
            }

            sb.Append(cmdText.ToString());
            sb.AppendLine();
            sb.Append(parms.ToString());

            sb.AppendLine();
            sb.AppendLine(t4 + "Int32 count = cm.ExecuteNonQuery();");
            sb.AppendLine(t4 + "return (count == 1) ? true : false;");
            sb.AppendLine(t3 + "}");
            sb.AppendLine(t3 + "return false;");
            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }



        // Clean - Delete by id method
        public static string getDeleteByIdDeclarationText(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();

            // skip if has no primary key
            sColumn column = table.Constraints
                .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            if (column == null) return "";

            string ColPTy = column.SqlDatatype.GetCsTypeString();

            sb.AppendLine(t2 + string.Format("public static Boolean Delete{0}By{2}({1} {0}{2})", table.Name, ColPTy, column.Name));
            sb.AppendLine(t2 + "{");

            sb.AppendLine(t3 + string.Format("if (Get{0}By{1}({0}{1}) != null)", table.Name, column.Name));
            sb.AppendLine(t3 + "{");

            // connection block with open
            sb.Append(db.getConnectionBlock(true, t4));
            sb.AppendLine(t4 + string.Format("cm.CommandText = \"DELETE FROM {0} WHERE {1} = {2}\";", table.FQN, column.FQN, column.getBasicParameterName()));
            sb.AppendLine(t4 + column.getAndSetCommandParameter("cm", null, table.Name + column.Name));
            sb.AppendLine(t4 + "int count = cm.ExecuteNonQuery();");
            sb.AppendLine(t4 + "cm.Connection.Close();");
            sb.AppendLine(t4 + "return (count == 1) ? true : false;");

            sb.AppendLine(t3 + "}");
            sb.AppendLine(t3 + "return false;");
            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }



        #endregion



        #region General

        // Clean - Delete by id
        public static string getGetByIdDeclarationText(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();

            sColumn column = table.Constraints
                .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            if (column == null) return "";

            string tblShNa = table.Name;
            string ColNa = column.Name;
            string ColPTy = column.SqlDatatype.GetCsTypeString();

            sb.AppendLine(t2 + string.Format("public static {0} Get{0}By{2}({1} {0}{2})", tblShNa, ColPTy, ColNa));
            sb.AppendLine(t2 + "{");

            sb.AppendLine(t3 + string.Format("{0} _{0} = new {0}();", tblShNa));
            sb.AppendLine(t3 + "DataSet ds = new DataSet();");

            //connection block with open
            sb.Append(db.getConnectionBlock(true, t3));

            sb.AppendLine(t3 + string.Format("cm.CommandText = \"SELECT TOP 1 * FROM {0} WHERE {1} = {2}\";", table.FQN, column.FQN, column.getBasicParameterName()));
            sb.AppendLine(t3 + column.getAndSetCommandParameter("cm", null, tblShNa + ColNa));
            sb.AppendLine(t3 + "SqlDataAdapter da = new SqlDataAdapter(cm);");
            sb.AppendLine(t3 + "da.Fill(ds);");
            sb.AppendLine(t3 + "cm.Connection.Close();");
            sb.AppendLine(t3 + "if (ds.Tables[0].Rows.Count == 0) return null;");
            sb.AppendLine(t3 + string.Format("_{0} = Parse{0}(ds.Tables[0].Rows[0]);", tblShNa));
            sb.AppendLine(t3 + string.Format("return _{0};", tblShNa));
            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }



        // Clean - Table count
        public static string getGetCountDeclarationText(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();

            sColumn column = table.Constraints
                .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            if (column == null) return "";

            string tblShNa = table.Name;

            sb.AppendLine(t2 + string.Format("public static Int32 Get{0}Count()", tblShNa));
            sb.AppendLine(t2 + "{");

            // connection block with open
            sb.Append(db.getConnectionBlock(true, t3));
            sb.AppendLine(t3 + string.Format("cm.CommandText = \"SELECT COUNT(*) FROM {0}\";", table.FQN));
            sb.AppendLine(t3 + "Int32 count = (Int32)cm.ExecuteScalar();");
            sb.AppendLine(t3 + "cm.Connection.Close();");

            sb.AppendLine(t3 + "return count;");

            sb.AppendLine(t2 + "}");
            return sb.ToString();

        }



        // Clean - Advanced search
        public static string getAdvSearch(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();
            sColumn column = table.Constraints
                            .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            if (column == null) return "";

            var tblShNa = table.CsRefName;

            //////////sb.AppendLine(t2 + "public static DataTable AdvancedSearch(Int32 pageSize, Int32 requiredPage, String fullOrderClause = \"ORDER BY " + column.FQN + " ASC\", String fullWhereClause = \"\", String fullTextSearch = \"\", List<SqlParameter> parameters = null)");
            sb.AppendLine(t2 + "public static List<" + tblShNa + "> AdvancedSearch(Int32 pageSize = Int32.MaxValue, Int32 requiredPage = 1, String fullOrderClause = \"ORDER BY " + column.FQN + " ASC\", String fullWhereClause = \"\", String fullTextSearch = \"\", List<SqlParameter> parameters = null)");
            // open functions
            sb.AppendLine(t2 + "{");
            sb.AppendLine(t3 + "DataSet ds = new DataSet();");
            // connection block
            sb.Append(db.getConnectionBlock(true, t3));
            sb.AppendLine();
            // load parameters
            sb.AppendLine(t3 + "if (parameters != null)");
            sb.AppendLine(t3 + "{");
            sb.AppendLine(t4 + "cm.Parameters.AddRange(parameters.ToArray());");
            sb.AppendLine(t3 + "}");
            sb.AppendLine();

            sb.AppendLine(t3 + "string searchWhere = @\"WHERE");
            sb.AppendLine(t3 + "(");

            // get connected tables
            List<sConstraint> connections = table.Constraints.Where(c => c.ConstraintType == sEnum.SqlConstraintType.ForeignKey).ToList();
            // full text search
            {
                // the normal table columns
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sColumn col = table.Columns[i];

                    string sfx = (i < table.Columns.Count - 1 | connections.Count > 0) ? "OR" : "";
                    sb.AppendLine(t4 + string.Format("{0} LIKE '%' + @searchText + '%' {1}", col.FQN, sfx));
                }
                // to know we reached the last 
                int cc00 = connections.Count;
                // get pk tables and each will get the column name and make an alias for it
                int jtc00 = 0; // this to make alias for the joined tables
                foreach (sConstraint constraint in connections)
                {
                    cc00--;
                    jtc00++;
                    sTable linkedTable = constraint.PK_Table;
                    if (linkedTable == null) continue;
                    for (int i = 0; i < linkedTable.Columns.Count; i++)
                    {
                        sColumn col = linkedTable.Columns[i];
                        string sfx = (i == linkedTable.Columns.Count - 1 & cc00 == 0) ? "" : "OR";
                        sb.AppendLine(t4 + string.Format("jt{0}.{1} LIKE '%' + @searchText + '%' {2}", jtc00, col.Name, sfx));
                    }
                }
            }
            sb.AppendLine(t3 + ")\";");

            sb.AppendLine();
            sb.AppendLine(t3 + "if (FullTextSearch != \"\")");
            sb.AppendLine(t3 + "{");
            sb.AppendLine(t4 + "SqlParameter fullSearchParam = new SqlParameter(\"@searchText\", FullTextSearch.ToString());");
            sb.AppendLine(t4 + "cm.Parameters.Add(fullSearchParam);");
            sb.AppendLine(t4 + "fullWhereClause = searchWhere;");
            sb.AppendLine(t3 + "}");
            sb.AppendLine();

            // big work start
            sb.AppendLine(t3 + "cm.CommandText = String.Format(@\"SELECT");
            // get the normal select stuff, to be able to parse normally
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sColumn col = table.Columns[i];

                string sfx = (i < table.Columns.Count - 1 | connections.Count > 0) ? "," : "";
                sb.AppendLine(t4 + string.Format("{0}{1}", col.FQN, sfx));
            }

            // to know we reached the last 
            int count = connections.Count;
            // get pk tables and each will get the column name and make an alias for it
            int jtc = 0; // this to make alias for the joined tables
            foreach (sConstraint constraint in connections)
            {
                count--;
                jtc++;
                sTable linkedTable = constraint.PK_Table;
                if (linkedTable == null) continue;
                for (int i = 0; i < linkedTable.Columns.Count; i++)
                {
                    sColumn col = linkedTable.Columns[i];
                    string combcolname = db.Name + "_" + constraint.Name + "_" + col.Name;
                    string sfx = (i == linkedTable.Columns.Count - 1 & count == 0) ? "" : ",";
                    sb.AppendLine(t4 + string.Format("jt{0}.{1} AS '{2}'{3}", jtc, col.Name, combcolname, sfx));
                }
            }
            // the from table
            sb.AppendLine(t4 + String.Format("FROM {0}", table.FQN));
            // left joins
            jtc = 0; // this to make alias for the joined tables
            foreach (sConstraint constraint in connections)
            {
                jtc++;
                sTable linkedTable = constraint.PK_Table;
                if (linkedTable == null) continue;
                sb.AppendLine(t4 + string.Format("LEFT JOIN {0} AS jt{3} ON jt{3}.{1} = {2}", linkedTable.FQN, constraint.PK_Column.Name, constraint.Column.FQN, jtc));
            }
            // finalize the command
            sb.AppendLine(t4 + "{0} {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY\", fullWhereClause, fullOrderClause, pageSize * (requiredPage - 1), pageSize); ");

            // finishing
            sb.AppendLine(t3 + "SqlDataAdapter da = new SqlDataAdapter(cm);");
            sb.AppendLine(t3 + "da.Fill(ds);");
            sb.AppendLine(t3 + "cm.Connection.Close();");

            // return
            ////////sb.AppendLine(t3 + "return ds.Tables[0];");
            {
                sb.AppendLine(t3 + string.Format("List<{0}> list_{0} = new List<{0}>();", tblShNa));
                sb.AppendLine(t3 + "foreach (DataRow row in ds.Tables[0].Rows)");
                sb.AppendLine(t3 + "{");
                sb.AppendLine(t4 + string.Format("list_{0}.Add({0}.Parse{0}(row));", tblShNa));
                sb.AppendLine(t3 + "}");
                sb.AppendLine(t3 + string.Format("return list_{0};", tblShNa));
            }

            // close function
            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }



        // Clean - Advanced serach records count
        public static string getAdvSerachCountOnly(this sTable table, sDb db)
        {

            StringBuilder sb = new StringBuilder();
            sColumn column = table.Constraints
                            .Where(c => c.ConstraintType.Equals(sEnum.SqlConstraintType.PrimaryKey)).FirstOrDefault()?.Column;
            if (column == null) return "";

            sb.AppendLine(t2 + "public static Int32 AdvancedSearchRecordsCount(String fullWhereClause = \"\", String FullTextSearch = \"\", List<SqlParameter> parameters = null)");
            // open functions
            sb.AppendLine(t2 + "{");
            // connection
            sb.Append(db.getConnectionBlock(true, t3));
            sb.AppendLine();
            // load parameters
            sb.AppendLine(t3 + "if (parameters != null)");
            sb.AppendLine(t3 + "{");
            sb.AppendLine(t4 + "cm.Parameters.AddRange(parameters.ToArray());");
            sb.AppendLine(t3 + "}");
            sb.AppendLine();

            sb.AppendLine(t3 + "string searchWhere = @\"WHERE");
            sb.AppendLine(t3 + "(");

            // get connected tables
            List<sConstraint> connections = table.Constraints.Where(c => c.ConstraintType == sEnum.SqlConstraintType.ForeignKey).ToList();
            // full text search
            {
                // the normal table columns
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sColumn col = table.Columns[i];

                    string sfx = (i < table.Columns.Count - 1 | connections.Count > 0) ? "OR" : "";
                    sb.AppendLine(t4 + string.Format("{0} LIKE '%' + @searchText + '%' {1}", col.FQN, sfx));
                }
                // to know we reached the last 
                int cc00 = connections.Count;
                // get pk tables and each will get the column name and make an alias for it
                int jtc00 = 0; //this to make alias for the joined tables
                foreach (sConstraint constraint in connections)
                {
                    cc00--;
                    jtc00++;
                    sTable linkedTable = constraint.PK_Table;
                    if (linkedTable == null) continue;
                    for (int i = 0; i < linkedTable.Columns.Count; i++)
                    {
                        sColumn col = linkedTable.Columns[i];
                        string sfx = (i == linkedTable.Columns.Count - 1 & cc00 == 0) ? "" : "OR";
                        sb.AppendLine(t4 + string.Format("jt{0}.{1} LIKE '%' + @searchText + '%' {2}", jtc00, col.Name, sfx));
                    }
                }
            }
            sb.AppendLine(t3 + ")\";");

            sb.AppendLine();
            sb.AppendLine(t3 + "if (FullTextSearch != \"\")");
            sb.AppendLine(t3 + "{");
            sb.AppendLine(t4 + "SqlParameter fullSearchParam = new SqlParameter(\"@searchText\", FullTextSearch.ToString());");
            sb.AppendLine(t4 + "cm.Parameters.Add(fullSearchParam);");
            sb.AppendLine(t4 + "fullWhereClause = searchWhere;");
            sb.AppendLine(t3 + "}");
            sb.AppendLine();

            // start the big work
            sb.AppendLine(t3 + "cm.CommandText = String.Format(@\"SELECT COUNT(*)");
            // the from table
            sb.AppendLine(t4 + String.Format("FROM {0}", table.FQN));
            // left joins
            int jtc = 0; // this to make alias for the joined tables
            foreach (sConstraint constraint in connections)
            {
                jtc++;
                sTable linkedTable = constraint.PK_Table;
                if (linkedTable == null) continue;
                sb.AppendLine(t4 + string.Format("LEFT JOIN {0} AS jt{3} ON jt{3}.{1} = {2}", linkedTable.FQN, constraint.PK_Column.Name, constraint.Column.FQN, jtc));
            }
            // finalize the command
            sb.AppendLine(t4 + "{0}\", fullWhereClause); ");

            // execute and return
            sb.AppendLine(t3 + "Int32 count = (Int32)cm.ExecuteScalar();");
            sb.AppendLine(t3 + "cm.Connection.Close();");

            sb.AppendLine(t3 + "return count;");

            // close function
            sb.AppendLine(t2 + "}");
            return sb.ToString();

        }

        #endregion

    }
}
