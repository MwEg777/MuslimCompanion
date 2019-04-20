using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace MuslimCompanion.Model
{
    public class QuranNoTashkeel
    {

        [NotNull, PrimaryKey]
        public int gid { get; set; }

        [NotNull]
        public int sid { get; set; }

        [NotNull]
        public int aid { get; set; }

        [NotNull]
        public string aya { get; set; }

    }
}
