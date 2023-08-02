namespace WaveAction.Application.Interfaces;

public interface IBcryptService
{
    public bool Verify(string password, string hash);
    public string HashPassword(string password);
}
