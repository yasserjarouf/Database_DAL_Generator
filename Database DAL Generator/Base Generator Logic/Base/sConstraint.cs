using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Generator_Logic
{
    public class sConstraint
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public sEnum.SqlConstraintType ConstraintType { get; set; }
        public sColumn Column { get; }
        public sTable PK_Table { get; set; }
        public sColumn PK_Column { get; set; }

        //constructor that needs a true referenced column not creating new!
        public sConstraint(sColumn column)
        {
            if (column == null | string.IsNullOrEmpty(column.Name?.Trim()) | column.SqlDatatype == sEnum.SqlDataType.None)
                throw new Exception("Column must be passed properly");
            Column = column;
        }
    }
}
