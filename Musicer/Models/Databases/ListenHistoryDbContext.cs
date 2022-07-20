namespace Musicer.Models.Databases
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Musicer.Models.Sounds;

    public class ListenHistoryDbContext : DbContext
    {
        private string dbFileName = "listenHistory.sqlite";

        private DbSet<ListenHistory> ListenHistories { get; set; }

        public void Save(ISound sound)
        {
            var history = ListenHistories.FirstOrDefault(l => l.FullName == sound.FullName);

            if (history == null)
            {
                ListenHistories.Add(
                    new ListenHistory()
                    {
                        FullName = sound.FullName,
                        Name = sound.Name,
                        LastListenDateTime = DateTime.Now,
                        ListenCount = 1,
                    });
            }
            else
            {
                history.ListenCount++;
                history.LastListenDateTime = DateTime.Now;
            }

            SaveChanges();
        }

        public List<ListenHistory> GetAll()
        {
            return ListenHistories.Where(l => true)
                .OrderByDescending(l => l.LastListenDateTime)
                .ToList();
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
