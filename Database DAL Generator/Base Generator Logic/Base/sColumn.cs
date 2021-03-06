﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Base_Generator_Logic
{
    public class sColumn
    {

        public string Name { get; set; }

        public sEnum.SqlDataType SqlDatatype { get; set; }

        public bool IsNullable { get; set; }

        public bool IsIdentity { get; set; }

        public int Position { get; set; }

        public List<Tuple<sEnum.SqlOption, dynamic>> Options { get; set; }

        public sTable Table { get; }



        // constructor that needs a true referenced column
        public sColumn(sTable table)
        {
            if (table == null | string.IsNullOrEmpty(table.Name?.Trim()))
                throw new Exception("Table must be passed properly");
            Table = table;
        }



        public string FQN
        {
            get
            {
                return string.Format("[{0}].[{1}].[{2}]", Table.SchemaName, Table.Name, Name);
            }
        }



        public string CsRefName
        {
            get
            {
                var rs = Regex.Replace(Name, @"\W", "_");
                return Regex.Replace(rs, @"_+", "_").Trim();
            }
        }

    }
}
