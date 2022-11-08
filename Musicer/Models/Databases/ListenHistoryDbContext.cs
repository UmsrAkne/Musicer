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

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private DbSet<SoundData> Sounds { get; set; }

        public void AddListenCount(ISound sound)
        {
            SaveSoundData(sound);

            // 視聴履歴の記録
            var soundData = Sounds.First(s => sound.FullName == s.FullName);
            ListenHistories.Add(new ListenHistory()
            {
                ListenDateTime = DateTime.Now,
                SoundDataId = soundData.Id,
            });

            SaveChanges();
        }

        public void UpdateSoundDuration(ISound sound)
        {
            SaveSoundData(sound);
            var snd = Sounds.First(s => s.FullName == sound.FullName);
            snd.PlaybackTimeTicks = sound.Duration.Ticks;
            SaveChanges();
        }

        public void SaveSoundData(ISound sound)
        {
            // サウンドの情報を記録
            if (!Sounds.Any(s => s.FullName == sound.FullName))
            {
                Sounds.Add(new SoundData() { FullName = sound.FullName, Name = sound.Name });
                SaveChanges();
            }
        }

        public SoundData GetSoundData(string fullName)
        {
            return Sounds.FirstOrDefault(s => s.FullName == fullName);
        }

        public List<ListenHistory> GetHistories(int takeCount)
        {
            var listenHistories = ListenHistories.OrderBy(l => l.ListenDateTime).Take(takeCount);
            var soundListenHistories = new List<ListenHistory>();
            var index = 1;

            foreach (var l in listenHistories)
            {
                var currentData = Sounds.FirstOrDefault(s => s.Id == l.SoundDataId);
                if (currentData != null)
                {
                    l.Name = currentData.Name;
                    var fileInfo = new FileInfo(currentData.FullName);
                    l.ParentDirectoryName = fileInfo.Directory?.Name;
                }

                l.ListenCount = ListenHistories.Count(lh => l.SoundDataId == lh.SoundDataId);
                l.Index = index++;
                soundListenHistories.Add(l);
            }

            return soundListenHistories;
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