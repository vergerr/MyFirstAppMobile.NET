using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MyFirstAppMobile.Data;
using MyFirstAppMobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile.ViewModels
{
    public partial class EntryFormViewModel : ObservableObject
    {
        private Guid? _editId;
        private readonly IEntriesRepository _repo;

        public EntryFormViewModel(IEntriesRepository repo)
        {
            _repo = repo;
        }

        [ObservableProperty]
        private DateTime date = DateTime.Today;

        // Propriétés pour le formulaire (liées aux champs de saisie)
        [ObservableProperty]
        private string activityType;

        [ObservableProperty]
        private string durationMinutes;

        [ObservableProperty]
        private string notes;

        [ObservableProperty]
        private bool isBusy;

        public bool CanSave => !IsBusy;
        partial void OnIsBusyChanged(bool value) => OnPropertyChanged(nameof(CanSave));

        [RelayCommand]
        public async Task Save()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                if (string.IsNullOrEmpty(ActivityType) || !int.TryParse(DurationMinutes, out int durationConv) || durationConv <= 0)
                {
                    await Shell.Current.DisplayAlert("Erreur", "Type d'activité obligatoire et durée > 0", "OK");
                    return;
                }

                // 1. On crée le nouvel objet avec les données saisies
                var entry = new FitnessEntry
                {
                    ActivityType = ActivityType,
                    DurationMinutes = durationConv,
                    Notes = Notes,
                    Date = Date
                };

                if (_editId is null)
                {
                    // 2. On l'ajoute à la liste
                    WeakReferenceMessenger.Default.Send(new NewEntryMessage(entry));
                }
                else
                {
                    entry.Id = (Guid)_editId;
                    WeakReferenceMessenger.Default.Send(new UpdateEntryMessage(entry));
                }

                // 4. On revient en arrière
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        internal async Task LoadForEditAsync(Guid id)
        {
            _editId = id;

            var entry = await _repo.GetByIdAsync(id);
            if (entry is null)
            {
                await Shell.Current.DisplayAlert("Erreur", "Erreur sur le chargement", "OK");
                return;
            }

            ActivityType = entry.ActivityType;
            DurationMinutes = entry.DurationMinutes.ToString();
            Notes = entry.Notes;
            Date = entry.Date;
        }

        internal void ResetForCreate()
        {
            _editId = null;
            ActivityType = "";
            DurationMinutes = "0";
            Notes = "";
            Date = DateTime.Today;
        }
    }
    // Message strongly-typed
    public sealed class NewEntryMessage : ValueChangedMessage<FitnessEntry>
    {
        public NewEntryMessage(FitnessEntry value) : base(value) { }
    }

    public sealed class UpdateEntryMessage : ValueChangedMessage<FitnessEntry>
    {
        public UpdateEntryMessage(FitnessEntry value) : base(value) { }
    }
}
