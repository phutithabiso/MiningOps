namespace MiningOps.Security
{
    public class PasswordHasher
    {
        // Method to generate a random salt
        public static string GenerateSalt(int size = 32)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var saltBytes = new byte[size];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
        // Method to hash a password with a given salt
        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var combinedPassword = password + salt;
                var bytes = System.Text.Encoding.UTF8.GetBytes(combinedPassword);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        // Method to verify a password against a stored hash and salt
        public static bool VerifyPassword(string enteredPassword, string storedHash, string salt)
        {
            var hashOfInput = HashPassword(enteredPassword, salt);
            return hashOfInput == storedHash;
        }

    }
}
