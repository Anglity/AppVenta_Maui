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

        public async Task<string> RegisterUserAsync(string email, string password)
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
                // Aquí puedes lanzar una excepción o manejar el error como prefieras.
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error registering user: {errorContent}");
            }

            var resultContent = await response.Content.ReadAsStringAsync();
            // Asumiendo que quieres el IdToken resultante, pero podrías querer manejar toda la respuesta.
            var result = JsonSerializer.Deserialize<FirebaseRegisterResponse>(resultContent);
            return result?.IdToken;
        }
    }

    public class FirebaseRegisterResponse
    {
        public string IdToken { get; set; }
        public string Email { get; set; }
        // Agrega otros campos de respuesta si es necesario.
    }
}
