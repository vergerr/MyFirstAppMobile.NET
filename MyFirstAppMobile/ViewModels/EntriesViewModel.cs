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
        public ObservableCollection<FitnessEntry> Entries { get; set; } = new();

        public EntriesViewModel(IEntriesRepository repo)
        {
            _repo = repo;
            // Réception du message venant du formulaire
            WeakReferenceMessenger.Default.Register<NewEntryMessage>(this, async (r, m) =>
            {
                await repo.AddAsync(m.Value);
                Entries.Add(m.Value);
            });
        }

        [RelayCommand]
        public async Task Load ()
        {
            Entries.Clear();
            var all = await _repo.GetAllAsync();
            foreach (var entry in all)
            {
                Entries.Add(entry);
            }
        }

        [RelayCommand]
        public async Task Add()
        {
            await Shell.Current.GoToAsync(nameof(EntryPage));
        }

        [RelayCommand]
        public async Task Delete(Guid id)
        {
            await _repo.DeleteAsync(id);
            FitnessEntry fe = Entries.First(e => e.Id == id);
            if (fe == null)
                return;
            Entries.Remove(fe);
        }
    }
}
