using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MyFirstAppMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile.ViewModels
{
    public partial class EntryFormViewModel : ObservableObject
    {
        // Propriétés pour le formulaire (liées aux champs de saisie)
        [ObservableProperty]
        private string activityType;

        [ObservableProperty]
        private string durationMinutes;

        [ObservableProperty]
        private string notes;

        private bool isBusy;

        [RelayCommand]
        public async Task Save()
        {
            if (isBusy) return;
            isBusy = true;

            try
            {
                // 1. On crée le nouvel objet avec les données saisies
                var entry = new FitnessEntry
                {
                    ActivityType = ActivityType,
                    DurationMinutes = int.Parse(DurationMinutes),
                    Notes = Notes,
                    Date = DateTime.Now
                };

                // 2. On l'ajoute à la liste
                WeakReferenceMessenger.Default.Send(new NewEntryMessage(entry));

                // 3. On vide le formulaire pour la prochaine fois
                ActivityType = string.Empty;
                DurationMinutes = "0";
                Notes = string.Empty;

                // 4. On revient en arrière
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                isBusy = false;
            }
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
    // Message strongly-typed
    public sealed class NewEntryMessage : ValueChangedMessage<FitnessEntry>
    {
        public NewEntryMessage(FitnessEntry value) : base(value) { }
    }
}
