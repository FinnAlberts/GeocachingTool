using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeocachingTool.Model
{
    public class FormulaLetter
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Letter { get; set; }

        public int Value { get; set; }
    }
}
