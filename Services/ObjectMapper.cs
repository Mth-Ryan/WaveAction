using AutoMapper;
using WaveAction.Models;
using WaveAction.Dtos.Access;
using WaveAction.Dtos.Author;
using WaveAction.Dtos.Posts;
using WaveAction.Dtos.Threads;

namespace WaveAction.Services;

public class ObjectMapperFactory
{
    private MapperConfiguration _config;

    public ObjectMapperFactory()
    {
        _config = new MapperConfiguration(cfg => {
            // Access Dtos
            cfg.CreateMap<SignupPorfileDto, ProfileModel>();
            cfg.CreateMap<SignupDto, AuthorDto>();

            // Author Dtos
            cfg.CreateMap<ProfileModel, AuthorProfileDto>();
            cfg.CreateMap<ProfileModel, AuthorShortProfileDto>();
            cfg.CreateMap<AuthorModel, AuthorDto>();
            cfg.CreateMap<AuthorModel, AuthorShortDto>();

            // Posts Dtos
            cfg.CreateMap<PostModel, PostDto>();
            cfg.CreateMap<PostModel, PostShortDto>();
            cfg.CreateMap<PostCreateDto, PostModel>();

            // Threads Dtos
            cfg.CreateMap<ThreadModel, ThreadDto>();
            cfg.CreateMap<ThreadCreateDto, ThreadModel>();
        });
    }

    public IMapper CreateMapper() => _config.CreateMapper();
}