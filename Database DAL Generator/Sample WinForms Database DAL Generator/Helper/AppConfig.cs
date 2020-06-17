using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample_WinForms_Database_DAL_Generator.Helper
{
    public class AppConfig
    {
        public List<string> Usings { get; set; }

        public string OutputDirectory { get; set; }

        public string NamespacePrefix { get; set; }

        public bool IgnoreTableSchema { get; set; }

        public string OutputFilesExtension { get; set; }

        public string DbName { get; set; }
        public string AppName { get; set; }

        public string ConnectionString { get; set; }

    }
}
