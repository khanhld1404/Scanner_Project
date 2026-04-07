using System.Security.Cryptography;
namespace Manage_PocketPc.Services
{
    public class Handle_Password
    {
        private const int SaltSize = 8;          // 8 bytes (gọn để chuỗi hash ngắn)
        private const int Iterations = 10000;    // có thể tăng nếu server đủ mạnh
        private const int KeySize = 20;          // 160-bit (SHA-1 output)

        // Tạo chuỗi hash dạng: v1:<saltBase64>:<hashBase64>
        public static string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.", nameof(password));

            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA1);
            byte[] key = pbkdf2.GetBytes(KeySize);

            return $"v1:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}";
        }

        // Xác minh (nếu cần dùng trên web)
        public static bool Verify(string password, string stored)
        {
            if (string.IsNullOrWhiteSpace(stored)) return false;

            var parts = stored.Split(':');
            if (parts.Length != 3 || parts[0] != "v1") return false;

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expected = Convert.FromBase64String(parts[2]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA1);
            byte[] actual = pbkdf2.GetBytes(expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }

    }
}
