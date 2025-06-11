using MAUINotificationApp.MVVM.Services;
using MAUINotificationApp.MVVM.ViewModels;

namespace MAUINotificationApp.MVVM.Views.Authentication;

public partial class LoginPage : ContentPage
{
    public LoginPage(AuthService authService)
    {
        InitializeComponent();
        BindingContext = new LoginViewModel(authService);
    }
}