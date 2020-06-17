
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
                case SqlDataType._varbinary: return CsDataType._Byte;
                case SqlDataType._bigint: return CsDataType._Int64;
                case SqlDataType._float: return CsDataType._Double;
                case SqlDataType._time: return CsDataType._TimeSpan;
                case SqlDataType._date: return CsDataType._DateTime;

                default: return CsDataType.None;
            }
        }

        //(extention) get proper text of the type for class property
        public static string GetCsTypeString(this SqlDataType sqlDataType)
        {
            //return N/A if it is not convertable
            if (sqlDataType.Equals(SqlDataType.None)) return "N/A";
            
            if (sqlDataType.GetCsDataType() == CsDataType._Byte) return "byte[]";
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
                case "datetime2": return SqlDataType._datetime;
                case "decimal": return SqlDataType._decimal;
                case "int": return SqlDataType._int;
                case "nvarchar": return SqlDataType._nvarchar;
                case "uniqueidentifier": return SqlDataType._uniqueidentifier;
                case "varchar": return SqlDataType._varchar;
                case "varbinary": return SqlDataType._varbinary;
                case "bigint": return SqlDataType._bigint;
                case "float": return SqlDataType._float;
                case "time": return SqlDataType._time;
                case "date": return SqlDataType._date;
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
            _varbinary,
            _char,
            _bit,
            _bigint,
            _float,
            _time,
            _date
        }

        public enum CsDataType
        {
            None = 0,
            _Int32,
            _Decimal,
            _Guid,
            _DateTime,
            _String,
            _Boolean,
            _Byte,
            _Int64,
            _Double,
            _TimeSpan
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
