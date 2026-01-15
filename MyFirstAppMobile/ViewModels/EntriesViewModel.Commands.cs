using CommunityToolkit.Mvvm.Input;
using MyFirstAppMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile.ViewModels
{
    public partial class EntriesViewModel
    {
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
            await Shell.Current.GoToAsync(nameof(EntryDetailsPage), new Dictionary<string, object>
            {
                ["EntryId"] = id
            });
        }

        [RelayCommand]
        public async Task Delete(Guid id)
        {
            FitnessEntry? fe = await _repo.GetByIdAsync(id);
            bool isConfirmed = await Shell.Current.DisplayAlert(
        "Suppression",
        $"Supprimer l'activité {fe.ActivityType} ?",
        "Oui",
        "Non");

            if (!isConfirmed)
                return;
            await _repo.DeleteAsync(id);
            fe = Entries.FirstOrDefault(e => e.Id == id);
            if (fe is null)
                return;
            Entries.Remove(fe);
        }

    }
}
