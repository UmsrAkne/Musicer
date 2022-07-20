namespace Musicer.Models.Databases
{
    using System.Data.SQLite;
    using System.IO;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public class ListenHistoryDbContext : DbContext
    {
        private string dbFileName = "listenHistory.sqlite";

        public DbSet<ListenHistory> Persons { get; set; }

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
