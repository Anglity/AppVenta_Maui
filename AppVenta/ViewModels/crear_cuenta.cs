using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

            // Aquí iría la lógica para registrar el usuario, por ejemplo, añadir a la base de datos
            await App.Current.MainPage.DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
            // Navegar a otra página si es necesario
        }

        [RelayCommand]
        public async Task GoBack()
        {
            // Muestra un mensaje al usuario antes de cambiar de página
            bool shouldGoBack = await App.Current.MainPage.DisplayAlert("Confirmar", "¿Deseas volver al inicio de sesión?", "Sí", "No");

            if (shouldGoBack)
            {
                // Si el usuario confirma, cambia la MainPage a LoginPage directamente
                Application.Current.MainPage = new NavigationPage(new AppVenta.Pages.LoginPage());
            }
        }

    }
}
