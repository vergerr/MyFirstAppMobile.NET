using CommunityToolkit.Mvvm.Messaging;
using MyFirstAppMobile.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile.ViewModels
{
    public partial class EntriesViewModel
    {
        public void RegisterMessengers()
        {
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
    }
}
