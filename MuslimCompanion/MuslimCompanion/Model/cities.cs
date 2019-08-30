using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MuslimCompanion.Model
{
    public class cities
    {

        public int cityId { get; set; }
        public string nameEN { get; set; }
        public string nameAR { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int altitude { get; set; }
        public string countryCode { get; set; }
        public int tzld { get; set; }

    }
}
