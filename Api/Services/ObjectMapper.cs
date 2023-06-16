using AutoMapper;
using WaveActionApi.Models;
using WaveActionApi.Dtos.Access;
using WaveActionApi.Dtos.Author;
using WaveActionApi.Dtos.Posts;
using WaveActionApi.Dtos.Threads;
using BC = BCrypt.Net.BCrypt;

namespace WaveActionApi.Services;

public class ObjectMapperFactory
{
    private readonly MapperConfiguration _config;

    public ObjectMapperFactory()
    {
        _config = new MapperConfiguration(cfg =>
        {
            // Access Dtos
            cfg.CreateMap<SignupProfileDto, ProfileModel>();
            cfg.CreateMap<SignupDto, AuthorModel>()
                .ForMember(dest => dest.PasswordHash, o => o.MapFrom(a => BC.HashPassword(a.Password)));

            // Author Dtos
            cfg.CreateMap<ProfileModel, AuthorProfileDto>();
            cfg.CreateMap<ProfileModel, AuthorShortProfileDto>();
            cfg.CreateMap<AuthorModel, AuthorDto>();
            cfg.CreateMap<AuthorModel, AuthorShortDto>();

            // Posts Dtos
            cfg.CreateMap<PostModel, PostDto>();
            cfg.CreateMap<PostModel, PostShortDto>();
            cfg.CreateMap<PostCreateDto, PostModel>()
                .ForMember(dest => dest.Tags, o => o.MapFrom(p => string.Join(",", p.Tags!.ToArray())));

            // Threads Dtos
            cfg.CreateMap<ThreadModel, ThreadDto>();
            cfg.CreateMap<ThreadCreateDto, ThreadModel>();
        });
    }

    public IMapper CreateMapper() => _config.CreateMapper();
}