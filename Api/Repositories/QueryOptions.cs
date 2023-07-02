using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WaveActionApi.Repositories;

public class QueryOptions
{
    [BindProperty(Name = "page")]
    public uint Page { get; set; } = 0;
    
    [BindProperty(Name = "pageSize")]
    [Range(1, 1000)]
    public uint PageSize { get; set; } = 25;

    public int GetSkip() => (int)Page * (int)PageSize;
    
    public int GetTake() => (int)PageSize;
}