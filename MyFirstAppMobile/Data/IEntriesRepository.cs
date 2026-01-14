using MyFirstAppMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile.Data
{
    public interface IEntriesRepository
    {
        Task<IReadOnlyList<FitnessEntry>> GetAllAsync();
        Task<IReadOnlyList<FitnessEntry>> GetBySearchAsync(string search);
        Task AddAsync(FitnessEntry entry);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(FitnessEntry entry);
        Task<FitnessEntry?> GetByIdAsync(Guid id);
    }
}
