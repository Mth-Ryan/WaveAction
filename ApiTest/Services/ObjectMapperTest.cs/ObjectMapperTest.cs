using AutoMapper;
using WaveActionApi.Services;

namespace WaveActionApiTest.Services.ObjectMapperTest.cs;

public partial class ObjectMapperTest
{
    private static readonly IMapper Mapper = new ObjectMapperFactory().CreateMapper();
}