using CommunityToolkit.Mvvm.Input;
using MyFirstAppMobile.ViewModels;

namespace MyFirstAppMobile;

public partial class EntryDetailsPage : ContentPage, IQueryAttributable
{
    private readonly EntryDetailsViewModel _viewModel;
    public EntryDetailsPage(EntryDetailsViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    async void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("EntryId", out var value) && value is Guid id)
            await _viewModel.LoadForShowDetailsAsync(id);
    }
}