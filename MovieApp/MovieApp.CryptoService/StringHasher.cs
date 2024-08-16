using System.Text;
using XSystem.Security.Cryptography;

namespace MovieApp.CryptoService
{
    public static class StringHasher
    {
        /// <summary>
        /// Computes the MD5 hash of the input string, typically used for hashing passwords.
        /// </summary>
        /// <param name="inputString">The input string to be hashed.</param>
        /// <returns>A string representing the MD5 hash of the input string.</returns>
        public static string Hash(string inputString)
        {
            var mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(inputString);
            byte[] hashedBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);
            return Encoding.ASCII.GetString(hashedBytes);
        }
    }
}