using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstAppMobile.ViewModels;
using System.Windows.Input;

namespace MyFirstAppMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage(EntriesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        async void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            if (BindingContext is EntriesViewModel viewModel)
            {
                await viewModel.SearchCommand.ExecuteAsync(searchBar.Text);
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is EntriesViewModel viewModel)
            {
                await viewModel.LoadCommand.ExecuteAsync(null);
            }
        }
    }
}
