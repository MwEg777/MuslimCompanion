using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace MuslimCompanion.Model
{
    public class Quran
    {

        [NotNull, PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public int DatabaseID { get; set; }

        [NotNull]
        public int SuraID { get; set; }

        [NotNull]
        public int VerseID { get; set; }

        [NotNull]
        public string AyahText { get; set; }

    }
}
