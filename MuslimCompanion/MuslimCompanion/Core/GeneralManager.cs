using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MuslimCompanion.Core
{
    public static class GeneralManager
    {

        public static SQLiteConnection conn;

        public static async void InitConnection()
        {

            while (App.DatabaseLocation == null)
            {

                await Task.Delay(25);

            }

            conn = new SQLiteConnection(App.DatabaseLocation);

        }

    }
}
