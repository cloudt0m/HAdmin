using BC = BCrypt.Net.BCrypt;
using System.Linq;

namespace HAdmin.Security
{
    public interface IHasher
    {
        string Hash(string password);
        bool Check(string hash, string password);
    }

    public class Hasher : IHasher
    {
        public string Hash(string password)
        {
            var hashedPassword = BC.HashPassword(password);
            return hashedPassword;
        }

        public bool Check(string hash, string password)
        {
            var isVerified = BC.Verify(password, hash);
            return isVerified;
        }
    }
}