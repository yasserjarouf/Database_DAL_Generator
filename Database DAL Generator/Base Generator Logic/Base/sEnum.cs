
namespace Base_Generator_Logic
{
    public static class sEnum
    {
        //(extention) get the proper Cs enum of the SqlDataType enum value
        public static CsDataType GetCsDataType(this SqlDataType sqlDataType)
        {
            switch (sqlDataType)
            {
                case SqlDataType.None: return CsDataType.None;
                case SqlDataType._int: return CsDataType._Int32;
                case SqlDataType._decimal: return CsDataType._Decimal;
                case SqlDataType._uniqueidentifier: return CsDataType._Guid;
                case SqlDataType._datetime: return CsDataType._DateTime;
                case SqlDataType._nvarchar: return CsDataType._String;
                case SqlDataType._varchar: return CsDataType._String;
                case SqlDataType._char: return CsDataType._String;
                case SqlDataType._bit: return CsDataType._Boolean;

                default: return CsDataType.None;
            }
        }

        //(extention) get proper text of the type for class property
        public static string GetCsTypeString(this SqlDataType sqlDataType)
        {
            //return N/A if it is not convertable
            if (sqlDataType.Equals(SqlDataType.None)) return "N/A";

            //return the converted value but remove the '_' char which is the first letter by using substring
            else return sqlDataType.GetCsDataType().ToString().Substring(1);
        }

        public static SqlDataType ParseStringToSqlDataType(string typeString)
        {
            switch (typeString)
            {
                case "bit": return SqlDataType._bit;
                case "char": return SqlDataType._char;
                case "datetime": return SqlDataType._datetime;
                case "decimal": return SqlDataType._decimal;
                case "int": return SqlDataType._int;
                case "nvarchar": return SqlDataType._nvarchar;
                case "uniqueidentifier": return SqlDataType._uniqueidentifier;
                case "varchar": return SqlDataType._varchar;
                default: return SqlDataType.None;
            }
        }
        
        public enum SqlDataType
        {
            None = 0,
            _int,
            _decimal,
            _uniqueidentifier,
            _datetime,
            _nvarchar,
            _varchar,
            _char,
            _bit
        }

        public enum CsDataType
        {
            None = 0,
            _Int32,
            _Decimal,
            _Guid,
            _DateTime,
            _String,
            _Boolean
        }

        public enum SqlConstraintType
        {
            PrimaryKey,
            ForeignKey,
        }

        public enum SqlOption
        {
            MaxChar,
            NumScale,
            NumPrecision
        }
    }
}
