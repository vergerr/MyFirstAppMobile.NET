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
                Entries.Clear();
                var all = await _repo.GetAllAsync();
                foreach (var entry in all)
                {
                    Entries.Add(entry);
                }
            }
            finally
            {
                isLoading = false;
                _semaphore.Release();
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
    }
}
