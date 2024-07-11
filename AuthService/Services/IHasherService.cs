namespace API.Services;

public interface IHasherService
{
    string Hash(string password);
    bool Verify(string hashedPassword, string providedPassword);
}