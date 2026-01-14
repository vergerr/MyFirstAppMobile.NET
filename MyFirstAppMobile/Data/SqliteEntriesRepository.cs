using MyFirstAppMobile.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
