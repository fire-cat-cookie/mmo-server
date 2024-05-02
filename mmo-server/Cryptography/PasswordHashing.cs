using System.Security.Cryptography;
using System.Text;

namespace mmo_server.Cryptography {
    public class PasswordHashing {

        public byte[] GenerateHash(string password, byte[] salt) {
            int hashIterationCount = 16793;
            return new Rfc2898DeriveBytes(password, salt, hashIterationCount).GetBytes(20);
        }

        public byte[] GenerateSalt() {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return salt;
        }

        public byte[] GenerateSaltedPasswordHash(string password) {
            byte[] passwordText = Encoding.ASCII.GetBytes(password);
            byte[] salt = GenerateSalt();
            byte[] key = GenerateHash(password, salt);
            byte[] combined = new byte[36];
            salt.CopyTo(combined, 0);
            key.CopyTo(combined, 16);
            return combined;
        }
    }
}
