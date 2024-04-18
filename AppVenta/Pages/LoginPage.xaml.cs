using AppVenta.ViewModels;

namespace AppVenta.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginVM(); // Asegúrate de que el ViewModel está siendo inicializado correctamente.
    }

}