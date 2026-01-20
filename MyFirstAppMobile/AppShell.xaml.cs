namespace MyFirstAppMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(EntryPage), typeof(EntryPage));
            Routing.RegisterRoute(nameof(EntryDetailsPage), typeof(EntryDetailsPage));
        }
    }
}
