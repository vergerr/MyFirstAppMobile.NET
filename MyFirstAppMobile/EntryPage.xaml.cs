using MyFirstAppMobile.Models;
using MyFirstAppMobile.ViewModels;
using System.Threading.Tasks;

namespace MyFirstAppMobile;

public partial class EntryPage : ContentPage, IQueryAttributable
{
	private readonly EntryFormViewModel _viewModel;

    public EntryPage(EntryFormViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
		BindingContext = vm;
	}

    async void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("EntryId", out var value) && value is Guid id)
            await _viewModel.LoadForEditAsync(id);
        else
            _viewModel.ResetForCreate();
    }

}