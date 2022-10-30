using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Musicer.Models.Sounds;

namespace Musicer.Models.Databases
{
    public class ListenHistoryDbContext : DbContext
    {
        private string dbFileName = "listenHistory.sqlite";

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private DbSet<ListenHistory> ListenHistories { get; set; }

        private DbSet<SoundData> Sounds { get; set; }

        public void Save(ISound sound)
        {
            var soundData = Sounds.FirstOrDefault(s => s.FullName == sound.FullName);

            if (soundData == null)
            {
                soundData = new SoundData() { FullName = sound.FullName, Name = sound.Name, };
                Sounds.Add(soundData);
            }

            ListenHistories.Add(new ListenHistory()
            {
                ListenDateTime = DateTime.Now,
                SoundDataId = soundData.Id,
            });

            SaveChanges();
        }

        public List<ListenHistory> GetHistories(int takeCount)
        {
            return new List<ListenHistory>();

            // Todo
            // return ListenHistories.Where(l => true)
            //     .OrderByDescending(l => l.LastListenDateTime)
            //     .Take(takeCount)
            //     .ToList();
        }

        public int GetListenCount(string fullName)
        {
            if (!Sounds.Any(s => s.FullName == fullName))
            {
                return 0;
            }

            var id = Sounds.First(s => s.FullName == fullName).Id;
            return ListenHistories.Count(l => l.SoundDataId == id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName); // ファイルが存在している場合は問答無用で上書き。
            }

            var connectionString = new SqliteConnectionStringBuilder { DataSource = dbFileName }.ToString();
            optionsBuilder.UseSqlite(new SQLiteConnection(connectionString));
        }
    }
}