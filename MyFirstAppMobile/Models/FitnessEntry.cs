using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MyFirstAppMobile.Models
{
    public class FitnessEntry
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.Today;
        public string ActivityType { get; set; } = "";
        public int DurationMinutes { get; set; }
        public string Notes { get; set; } = "";
    }
}
