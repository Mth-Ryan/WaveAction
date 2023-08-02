using WaveAction.Domain.Specification;

namespace WaveAction.Application.Dtos.Shared;

public class PaginatedDataDto<T>
{
    public string? Search { get; set; }
    public uint Page { get; set; }
    public uint PageSize { get; set; }
    public uint ItemsTotalCount { get; set; }
    public uint PagesTotalCount => (uint)Math.Ceiling((float)ItemsTotalCount / PageSize);
    public string? Order { get; set; }
    public List<T> Data { get; set; } = new List<T>();

    public PaginatedDataDto(QueryOptions query, int total, List<T> data)
    {
        Search = query.SimpleSearch;
        Page = query.Page;
        PageSize = query.PageSize;
        ItemsTotalCount = (uint)total;
        Order = query.OrderBy;
        Data = data;
    }
}
