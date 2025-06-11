using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUINotificationApp.MVVM.Services;
using MAUINotificationApp.MVVM.Views.Authentication;
using System.Threading.Tasks;

namespace MAUINotificationApp.MVVM.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        public ProfileViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Logout()
        {
            _authService.LogoutAsync();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
