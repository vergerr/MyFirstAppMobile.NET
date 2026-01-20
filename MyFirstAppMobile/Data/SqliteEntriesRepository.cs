using MyFirstAppMobile.Models;
using SQLite;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MyFirstAppMobile.Tests")]

namespace MyFirstAppMobile.Data
{
    internal class SqliteEntriesRepository : IEntriesRepository
    {
        private readonly SQLiteAsyncConnection _db;
        private bool _initialized;

        public SqliteEntriesRepository(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
        }

        private async Task InitAsync()
        {
            if (_initialized) return;
            await _db.CreateTableAsync<FitnessEntry>();
            _initialized = true;
        }
        public async Task AddAsync(FitnessEntry entry)
        {
            await InitAsync();
            await _db.InsertAsync(entry);
        }

        public async Task DeleteAsync(Guid id)
        {
            await InitAsync();
            await _db.DeleteAsync<FitnessEntry>(id);
        }

        public async Task<IReadOnlyList<FitnessEntry>> GetAllAsync()
        {
            await InitAsync();
            return await _db.Table<FitnessEntry>()
                            .OrderByDescending(x => x.Date)
                            .ToListAsync();
        }

        public async Task UpdateAsync(FitnessEntry entry)
        {
            await InitAsync();
            await  _db.UpdateAsync(entry);
        }

        public async Task<FitnessEntry?> GetByIdAsync(Guid id)
        {
            await InitAsync();
            return await _db.Table<FitnessEntry>()
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<FitnessEntry>> GetBySearchAsync(string search)
        {
            await InitAsync();
            if (string.IsNullOrEmpty(search))
                return await GetAllAsync();
            var pattern = search.Trim().ToLower();
            return await _db.Table<FitnessEntry>()
                            .Where(x =>  x.ActivityType.ToLower().Contains(pattern) || x.Notes.ToLower().Contains(pattern))
                            .OrderByDescending(x => x.Date)
                            .ToListAsync();
                            
        }

        public async Task<IReadOnlyList<FitnessEntry>> GetFilteredEntriesAsync(string search, string filter, string sort)
        {
            var all = await this.GetAllAsync();
            var now = DateTime.Now;

            if (filter == "Semaine")
                all = all.Where(i => i.Date >= now.AddDays(-7)).ToList();
            else if (filter == "Mois")
                all = all.Where(i => i.Date.Month == now.Month && i.Date.Year == now.Year).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var p = search.Trim();
                all = all.Where(x =>
                    (x.ActivityType?.Contains(p, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Notes?.Contains(p, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            IEnumerable<FitnessEntry> ordered = sort switch
            {
                "Date (Desc)" => all.OrderByDescending(i => i.Date),
                "Date (Asc)" => all.OrderBy(i => i.Date),
                "Duration (Desc)" => all.OrderByDescending(i => i.DurationMinutes),
                "Duration (Asc)" => all.OrderBy(i => i.DurationMinutes),
                _ => all
            };
            return all;
        }
    }
}
