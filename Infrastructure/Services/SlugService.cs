using Slugify;
using WaveAction.Application.Interfaces;

namespace WaveAction.Infrastructure.Services;

public class SlugService : ISlugService
{
    private readonly ISlugHelper _helper;

    public SlugService()
    {
        _helper = new SlugHelper();
    }

    public string Generate(string raw) => _helper.GenerateSlug(raw);
}
