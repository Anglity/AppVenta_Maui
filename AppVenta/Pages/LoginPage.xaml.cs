using AppVenta.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;


namespace AppVenta.Pages {

    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginVM(); // Asegúrate de que el ViewModel está siendo inicializado correctamente.
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            using (var paint = new SKPaint())
            {
                // Crea un degradado lineal
                paint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, 0),
                    new SKPoint(0, e.Info.Height),
                    new[] { SKColor.Parse("#1C375C"), SKColor.Parse("#131A24") },
                    SKShaderTileMode.Clamp);

                canvas.DrawRect(new SKRect(0, 0, e.Info.Width, e.Info.Height), paint);
            }
        }
    }
}