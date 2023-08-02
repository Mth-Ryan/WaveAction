using WaveAction.Application.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace WaveAction.Infrastructure.Services;

public class BcryptService : IBcryptService
{
    public BcryptService() { }

    public bool Verify(string password, string hash) =>
        BC.Verify(password, hash);

    public string HashPassword(string password) =>
        BC.HashPassword(password);
}
