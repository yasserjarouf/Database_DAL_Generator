using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Generator_Logic
{
    public class sDb
    {
        public string Name { get; set; }
        public List<sTable> Tables { get; set; }

        public string getConnectionStringVariable()
        {
            return getCnString();
        }

        public string getDbNameSpace(string appname)
        {
            return string.Format("{0}.{1}.DAL.Database", appname.Trim(), Name.Trim());
        }

        public string getCnString()
        {
            return string.Format("_{0}.ConnectionString()", Name.Trim());
        }

        public string getConnectionBlock(bool withOpen, string tabs = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(tabs + "SqlConnection cn = new SqlConnection(" + getConnectionStringVariable() + ");");
            sb.AppendLine(tabs + "SqlCommand cm = new SqlCommand();");
            sb.AppendLine(tabs + "cm.Connection = cn;");
            if (withOpen) sb.AppendLine(tabs + "cm.Connection.Open();");

            return sb.ToString();
        }

    }
}
