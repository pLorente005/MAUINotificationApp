using MAUINotificationApp.MVVM.Services;
using MAUINotificationApp.MVVM.ViewModels;
using MAUINotificationApp.MVVM.Views.Authentication;

namespace MAUINotificationApp.MVVM.Views.User;

public partial class ProfilePage : ContentPage
{
    private readonly AuthService _authService;

    public ProfilePage(AuthService authService)
    {
        InitializeComponent();
        BindingContext = new ProfileViewModel(authService);

    }

}