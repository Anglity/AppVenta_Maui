using Firebase.Database;
using Firebase.Database.Query;

namespace AppVenta
{
    public class FirebaseDatabaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseDatabaseService()
        {
            _firebaseClient = new FirebaseClient("https://email-d82c2-default-rtdb.firebaseio.com/");
        }

        public async Task SaveUserAsync(string nombre, string email, string password)
        {
            await _firebaseClient
                .Child("Users")
                .PostAsync(new { Nombre = nombre, Email = email, Password = password });
        }

        public async Task<bool> UserExistsAsync(string nombre, string email)
        {
            // Verificar si el nombre de usuario existe
            var userByName = await _firebaseClient
                .Child("Users")
                .OrderBy("Nombre")
                .EqualTo(nombre)
                .OnceSingleAsync<object>();

            // Verificar si el correo electrónico existe
            var userByEmail = await _firebaseClient
                .Child("Users")
                .OrderBy("Email")
                .EqualTo(email)
                .OnceSingleAsync<object>();

            // Si existe el nombre de usuario o el correo electrónico, retorna true
            return userByName != null || userByEmail != null;
        }
    }
}
