using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using System.Text.Json;

namespace AppVenta.ViewModels
{
    public partial class LoginVM : ObservableObject
    {



        [ObservableProperty]
        public string usuario = string.Empty;
        [ObservableProperty]
        public string password = string.Empty;

        private readonly string _firebaseApiKey = "AIzaSyA2cI0MIlrCtznp4rt9kdbIbGePo3ARcms";

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, ingrese su usuario y contraseña.", "Aceptar");
                return;
            }

            var signInUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseApiKey}";
            var userData = new
            {
                email = Usuario,
                password = Password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(signInUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<FirebaseLoginResponse>(resultContent);
                // Aquí asumimos que quieres guardar el token o alguna identificación del usuario
                Preferences.Set("idToken", result?.IdToken);

                Application.Current.MainPage = new AppShell();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                await Application.Current.MainPage.DisplayAlert("Error", $"Inicio de sesión fallido: {errorContent}", "Aceptar");
            }
        }

        [RelayCommand]
        private async Task Register()
        {
            // Cambia la MainPage a RegisterPage directamente
            Application.Current.MainPage = new NavigationPage(new AppVenta.Pages.RegisterPage());
        }


        private const string RedirectUri = "myapp://";

        [RelayCommand]
        private async Task LoginWithGoogle()
        {
            try
            {
                // Construye la URL para la autenticación de Google
                var authUrl = new Uri($"https://accounts.google.com/o/oauth2/v2/auth?client_id={_firebaseApiKey}&redirect_uri={RedirectUri}&response_type=token&scope=email");
                var callbackUrl = new Uri(RedirectUri);

                var authResult = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);

                // Verifica si se ha recibido un token de acceso
                if (!string.IsNullOrEmpty(authResult?.AccessToken))
                {
                    // Almacenar el token o realizar acciones adicionales según sea necesario
                    Preferences.Set("accessToken", authResult.AccessToken);

                    // Navegar a la página principal de la aplicación
                    Application.Current.MainPage = new AppShell();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores de autenticación
                await Application.Current.MainPage.DisplayAlert("Authentication Failed", $"An error occurred: {ex.Message}", "OK");
            }
        }


    }

    public class FirebaseLoginResponse
    {
        public string IdToken { get; set; }
        // Otros campos de respuesta que puedas necesitar
    }





}
