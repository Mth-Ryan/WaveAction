using AutoMapper;
using Microsoft.Extensions.Logging;
using WaveAction.Application.Dtos.Access;
using WaveAction.Application.Interfaces;
using WaveAction.Domain.Common.Exceptions;
using WaveAction.Domain.Interfaces;
using WaveAction.Domain.Models;

namespace WaveAction.Application.Services;

public class AccessAppService : IAccessAppService
{
    private readonly ILogger<AccessAppService> _logger;
    private readonly IAccessRepository _repository;
    private readonly IBcryptService _bcrypt;
    private readonly IMapper _mapper;

    public AccessAppService(
        ILogger<AccessAppService> logger,
        IAccessRepository repository,
        IBcryptService bcrypt,
        IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _bcrypt = bcrypt;
        _mapper = mapper;
    }

    public async Task<AuthorModel> Login(LoginDto input)
    {
        var author = await _repository.GetAuthorByUsernameOrEmail(input.UserNameOrEmail!);
        if (author is null)
            throw new EntityNotFoundException("Author not found");

        if (!_bcrypt.Verify(input.Password!, author.PasswordHash))
            throw new ArgumentException("Invalid Password");

        return author;
    }

    public async Task<AuthorModel> Signup(SignupDto input)
    {
        var foundEmail = await _repository.GetAuthorByUsernameOrEmail(input.Email!);
        if (!(foundEmail is null))
            throw new EntityFoundException("Email already taken");

        var foundUserName = await _repository.GetAuthorByUsernameOrEmail(input.UserName!);
        if (!(foundUserName is null))
            throw new EntityFoundException("Username already taken");

        var author = _mapper.Map<AuthorModel>(input);
        author.Profile.PublicEmail = author.Email;

        await _repository.AddAuthor(author);

        return author;
    }
}
