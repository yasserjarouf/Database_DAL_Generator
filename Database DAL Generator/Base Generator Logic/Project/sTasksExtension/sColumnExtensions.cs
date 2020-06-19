using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Generator_Logic.Extensions
{
    public static class sColumnExtensions
    {

        // Clean - little code to get the param name of the column like @p_Id
        public static string getBasicParameterName(this sColumn column)
        {
            return "@p_" + column.Name;
        }



        // Clean - to directly get the csdatatype enum
        public static sEnum.CsDataType getCsDataType(this sColumn column)
        {
            return column.SqlDatatype.GetCsDataType();
        }



        // Clean - get the complete single declaration line of normal property, no constraint involved
        public static string getCsNormalPropertyDeclarationText(this sColumn column)
        {
            string type = getCsFinalTypeStringWithNullable(column);

            string name = column.CsRefName;

            string output = string.Format("public {0} {1} {{ get; set; }}", type, name);
            return output;
        }



        // Clean - get the proper cs type consedering nullablitiy but not the string one since string can't be nullable
        public static string getCsAndIfNullTypeWithIgnoreString(this sColumn column)
        {
            string type = column.SqlDatatype.GetCsTypeString();
            if (column.IsNullable) return type + "?";
            else return type;
        }



        // Clean - check if the column is a foreign key
        public static bool checkIfColumnLinkedWithAnotherTable(this sColumn column)
        {
            // true if the table has a foreign key and it is our column
            return (column.Table.Constraints.Where(c => c.ConstraintType
                                .Equals(sEnum.SqlConstraintType.ForeignKey) & c.Column.Name == column.Name)
                                .Count() == 1);
        }



        // Clean - get the declaration line to get the linked object
        public static string getCsPropertyDeclarationTextForLinkedTable(this sColumn column)
        {
            // finding a constraint is a must
            sConstraint constraint = column.Table.Constraints.Where(c => c.Column == column).First();

            string objName = constraint.Name.Split('.').Last();
            string objType = constraint.PK_Table.Name;
            string paramName = column.Name;
            string paramType = column.SqlDatatype.GetCsTypeString();

            string r = string.Format("public {0} {1}() {{ return {0}.Get{0}ById(({3}){2}); }}", objType, objName, paramName, paramType);
            return r;
        }



        // Clean - get the final type string with checking nullability also, like "Int32?"
        public static string getCsFinalTypeStringWithNullable(sColumn column)
        {
            string type = column.SqlDatatype.GetCsTypeString();
            sEnum.CsDataType csdt = column.SqlDatatype.GetCsDataType();

            if (column.IsNullable &
                !csdt.Equals(sEnum.CsDataType._String) &
                !csdt.Equals(sEnum.CsDataType._Byte) &
                !csdt.Equals(sEnum.CsDataType.None))
            {
                return type + "?";
            }
            else
            {
                return type;
            }
        }



        // Clean - automate the parameter line thing
        public static string getAndSetCommandParameter(this sColumn column, string commandName, string parent = null, string propNa = null)
        {
            // propNa means if we want to set custom property name or else use the column name
            string prnt = (parent == null) ? "" : parent + ".";
            string colNa = (propNa == null) ? column.Name : propNa;

            string tp, eq;
            int len;

            // sample: _Contact_m.FirstName == null ? (Object)DBNull.Value : _Contact_m.FirstName;
            switch (column.SqlDatatype)
            {
                case sEnum.SqlDataType._nvarchar:
                    tp = "SqlDbType.NVarChar";
                    eq = string.Format("{0}{1} == null ? (Object)DBNull.Value : {0}{1}", prnt, colNa);
                    len = column.Options.Where(o => o.Item1 == sEnum.SqlOption.MaxChar).First().Item2;
                    return string.Format("{0}.Parameters.Add(\"{1}\", {2}, {3}).Value = {4};", commandName, column.getBasicParameterName(), tp, len, eq);

                case sEnum.SqlDataType._int:
                    tp = "SqlDbType.Int";
                    if (column.IsNullable)
                        eq = string.Format("{0}{1} == null ? (Object)DBNull.Value : {0}{1}", prnt, colNa);
                    else
                        eq = string.Format("{0}{1}", prnt, colNa);
                    return string.Format("{0}.Parameters.Add(\"{1}\", {2}).Value = {3};", commandName, column.getBasicParameterName(), tp, eq);

                case sEnum.SqlDataType._uniqueidentifier:
                    tp = "SqlDbType.UniqueIdentifier";
                    if (column.IsNullable)
                        eq = string.Format("{0}{1} == null ? (Object)DBNull.Value : {0}{1}", prnt, colNa);
                    else
                        eq = string.Format("{0}{1}", prnt, colNa);
                    return string.Format("{0}.Parameters.Add(\"{1}\", {2}).Value = {3};", commandName, column.getBasicParameterName(), tp, eq);

                case sEnum.SqlDataType._datetime:
                    tp = "SqlDbType.DateTime";
                    if (column.IsNullable)
                        eq = string.Format("{0}{1} == null ? (Object)DBNull.Value : {0}{1}", prnt, colNa);
                    else
                        eq = string.Format("{0}{1}", prnt, colNa);
                    return string.Format("{0}.Parameters.Add(\"{1}\", {2}).Value = {3};", commandName, column.getBasicParameterName(), tp, eq);

                case sEnum.SqlDataType._bit:
                    tp = "SqlDbType.Bit";
                    if (column.IsNullable)
                        eq = string.Format("{0}{1} == null ? (Object)DBNull.Value : {0}{1}", prnt, colNa);
                    else
                        eq = string.Format("{0}{1}", prnt, colNa);
                    return string.Format("{0}.Parameters.Add(\"{1}\", {2}).Value = {3};", commandName, column.getBasicParameterName(), tp, eq);

                case sEnum.SqlDataType._decimal:
                    tp = "SqlDbType.Decimal";
                    if (column.IsNullable)
                        eq = string.Format("{0}{1} == null ? (Object)DBNull.Value : {0}{1}", prnt, colNa);
                    else
                        eq = string.Format("{0}{1}", prnt, colNa);
                    return string.Format("{0}.Parameters.Add(\"{1}\", {2}).Value = {3};", commandName, column.getBasicParameterName(), tp, eq);

                case sEnum.SqlDataType._char:
                    tp = "SqlDbType.Char";
                    if (column.IsNullable)
                        eq = string.Format("{0}{1} == null ? (Object)DBNull.Value : {0}{1}", prnt, colNa);
                    else
                        eq = string.Format("{0}{1}", prnt, colNa);
                    return string.Format("{0}.Parameters.Add(\"{1}\", {2}).Value = {3};", commandName, column.getBasicParameterName(), tp, eq);

                case sEnum.SqlDataType._varbinary:
                    tp = "SqlDbType.Binary";
                    if (column.IsNullable)
                        eq = string.Format("{0}{1} == null ? (Object)DBNull.Value : {0}{1}", prnt, colNa);
                    else
                        eq = string.Format("{0}{1}", prnt, colNa);
                    return string.Format("{0}.Parameters.Add(\"{1}\", {2}).Value = {3};", commandName, column.getBasicParameterName(), tp, eq);
            }

            return "";
        }

    }
}
