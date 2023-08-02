using AutoMapper;
using WaveAction.Application.Dtos.Access;
using WaveAction.Application.Dtos.Author;
using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Threads;
using WaveAction.Application.Interfaces;
using WaveAction.Domain.Models;

namespace WaveAction.Infrastructure.Mapper;

public class ObjectMapperFactory
{
    private readonly MapperConfiguration _config;

    public ObjectMapperFactory(ISlugService slug, IBcryptService bcrypt)
    {
        _config = new MapperConfiguration(cfg =>
        {
            // Access Dtos
            cfg.CreateMap<SignupProfileDto, ProfileModel>();
            cfg.CreateMap<SignupDto, AuthorModel>()
                .ForMember(dest => dest.PasswordHash, o => o.MapFrom(a => bcrypt.HashPassword(a.Password!)));

            // Author Dtos
            cfg.CreateMap<AuthorProfileDto, ProfileModel>();
            cfg.CreateMap<ProfileModel, AuthorProfileDto>();
            cfg.CreateMap<ProfileModel, AuthorShortProfileDto>();
            cfg.CreateMap<AuthorModel, AuthorDto>();
            cfg.CreateMap<AuthorModel, AuthorShortDto>();

            // Threads Dtos
            cfg.CreateMap<ThreadModel, ThreadDto>();
            cfg.CreateMap<ThreadModel, ThreadShortDto>();
            cfg.CreateMap<ThreadCreateDto, ThreadModel>()
                .ForMember(dest => dest.TitleSlug, o => o.MapFrom(t => slug.Generate(t.Title!)));

            // Posts Dtos
            cfg.CreateMap<PostModel, PostDto>()
                .ForMember(
                    dest => dest.TagList,
                    o => o.MapFrom(p => p.Tags.Split(',', StringSplitOptions.None).ToList()));
            cfg.CreateMap<PostModel, PostShortDto>()
                .ForMember(dest => dest.TagsList,
                    o => o.MapFrom(p => p.Tags.Split(',', StringSplitOptions.None).ToList()));
            cfg.CreateMap<PostCreateDto, PostModel>()
                .ForMember(dest => dest.Tags, o => o.MapFrom(p => string.Join(",", p.TagList!.ToArray())))
                .ForMember(dest => dest.TitleSlug, o => o.MapFrom(p => slug.Generate(p.Title!)));
        });
    }

    public IMapper CreateMapper() => _config.CreateMapper();
}
