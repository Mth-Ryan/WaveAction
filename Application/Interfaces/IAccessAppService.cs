using WaveAction.Application.Dtos.Access;
using WaveAction.Domain.Models;

namespace WaveAction.Application.Interfaces;

public interface IAccessAppService
{
    public Task<AuthorModel> Signup(SignupDto input);
    public Task<AuthorModel> Login(LoginDto input);
}
