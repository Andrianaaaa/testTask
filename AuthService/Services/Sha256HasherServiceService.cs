using System.Security.Cryptography;
using System.Text;

namespace API.Services;
using Microsoft.AspNetCore.Identity;

//Simple implementation of IHasherService using SHA256
//in real world scenario, we should use a more secure hashing algorithm
public class Sha256HasherServiceService : IHasherService
{
    private readonly PasswordHasher<object> _passwordHasher;

    public Sha256HasherServiceService()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

    public string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }

    public bool Verify(string hashedPassword, string providedPassword)
    {
        var hashedProvidedPassword = Hash(providedPassword);
        return hashedProvidedPassword == hashedPassword;
    }
}
