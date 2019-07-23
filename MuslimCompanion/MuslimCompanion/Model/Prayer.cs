using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MuslimCompanion.Model
{
    public class Prayers
    {

        [NotNull, PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string PrayerType { set; get; }

        [NotNull]
        public DateTime FireTime { set; get; }

        [NotNull]
        public int ScheduledToFire { set; get; }

        [NotNull]
        public int Fired { set; get; }

    }
}
