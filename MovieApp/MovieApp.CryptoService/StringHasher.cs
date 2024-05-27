using System.Text;
using XSystem.Security.Cryptography;

namespace MovieApp.CryptoService
{
    public static class StringHasher
    {
        public static string Hash(string inputString)
        {
            var mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(inputString);
            byte[] hashedBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);
            return Encoding.ASCII.GetString(hashedBytes);
        }
    }
}