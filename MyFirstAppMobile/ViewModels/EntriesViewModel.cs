using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstAppMobile.Models;
using System.Collections.ObjectModel;

namespace MyFirstAppMobile.ViewModels
{
    public partial class EntriesViewModel : ObservableObject
    {
        public ObservableCollection<FitnessEntry> Entries { get; set; } = new();
        
        // Propriétés pour le formulaire (liées aux champs de saisie)
        [ObservableProperty]
        string activityType;

        [ObservableProperty]
        int durationMinutes;

        [ObservableProperty]
        string notes;

        [RelayCommand]
        public async Task SaveEntry()
        {
            // 1. On crée le nouvel objet avec les données saisies
            var newEntry = new FitnessEntry
            {
                ActivityType = this.ActivityType,
                DurationMinutes = this.DurationMinutes,
                Notes = this.Notes,
                Date = DateTime.Now
            };

            // 2. On l'ajoute à la liste
            Entries.Add(newEntry);

            // 3. On vide le formulaire pour la prochaine fois
            activityType = string.Empty;
            durationMinutes = 0;
            notes = string.Empty;

            // 4. On revient en arrière
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }



        [RelayCommand]
        public async Task Add()
        {
            await Shell.Current.GoToAsync(nameof(EntryPage));
        }
    }
}
