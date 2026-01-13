using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyFirstAppMobile.Data;
using MyFirstAppMobile.Models;
using System.Collections.ObjectModel;

namespace MyFirstAppMobile.ViewModels
{
    public partial class EntriesViewModel : ObservableObject
    {
        private readonly IEntriesRepository _repo;
        public ObservableCollection<FitnessEntry> Entries { get; } = new();

        public EntriesViewModel(IEntriesRepository repo)
        {
            _repo = repo;
            // Handler de réception du message venant du formulaire pour une nouvelel EntryFitness
            WeakReferenceMessenger.Default.Register<NewEntryMessage>(this, async (r, m) =>
            {
                await _repo.AddAsync(m.Value);
            });

            //Handler de réception pour modification d'un EntryFitness
            WeakReferenceMessenger.Default.Register<UpdateEntryMessage>(this, async (r, m) =>
            {
                await _repo.UpdateAsync(m.Value);

                var current = Entries.FirstOrDefault(x => x.Id == m.Value.Id);
                if (current is null) return;

                var idx = Entries.IndexOf(current);
                Entries[idx] = m.Value;

            });


        }

        private readonly SemaphoreSlim _semaphore = new(1, 1);

        private bool isLoading;
        public bool CanInteract => !isLoading;
        void OnIsLoadingChanged()
        {
            OnPropertyChanged(nameof(CanInteract));
        }

        [RelayCommand]
        public async Task Load()
        {
            if (!await _semaphore.WaitAsync(0))
                return;

            try
            {
                isLoading = true;
                var all = await _repo.GetAllAsync();
                ReplaceEntries(all);
            }
            finally
            {
                isLoading = false;
                _semaphore.Release();
            }
        }

        private CancellationTokenSource? _searchCts;

        [RelayCommand]
        public async Task Search(string search)
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;


            try
            {
                await Task.Delay(300, token); // debounce
                if (token.IsCancellationRequested)
                    return;

                if (!await _semaphore.WaitAsync(0))
                    return;
                try
                {
                    isLoading = true;
                    var all = await _repo.GetBySearchAsync(search);
                    ReplaceEntries(all);
                }
                finally
                {
                    isLoading = false;
                    _semaphore.Release();
                }
            }
            catch (TaskCanceledException)
            {
                // Ignore
            }
        }

        [RelayCommand]
        public async Task Add()
        {
            await Shell.Current.GoToAsync(nameof(EntryPage));
        }

        [RelayCommand]
        public async Task Edit(Guid id)
        {
            await Shell.Current.GoToAsync(nameof(EntryPage), new Dictionary<string, object>
            {
                ["EntryId"] = id
            });
        }

        [RelayCommand]
        public async Task Delete(Guid id)
        {
            await _repo.DeleteAsync(id);
            var fe = Entries.FirstOrDefault(e => e.Id == id);
            if (fe is null)
                return;
            Entries.Remove(fe);
        }

        [RelayCommand]
        // Méthode principale de filtrage
        public async Task ApplyFilter(string range)
        {
            DateTime now = DateTime.Now;
            var all = await _repo.GetAllAsync();

            if (range == "Week")
            {
                // Filtre : 7 derniers jours
                all = all.Where(i => i.Date >= now.AddDays(-7)).ToList();
            }
            else if (range == "Month")
            {
                // Filtre : même mois et même année
                all = all.Where(i => i.Date.Month == now.Month && i.Date.Year == now.Year).ToList();
            }

            // Mise à jour de la collection affichée
            ReplaceEntries(all);
        }

        public void ReplaceEntries(IEnumerable<FitnessEntry> entries)
        {
            Entries.Clear();
            foreach (var entry in entries)
            {
                Entries.Add(entry);
            }
        }
    }
}
