using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using ZenseMe.Lib.Objects;
using ZenseMe.Lib.Storage;

namespace ZenseMe.Lib.DataAccessObjects
{
    public class IgnoreTrackDAO
    {
        private Database _hDatabase;
        private List<EntryObject> ConfirmScrobbleList;

        public IgnoreTrackDAO()
        {
            _hDatabase = new Database();
        }

        public void IgnoreTrack(List<EntryObject> entries)
        {
            ConfirmScrobbleList = entries;

            if (ConfirmScrobbleList != null && ConfirmScrobbleList.Count > 0)
            {
                foreach (EntryObject track in ConfirmScrobbleList)
                {
                    Console.WriteLine("Adding track to Ignore list");
                    if (!ObjectExists(track.PersistentId))
                    {
                        string sqlQuery = "INSERT INTO device_tracks WHERE persistent_id = @persistentId ("
                                + "[ignored],"
                                + ") VALUES ("
                                + "@ignored)";

                        _hDatabase.Execute(sqlQuery,
                            new SQLiteParameter("@persistentId", track.PersistentId),
                            new SQLiteParameter("@ignored", "1"));
                    }
                    else
                    {
                        string sqlQueryUpdate = "UPDATE device_tracks SET ignored = @ignored WHERE persistent_id = @persistentId";
                        _hDatabase.Execute(sqlQueryUpdate,
                            new SQLiteParameter("@persistentId", track.PersistentId),
                            new SQLiteParameter("@ignored", "1"));
                    }
                }
            }
        }

        public void UnignoreTrack(List<EntryObject> entries)
        {
            ConfirmScrobbleList = entries;

            if (ConfirmScrobbleList != null && ConfirmScrobbleList.Count > 0)
            {
                foreach (EntryObject track in ConfirmScrobbleList)
                {
                    Console.WriteLine("Adding track to Recently Played list");
                    if (ObjectExists(track.PersistentId))
                    {
                        string sqlQueryUpdate = "UPDATE device_tracks SET ignored = @ignored WHERE persistent_id = @persistentId";
                        _hDatabase.Execute(sqlQueryUpdate,
                            new SQLiteParameter("@persistentId", track.PersistentId),
                            new SQLiteParameter("@ignored", "0"));
                    }
                }
            }
        }

        public void SkipTrack(List<EntryObject> entries)
        {
            ConfirmScrobbleList = entries;

            if (ConfirmScrobbleList != null && ConfirmScrobbleList.Count > 0)
            {
                foreach (EntryObject track in ConfirmScrobbleList)
                {
                    Console.WriteLine("Skipping selected track.");
                    if (ObjectExists(track.PersistentId))
                    {
                        string sqlQueryUpdate = "UPDATE device_tracks SET play_count_his = @play_count WHERE persistent_id = @persistentId";
                        _hDatabase.Execute(sqlQueryUpdate,
                            new SQLiteParameter("@persistentId", track.PersistentId),
                            new SQLiteParameter("@play_count", track.PlayCount));
                    }
                }
            }
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
    }
}