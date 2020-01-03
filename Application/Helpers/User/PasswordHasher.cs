using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Security.Cryptography;
namespace Helpers.User.PasswordHasher
{
    public sealed class PasswordHasher : IPasswordHasher
{
  private const int SaltSize = 16; // 128 bit 
  private const int KeySize = 32; // 256 bit

  private int Iterations = 10000;
  

  public string Hash(string password)
  {
    using (var algorithm = new Rfc2898DeriveBytes(
      password,
      SaltSize,
      Iterations,
      HashAlgorithmName.SHA512))
    {
      var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
      var salt = Convert.ToBase64String(algorithm.Salt);

      return $"{salt}.{key}";
    }
  }

  public bool Check(string hash, string password)
  {
    var parts = hash.Split('.', 2);

    if (parts.Length != 2)
    {

    }

    var salt = Convert.FromBase64String(parts[0]);
    var key = Convert.FromBase64String(parts[1]);

    using (var algorithm = new Rfc2898DeriveBytes(
      password,
      salt,
      Iterations,
      HashAlgorithmName.SHA512))
    {
      var keyToCheck = algorithm.GetBytes(KeySize);

      var verified = keyToCheck.SequenceEqual(key);

      return verified;
    }
  }
}
}