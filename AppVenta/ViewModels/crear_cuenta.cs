using AppVenta.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace AppVenta.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly string SendGridApiKey = "O91BTtvjShuHD23KotWHdQ";
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

                // Enviar correo electrónico de verificación
                await SendVerificationEmail(Email);

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

        private async Task SendVerificationEmail(string email)
        {
            var client = new SendGridClient(SendGridApiKey);
            var from = new EmailAddress("edwinesp19@gmail.com", "Tu Nombre o Compañía");
            var to = new EmailAddress(email);
            var templateId = "d-346a2dcbf259464694024fe537b1c29d "; // Reemplaza con el ID real de tu plantilla

            // No necesitas subject ni contenido porque ya están definidos en la plantilla
            var msg = new SendGridMessage()
            {
                From = from,
                TemplateId = templateId
            };

            msg.AddTo(to);

            // Puedes pasar dinámicamente los datos a tu plantilla
            msg.SetTemplateData(new
            {
                codigoVerificacion = "123456" // Suponiendo que tienes un campo de código de verificación en tu plantilla
            });

            var response = await client.SendEmailAsync(msg);
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
