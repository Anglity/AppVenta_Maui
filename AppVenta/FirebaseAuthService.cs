using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppVenta.Services
{
    public class FirebaseAuthService
    {
        private readonly string _firebaseApiKey;
        private readonly HttpClient _httpClient;

        public FirebaseAuthService(string apiKey)
        {
            _firebaseApiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task RegisterUserAsync(string email, string password)
        {
            var signUpUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_firebaseApiKey}";
            var userData = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(signUpUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorResult = JsonSerializer.Deserialize<FirebaseErrorResponse>(errorContent);

                if (errorResult?.Error?.Message == "EMAIL_EXISTS")
                {
                    // Manejo específico del error EMAIL_EXISTS
                    throw new Exception("Una cuenta con este correo electrónico ya existe.");
                }

                throw new Exception($"Error registering user: {errorContent}");
            }

            // Aquí ya no devolvemos el IdToken.
            // Si no lo necesitas para nada más, no tienes que hacer nada más aquí.
        }




        public class FirebaseErrorResponse
        {
            public FirebaseError Error { get; set; }
        }

        public class FirebaseError
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public FirebaseErrorDetails[] Errors { get; set; }
        }

        public class FirebaseErrorDetails
        {
            public string Message { get; set; }
            public string Domain { get; set; }
            public string Reason { get; set; }
        }

        public class FirebaseRegisterResponse
        {
            public string IdToken { get; set; }
            public string Email { get; set; }
            public string RefreshToken { get; set; }
            public string ExpiresIn { get; set; }
            // Agrega otros campos de respuesta si es necesario.
        }
    }
}
