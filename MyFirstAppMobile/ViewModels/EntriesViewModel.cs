using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyFirstAppMobile.Models;
using System.Collections.ObjectModel;

namespace MyFirstAppMobile.ViewModels
{
    public partial class EntriesViewModel : ObservableObject
    {
        public ObservableCollection<FitnessEntry> Entries { get; set; } = new();

        public EntriesViewModel()
        {
            // Réception du message venant du formulaire
            WeakReferenceMessenger.Default.Register<NewEntryMessage>(this, (r, m) =>
            {
                Entries.Add(m.Value);
            });
        }

        [RelayCommand]
        public async Task Add()
        {
            await Shell.Current.GoToAsync(nameof(EntryPage));
        }
    }
}
