using MyFirstAppMobile.Models;
using MyFirstAppMobile.ViewModels;

namespace MyFirstAppMobile;

public partial class EntryPage : ContentPage
{
	public EntryPage(EntryFormViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}