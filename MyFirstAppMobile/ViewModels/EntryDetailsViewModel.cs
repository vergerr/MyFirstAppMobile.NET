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
    public partial class EntryDetailsViewModel : ObservableObject
    {
        private Guid? _id;
        private readonly IEntriesRepository _repo;

        public EntryDetailsViewModel(IEntriesRepository repo)
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

        
        internal async Task LoadForShowDetailsAsync(Guid id)
        {
            _id = id;

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
    }
}
