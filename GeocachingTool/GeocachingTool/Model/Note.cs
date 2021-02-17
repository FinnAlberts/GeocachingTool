using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeocachingTool.Model
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }
    }
}
