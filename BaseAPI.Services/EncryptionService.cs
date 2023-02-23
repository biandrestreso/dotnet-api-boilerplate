using BaseAPI.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BaseAPI.Services
{
    public class EncryptionService : IEncryptionService
    {
        private const int KeySize = 64;
        private const int Iterations = 350000;
        readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

        public string Encrypt(string plainText, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(KeySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(plainText),
                salt,
                Iterations,
                _hashAlgorithm,
                KeySize);

            return Convert.ToHexString(hash);
        }
        
        public bool Verify(string plainText, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(plainText, salt, Iterations, _hashAlgorithm, KeySize);

            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }
    }
}