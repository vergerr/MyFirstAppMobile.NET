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
        Task AddAsync(FitnessEntry entry);
        Task DeleteAsync(Guid id);
    }
}
