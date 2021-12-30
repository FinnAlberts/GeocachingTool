using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeocachingTool.Model
{
    /// <summary>
    /// A note
    /// </summary>
    public class Note
    {
        /// <summary>
        /// ID
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Name of the note
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contents of the note
        /// </summary>
        public string Details { get; set; }
    }
}
