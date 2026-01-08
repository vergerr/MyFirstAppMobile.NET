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
