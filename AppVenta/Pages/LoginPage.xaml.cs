using AppVenta.ViewModels;

namespace AppVenta.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginVM(); // Aseg�rate de que el ViewModel est� siendo inicializado correctamente.
    }

}