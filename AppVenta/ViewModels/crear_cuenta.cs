using AppVenta.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AppVenta.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string nombre;
        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string confirmPassword;

        private FirebaseAuthService _firebaseAuthService;

        public RegisterViewModel()
        {
            // Asegúrate de tener el apiKey correcto
            _firebaseAuthService = new FirebaseAuthService("AIzaSyA2cI0MIlrCtznp4rt9kdbIbGePo3ARcms");
        }

        [RelayCommand]
        public async Task Register()
        {
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            try
            {
                var user = await _firebaseAuthService.RegisterUserAsync(Email, Password);
                await App.Current.MainPage.DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
                // Navegar a otra página si es necesario
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task GoBack()
        {
            bool shouldGoBack = await App.Current.MainPage.DisplayAlert("Confirmar", "¿Deseas volver al inicio de sesión?", "Sí", "No");
            if (shouldGoBack)
            {
                Application.Current.MainPage = new NavigationPage(new AppVenta.Pages.LoginPage());
            }
        }
    }
}
