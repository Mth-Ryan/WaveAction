using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WaveActionApi.Repositories;

public class QueryOptions
{
    [BindProperty(Name = "simpleSearch")]
    public string? SimpleSearch { get; set; }

    [BindProperty(Name = "page")]
    public uint Page { get; set; } = 0;

    [BindProperty(Name = "pageSize")]
    [Range(1, 1000)]
    public uint PageSize { get; set; } = 25;

    [BindProperty(Name = "orderBy")]
    public string OrderBy { get; set; } = "createdAt.desc";
}
