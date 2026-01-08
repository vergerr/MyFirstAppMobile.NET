using MyFirstAppMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile.Data
{
    public class InMemoryEntriesRepository : IEntriesRepository
    {
        private readonly List<FitnessEntry> _items = new();

        public Task AddAsync(FitnessEntry entry)
        {
            _items.Add(entry);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _items.RemoveAll(x => x.Id == id);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<FitnessEntry>> GetAllAsync()
        => Task.FromResult((IReadOnlyList<FitnessEntry>)_items.ToList());
    }
}
