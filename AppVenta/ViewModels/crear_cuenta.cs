using AppVenta.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Database;
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

        private FirebaseDatabaseService _firebaseDatabaseService;
        private FirebaseAuthService _firebaseAuthService;

        public RegisterViewModel()
        {
            _firebaseDatabaseService = new FirebaseDatabaseService();
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

            if (Password.Length < 8)
            {
                await App.Current.MainPage.DisplayAlert("Error", "La contraseña debe tener al menos 8 caracteres.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            try
            {
                // Proceder con el registro de autenticación de Firebase
                await _firebaseAuthService.RegisterUserAsync(Email, Password);

                // Si el registro es exitoso, guarda la información adicional del usuario
                await _firebaseDatabaseService.SaveUserAsync(Nombre, Email, Password);

                // Notificar al usuario que el registro fue exitoso
                await App.Current.MainPage.DisplayAlert("Éxito", "Usuario registrado correctamente. Por favor, verifica tu email.", "OK");

                // Navegar a la página principal de la aplicación
                Application.Current.MainPage = new AppShell(); // Reemplazar con la página principal de tu aplicación
            }
            catch (Exception ex)
            {
                // Asumiendo que el FirebaseAuthService lanza una excepción con un mensaje claro cuando el email ya existe.
                if (ex.Message.Contains("EMAIL_EXISTS"))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "El correo electrónico ya está en uso por otra cuenta.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error al registrar: " + ex.Message, "OK");
                }
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
