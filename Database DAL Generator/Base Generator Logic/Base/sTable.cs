using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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



        public string CsRefName
        {
            get
            {
                var rs = Regex.Replace(Name, @"\W", "_");
                rs = Regex.Replace(rs, @"_+", "_").Trim();

                // to fix the issue of conflict when table starts already with double underscores
                return Name.StartsWith("__") ? "_" + rs : rs;
            }
        }



        public bool HasIdentity()
        {
            return (Columns.Where(c => c.IsIdentity).Count() > 0);
        }

    }
}
