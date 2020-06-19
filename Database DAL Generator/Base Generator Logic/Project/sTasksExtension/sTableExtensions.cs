using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Generator_Logic.Extensions
{
    public static class sTableExtensions
    {

        const string t1 = "\t";
        const string t2 = "\t\t";
        const string t3 = "\t\t\t";
        const string t4 = "\t\t\t\t";
        const string t5 = "\t\t\t\t\t";
        const string t6 = "\t\t\t\t\t\t";



        // Clean - get all properties declaration text with navigational object
        public static string getPropertiesDeclarationText(this sTable table)
        {

            StringBuilder st = new StringBuilder();

            foreach (sColumn column in table.Columns)
            {
                // normal property
                st.AppendLine(t2 + column.getCsNormalPropertyDeclarationText());

                // get also if there is a linked object
                if (column.checkIfColumnLinkedWithAnotherTable())
                    st.AppendLine(t2 + column.getCsPropertyDeclarationTextForLinkedTable()); ;
            }

            return st.ToString();

        }



        // Clean - get the parse function string
        public static string getParseDeclarationText(this sTable table)
        {

            StringBuilder sb = new StringBuilder();

            // declare
            sb.AppendLine(t2 + string.Format("public static {0} Parse{0}(DataRow row)", table.CsRefName));
            // open
            sb.AppendLine(t2 + "{");

            // get nullable columns
            var nullbs = table.Columns.Where(c => c.IsNullable & !c.getCsDataType().Equals(sEnum.CsDataType._String) & !c.getCsDataType().Equals(sEnum.CsDataType._Byte));

            // process the median nullable types
            foreach (sColumn column in nullbs)
            {
                sb.AppendLine(string.Format(t3 + "{0} __{1} = null;", column.getCsAndIfNullTypeWithIgnoreString(), column.CsRefName));
                sb.AppendLine(string.Format(t3 + "if (!row.IsNull(\"{0}\")) __{3} = {1}.Parse(row[\"{2}\"].ToString());", column.Name, column.SqlDatatype.GetCsTypeString(), column.Name, column.CsRefName));
            }

            // new empty line if there are nullables
            if (nullbs.Count() > 0)
                sb.AppendLine();

            // return object creation
            sb.AppendLine(string.Format(t3 + "{0} _{0} = new {0}()", table.CsRefName));
            sb.AppendLine(t3 + "{");

            // output properties values assignment
            foreach (sColumn column in table.Columns)
            {
                // string is special case
                if (column.getCsDataType().Equals(sEnum.CsDataType._String))
                {
                    sb.AppendLine(t4 + string.Format("{0} = row[\"{1}\"].ToString(),", column.CsRefName, column.Name));
                }
                else if (column.getCsDataType().Equals(sEnum.CsDataType._Byte))
                {
                    sb.AppendLine(t4 + string.Format("{0} = (byte[])row[\"{1}\"],", column.CsRefName, column.Name));
                }
                else if (column.getCsDataType().Equals(sEnum.CsDataType._TimeSpan))
                {
                    if (column.IsNullable)
                        sb.AppendLine(t4 + string.Format("{0} = __{0},", column.CsRefName));

                    else
                        sb.AppendLine(t4 + string.Format("{0} = (TimeSpan)row[\"{1}\"],", column.CsRefName, column.Name));

                }
                else
                {
                    if (column.IsNullable)
                        sb.AppendLine(t4 + string.Format("{0} = __{0},", column.CsRefName));

                    else
                        sb.AppendLine(t4 + string.Format("{0} = {1}.Parse(row[\"{2}\"].ToString()),", column.CsRefName, column.SqlDatatype.GetCsTypeString(), column.Name));
                }
            }

            // close the object declare and new line
            sb.AppendLine(t3 + "};");
            sb.AppendLine();

            // the return clause
            sb.AppendLine(t3 + string.Format("return _{0};", table.CsRefName));

            // close
            sb.AppendLine(t2 + "}");

            return sb.ToString();

        }

    }
}
