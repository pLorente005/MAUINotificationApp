using MAUINotificationApp.MVVM.Services;
using MAUINotificationApp.MVVM.Views.Authentication;
using MAUINotificationApp.MVVM.Views.User;

namespace MAUINotificationApp
{
    public partial class AppShell : Shell
    {
        private readonly AuthService authService;

        public AppShell()
        {
            InitializeComponent();

            authService = new AuthService();

            this.Navigating += OnNavigating;
        }

        private async void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            var rutasProtegidas = new[] { nameof(MainPage), nameof(ProfilePage) };

            if (rutasProtegidas.Any(ruta => e.Target.Location.OriginalString.Contains(ruta)))
            {
                bool isAuthenticated = Preferences.Default.Get("AuthState", false);

                if (!isAuthenticated)
                {
                    e.Cancel();
                    await Shell.Current.GoToAsync("//LoginPage");
                }
            }
        }
    }
}
