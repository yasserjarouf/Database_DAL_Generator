using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Generator_Logic
{
    public class sTable
    {
        public string Name { get; set; }
        public string SchemaName { get; set; }
        public List<sColumn> Columns { get; set; }
        public List<sConstraint> Constraints { get; set; }

        public string FQN
        {
            get
            {
                return string.Format("[{0}].[{1}]", SchemaName, Name);
            }
        }

        public bool HasIdentity()
        {
            return (Columns.Where(c => c.IsIdentity).Count() > 0);
        }

        //get name to be used as the table object name, while ignoring the text after a char
        //if not possible, get the whole name then
        public string GetStrippedNameBy(char chr)
        {
            int idx = Name.IndexOf(chr);
            if (idx < 1) return Name;
            return Name.Substring(0, idx);
        }
    }
}
