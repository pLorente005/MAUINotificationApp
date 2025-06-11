using Plugin.Firebase.CloudMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MAUINotificationApp.MVVM.Services
{
    public class AuthService
    {
        private const string AuthStateKey = "AuthState";
        private const string LoggedUserKey = "LoggedUser";
        private const string FcmTokenPrefKey = "fcm_previous_token";

        private const string FunctionBaseUrl = "----FunctionBaseUrl----";

        private const string FunctionKey = "----FunctionKey----";

        public async Task<bool> IsAuthenticatedAsync()
        {
            await Task.Delay(2000);

            var authState = Preferences.Default.Get<bool>(AuthStateKey, false);
            return authState;
        }

        public async Task<bool> LoginAsync(string user, string password)
        {
            var fcmToken = Preferences.Default.Get<string>(FcmTokenPrefKey, null);

            if (string.IsNullOrWhiteSpace(fcmToken))
            {
                try
                {
                    fcmToken = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FCM] GetTokenAsync falló: {ex}");                 
                }
            }

            string encodedUser = HttpUtility.UrlEncode(user);
            string encodedPassword = HttpUtility.UrlEncode(password);
            string encodedToken = HttpUtility.UrlEncode(fcmToken);   
            string encodedKey = HttpUtility.UrlEncode(FunctionKey);   

            var url = new StringBuilder($"{FunctionBaseUrl}?action=login");
            url.Append($"&username={encodedUser}");
            url.Append($"&password={encodedPassword}");
            url.Append($"&fcmtoken={encodedToken}");
            url.Append($"&code={encodedKey}");

            using var http = new HttpClient();
            HttpResponseMessage response;

            try
            {
                response = await http.GetAsync(url.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar con el servicio de autenticación.", ex);
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Preferences.Default.Set<bool>(AuthStateKey, true);
                Preferences.Default.Set<string>(LoggedUserKey, user);
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }
            else
            {
                throw new Exception(
                    $"Respuesta inesperada del servidor: {(int)response.StatusCode} {response.ReasonPhrase}"
                );
            }
        }

        public async Task LogoutAsync()
        {
            var user = Preferences.Default.Get<string>(LoggedUserKey, null);

            var fcmToken = Preferences.Default.Get<string>(FcmTokenPrefKey, null);

            if (string.IsNullOrWhiteSpace(user))
            {
                Preferences.Default.Remove(AuthStateKey);
                Preferences.Default.Remove(LoggedUserKey);
                return;
            }

            string encodedUser = HttpUtility.UrlEncode(user);
            string encodedKey = HttpUtility.UrlEncode(FunctionKey);

            var urlBuilder = new StringBuilder($"{FunctionBaseUrl}?action=logout");
            urlBuilder.Append($"&username={encodedUser}");
            urlBuilder.Append($"&code={encodedKey}");

            if (!string.IsNullOrWhiteSpace(fcmToken))
            {
                string encodedToken = HttpUtility.UrlEncode(fcmToken);
                urlBuilder.Append($"&fcmtoken={encodedToken}");
            }

            string url = urlBuilder.ToString();

            using var http = new HttpClient();
            HttpResponseMessage response;

            try
            {
                response = await http.GetAsync(url);
            }
            catch (Exception ex)
            {
                Preferences.Default.Remove(AuthStateKey);
                Preferences.Default.Remove(LoggedUserKey);
                //Preferences.Default.Remove(FcmTokenPrefKey);
                throw new Exception("Error al comunicarme con el servicio para hacer logout.", ex);
            }

            if (response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.NotFound)
            {
                Preferences.Default.Remove(AuthStateKey);
                Preferences.Default.Remove(LoggedUserKey);
                //Preferences.Default.Remove(FcmTokenPrefKey);
            }
            else
            {
                Preferences.Default.Remove(AuthStateKey);
                Preferences.Default.Remove(LoggedUserKey);
                //Preferences.Default.Remove(FcmTokenPrefKey);
                throw new Exception($"Respuesta inesperada al hacer logout: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
        }
    }
}
