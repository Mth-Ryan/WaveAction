using WaveActionApi.Models;

public static class QuerySearchExtensions
{
    public static IQueryable<AuthorModel> SimpleSearch(this IQueryable<AuthorModel> query, string? search)
    {
        if (search is null)
        {
            return query;
        }
        return query.Where(a => a.UserName.Contains(search) ||
                           a.Email.Contains(search) ||
                           a.Profile!.FirstName.Contains(search) ||
                           a.Profile!.LastName.Contains(search));
    }

    public static IQueryable<ThreadModel> SimpleSearch(this IQueryable<ThreadModel> query, string? search)
    {
        if (search is null)
        {
            return query;
        }
        return query.Where(t => t.Title!.Contains(search));
    }

    public static IQueryable<PostModel> SimpleSearch(this IQueryable<PostModel> query, string? search)
    {
        if (search is null)
        {
            return query;
        }
        return query.Where(p => p.Title!.Contains(search));
    }
}
