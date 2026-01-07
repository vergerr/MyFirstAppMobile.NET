using MyFirstAppMobile.Models;
using MyFirstAppMobile.ViewModels;

namespace MyFirstAppMobile;

public partial class EntryPage : ContentPage
{
	public EntryPage(EntriesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}