using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUINotificationApp.MVVM.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;   
using System.Threading.Tasks;

namespace MAUINotificationApp.MVVM.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private const string LoggedUserKey = "LoggedUser";

        private readonly AuthService _authService;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El usuario y la contraseña son obligatorios.",
                    "Aceptar");
                return;
            }

            bool loginOk = false;
            try
            {
                loginOk = await _authService.LoginAsync(UserName.Trim(), Password.Trim());
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error de conexión",
                    $"No se pudo conectar al servidor: {ex.Message}",
                    "Aceptar");
                return;
            }

            if (loginOk)
            {
                Preferences.Default.Set(LoggedUserKey, UserName.Trim());

                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Acceso denegado",
                    "Usuario o contraseña incorrectos.",
                    "Aceptar");
            }
        }
    }
}
