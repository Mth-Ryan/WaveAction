namespace WaveActionApi.Dtos.Shared;

public class PaginatedDataDto<T>
{
    public string? Search { get; set; }
    public uint Page { get; set; }
    public uint PageSize { get; set; }
    public uint ItemsTotalCount { get; set; }
    public uint PagesTotalCount => (uint)Math.Ceiling((float)ItemsTotalCount / PageSize);
    public string? Order { get; set; }
    public List<T> Data { get; set; } = new List<T>();
}
