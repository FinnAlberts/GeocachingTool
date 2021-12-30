using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeocachingTool.Model
{
    /// <summary>
    /// A letter with a numerical value
    /// </summary>
    public class FormulaLetter
    {
        /// <summary>
        /// ID
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// The letter
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public float Value { get; set; }
    }
}
