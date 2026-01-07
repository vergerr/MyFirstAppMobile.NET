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
    }
}
