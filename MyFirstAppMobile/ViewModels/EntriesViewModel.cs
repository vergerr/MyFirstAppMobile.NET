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

        [ObservableProperty]
        private string selectedFilter;

        partial void OnSelectedFilterChanged(string value) => _ = Refresh();

        [ObservableProperty]
        private string selectedSort;

        partial void OnSelectedSortChanged(string value) => _ = Refresh();
        
        public EntriesViewModel(IEntriesRepository repo)
        {
            _repo = repo;
            RegisterMessengers();
        }

        private readonly SemaphoreSlim _semaphore = new(1, 1);

        [ObservableProperty]
        private bool isLoading;
        public bool CanInteract => !IsLoading;
        partial void OnIsLoadingChanged(bool value)
        {
            OnPropertyChanged(nameof(CanInteract));
        }

       

        private string _currentSearch = string.Empty;

        public async Task Refresh()
        {
            if (!await _semaphore.WaitAsync(0)) return;
            try
            {
                IsLoading = true;

                var ordered = await _repo.GetFilteredEntriesAsync(_currentSearch, SelectedFilter, SelectedSort);

                ReplaceEntries(ordered);
            }
            finally
            {
                IsLoading = false;
                _semaphore.Release();
            }
        }

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
