using CalculateurAge.Views;

namespace CalculateurAge
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ResultatPage),
                              typeof(ResultatPage));
        }
    }
}
