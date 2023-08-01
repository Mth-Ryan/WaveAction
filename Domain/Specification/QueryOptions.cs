using System.ComponentModel.DataAnnotations;

namespace WaveAction.Domain.Specification;

public class QueryOptions
{
    public string? SimpleSearch { get; set; }

    public uint Page { get; set; } = 0;

    [Range(1, 1000)]
    public uint PageSize { get; set; } = 25;

    public string OrderBy { get; set; } = "createdAt.desc";
}
