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

        // Liste des options pour le Picker
        public List<string> SortOptions { get; } = new List<string>
    {
        "Date (Desc)",
        "Date (Asc)",
        "Duration (Desc)",
        "Duration (Asc)"
    };

        public List<string> FilterOptions { get; } = new List<string>
    {
        "Tout",
        "Semaine",
        "Mois"
    };

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    OnPropertyChanged();
                    _ = Refresh();
                }
            }
        }

        private string _selectedSort;
        public string SelectedSort
        {
            get => _selectedSort;
            set
            {
                if (_selectedSort != value)
                {
                    _selectedSort = value;
                    OnPropertyChanged();
                    _ = Refresh(); ; // On retrie dès que la sélection change
                }
            }
        }

        public EntriesViewModel(IEntriesRepository repo)
        {
            _repo = repo;
            // Handler de réception du message venant du formulaire pour une nouvelel EntryFitness
            WeakReferenceMessenger.Default.Register<NewEntryMessage>(this, async (r, m) =>
            {
                await _repo.AddAsync(m.Value);
                await Refresh();
            });

            //Handler de réception pour modification d'un EntryFitness
            WeakReferenceMessenger.Default.Register<UpdateEntryMessage>(this, async (r, m) =>
            {
                await _repo.UpdateAsync(m.Value);
                await Refresh();
            });
        }

        private readonly SemaphoreSlim _semaphore = new(1, 1);

        [ObservableProperty]
        private bool isLoading;
        public bool CanInteract => !IsLoading;
        partial void OnIsLoadingChanged(bool value)
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
                IsLoading = true;
                var all = await _repo.GetAllAsync();
                ReplaceEntries(all);
            }
            finally
            {
                IsLoading = false;
                _semaphore.Release();
            }
        }

        private string _currentSearch = string.Empty;

        public async Task Refresh()
        {
            if (!await _semaphore.WaitAsync(0)) return;
            try
            {
                IsLoading = true;

                var all = await _repo.GetAllAsync();
                var now = DateTime.Now;

                if (_selectedFilter == "Semaine")
                    all = all.Where(i => i.Date >= now.AddDays(-7)).ToList();
                else if (_selectedFilter == "Mois")
                    all = all.Where(i => i.Date.Month == now.Month && i.Date.Year == now.Year).ToList();

                if (!string.IsNullOrWhiteSpace(_currentSearch))
                {
                    var p = _currentSearch.Trim();
                    all = all.Where(x =>
                        (x.ActivityType?.Contains(p, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (x.Notes?.Contains(p, StringComparison.OrdinalIgnoreCase) ?? false)
                    ).ToList();
                }

                IEnumerable<FitnessEntry> ordered = SelectedSort switch
                {
                    "Date (Desc)" => all.OrderByDescending(i => i.Date),
                    "Date (Asc)" => all.OrderBy(i => i.Date),
                    "Duration (Desc)" => all.OrderByDescending(i => i.DurationMinutes),
                    "Duration (Asc)" => all.OrderBy(i => i.DurationMinutes),
                    _ => all
                };

                ReplaceEntries(ordered);
            }
            finally
            {
                IsLoading = false;
                _semaphore.Release();
            }
        }

        #region Commands

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
        public async Task ShowDetails(Guid id)
        {
            await Shell.Current.GoToAsync(nameof(DetailsEntryPage), new Dictionary<string, object>
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
        #endregion

        #region Searching
        [ObservableProperty]
        private string searchText;

        partial void OnSearchTextChanged(string value)
        {
            _ = Search(value);
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

                _currentSearch = search;
                await Refresh();
            }
            catch (TaskCanceledException)
            {
                // Ignore
            }
        }
        #endregion

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
