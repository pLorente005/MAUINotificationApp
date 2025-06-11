using Plugin.Firebase.CloudMessaging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace MAUINotificationApp;

public partial class MainPage : ContentPage
{
    // URL base de tu Function combinada (sin query string)
    private const string FunctionBaseUrl = "----FunctionBaseUrl----";

    // El mismo key que usas para la Function en Azure
    private const string FunctionKey = "----FunctionKey----";

    // Usuario que utilizas tanto para registrar como para enviar
    private const string DemoUser = "usuario_demo";

    // Topic al que queremos suscribirnos automáticamente
    private const string DefaultTopic = "Bienvenido";

    // Keys en Preferences para token, registro y suscripción
    private const string PrefKey_PreviousToken = "fcm_previous_token";
    private const string PrefKey_Subscribed = "fcm_subscribed";

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //Pedir permiso de notificaciones (Android 13+)
        await Permissions.RequestAsync<Permissions.PostNotifications>();

        try
        {
            await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FCM] CheckIfValidAsync falló: {ex}");
            this.Status.Text = "FCM no válido";
            return;
        }

        //Obtener token actual
        string currentToken;
        try
        {
            currentToken = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FCM] GetTokenAsync falló: {ex}");
            this.Status.Text = "Error al obtener token";
            return;
        }

        if (string.IsNullOrWhiteSpace(currentToken))
        {
            this.Status.Text = "Token FCM vacío";
            return;
        }

        string prevToken = Preferences.Get(PrefKey_PreviousToken, "");
        if (currentToken != prevToken)
        {
            bool registrado = await RegisterDeviceOnBackend(currentToken);
            if (registrado)
            {
                Preferences.Set(PrefKey_PreviousToken, currentToken);
                Console.WriteLine("[FCM] Token registrado/actualizado en backend.");
            }
            else
            {
                Console.WriteLine("[FCM] Falló el registro del token en backend.");
            }
        }
        else
        {
            Console.WriteLine("[FCM] El token no cambió, no se re-registra.");
        }

        bool yaSuscrito = Preferences.Get(PrefKey_Subscribed, false);

        if (!yaSuscrito)
        {
            try
            {
                await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync(DefaultTopic);
                Preferences.Set(PrefKey_Subscribed, true);
                Console.WriteLine($"[FCM] Suscrito automáticamente al topic '{DefaultTopic}'.");
                this.Status.Text = $"Suscrito a '{DefaultTopic}'";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FCM] Error suscribiéndose al topic: {ex}");
                this.Status.Text = "Error al suscribir topic";
            }
        }
        else
        {
            this.Status.Text = $"Ya suscrito a '{DefaultTopic}'";
        }
    }
    
    /// <summary>
    /// Llamada al backend para registrar (o actualizar) el dispositivo con su token.
    /// Devuelve true si la llamada fue satisfactoria (código 2xx).
    /// </summary>
    private async Task<bool> RegisterDeviceOnBackend(string fcmToken)
    {
        string url = $"{FunctionBaseUrl}"
            + $"?code={Uri.EscapeDataString(FunctionKey)}"
            + $"&action=registerdevice"
            + $"&user={Uri.EscapeDataString(DemoUser)}"
            + $"&mail={Uri.EscapeDataString("demo@correo.com")}"
            + $"&password={Uri.EscapeDataString("1")}"
            + $"&devicetype={Uri.EscapeDataString(DeviceInfo.Platform.ToString())}"
            + $"&active={Uri.EscapeDataString("true")}"
            + $"&fcmtoken={Uri.EscapeDataString(fcmToken)}";

        using var client = new HttpClient();
        try
        {
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Backend] Error al registrar dispositivo: {error}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Backend] Excepción registrando dispositivo: {ex}");
            return false;
        }
    }

    // Método opcional para que el usuario se dé de baja manualmente
    private async void OnUnSubscribeClicked(object sender, EventArgs e)
    {
        try
        {
            await CrossFirebaseCloudMessaging.Current.UnsubscribeFromTopicAsync(DefaultTopic);
            Preferences.Set(PrefKey_Subscribed, false);
            this.Status.Text = "Ya no estás suscrito";
            await DisplayAlert("Bienvenido", $"Te has dado de baja del tema '{DefaultTopic}'.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error al desuscribir", ex.Message, "OK");
        }
    }

    // Botón de prueba para enviar una notificación a tu usuario/demo
    private async void OnTestSendClicked(object sender, EventArgs e)
    {
        string url = $"{FunctionBaseUrl}"
            + $"?code={Uri.EscapeDataString(FunctionKey)}"
            + $"&action=sendnotifications"
            + $"&user={Uri.EscapeDataString(DemoUser)}"
            + $"&message={Uri.EscapeDataString("Prueba de notificación personalizada cmd")}";

        using var client = new HttpClient();
        try
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Notificación enviada correctamente al usuario.", "OK");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"La API respondió con un error: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de conexión", $"No se pudo enviar la notificación: {ex.Message}", "OK");
        }
    }
}
