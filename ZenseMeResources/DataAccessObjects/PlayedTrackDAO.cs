using System;
using System.Data;
using System.Data.SQLite;
using ZenseMe.Lib.Storage;

namespace ZenseMe.Lib.DataAccessObjects
{
    public class PlayedTrackDAO
    {
        private Database _hDatabase;

        public PlayedTrackDAO()
        {
            _hDatabase = new Database();
        }

        private bool ObjectExists(string persistentId)
        {
            string sqlQuery = "SELECT * FROM device_tracks WHERE persistent_id = @persistentId";
            DataSet dataSet = _hDatabase.Fetch(sqlQuery, new SQLiteParameter("@persistentId", persistentId));

            if (dataSet.Tables[0].Rows.Count == 0)
            {
                Console.WriteLine("PersistentId does not exist.");
                return false;
            }
            return true;
        }

        public void SaveObject(string persistentId, DateTime dateSubmitted, int PlayCount)
        {
            if (!ObjectExists(persistentId))
            {
                string sqlQuery = "INSERT INTO device_tracks WHERE persistent_id = @persistentId ("
                        + "[play_count_his],"
                        + "[date_submitted]"
                        + ") VALUES ("
                        + "@PlayCount,"
                        + "@dateSubmitted)";

                _hDatabase.Execute(sqlQuery,
                    new SQLiteParameter("@persistentId", persistentId),
                    new SQLiteParameter("@PlayCount", PlayCount),
                    new SQLiteParameter("@dateSubmitted", dateSubmitted));
            }
            else
            {
                string sqlQueryUpdate = "UPDATE device_tracks SET play_count_his = @PlayCount, date_submitted = @dateSubmitted WHERE persistent_id = @persistentId";
                _hDatabase.Execute(sqlQueryUpdate,
                    new SQLiteParameter("@persistentId", persistentId),
                    new SQLiteParameter("@PlayCount", PlayCount),
                    new SQLiteParameter("@dateSubmitted", dateSubmitted)
                );
            }
        }
    }
}