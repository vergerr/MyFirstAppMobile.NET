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

        async void OnFilterWeekClicked(object sender, EventArgs e)
        {
            if (BindingContext is EntriesViewModel viewModel)
            {
                await viewModel.ApplyFilterCommand.ExecuteAsync("Week");
            }
        }
        async void OnFilterMonthClicked(object sender, EventArgs e)
        {
            if (BindingContext is EntriesViewModel viewModel)
            {
                await viewModel.ApplyFilterCommand.ExecuteAsync("Month");
            }
        }
        async void OnFilterAllClicked(object sender, EventArgs e)
        {
            if (BindingContext is EntriesViewModel viewModel)
            {
                await viewModel.ApplyFilterCommand.ExecuteAsync("All");
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
