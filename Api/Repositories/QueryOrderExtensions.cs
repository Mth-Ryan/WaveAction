using WaveActionApi.Models;

namespace WaveActionApi.Repositories;

public static class QueryOrderExtensions
{
    public static IQueryable<AuthorModel> AuthorOrder(this IQueryable<AuthorModel> query, string order)
    {
        switch (order)
        {
            case "userName.asc":
                return query.OrderBy(a => a.UserName);

            case "userName.desc":
                return query.OrderByDescending(a => a.UserName);

            case "email.asc":
                return query.OrderBy(a => a.Email);

            case "email.desc":
                return query.OrderByDescending(a => a.Email);

            case "firstName.asc":
                return query.OrderBy(a => a.Profile!.FirstName);

            default:
                return query.OrderByDescending(a => a.Profile!.FirstName);
        }
    }

    public static IQueryable<ThreadModel> ThreadsOrder(this IQueryable<ThreadModel> query, string order)
    {
        switch (order)
        {
            case "title.asc":
                return query.OrderBy(p => p.Title);

            case "title.desc":
                return query.OrderByDescending(p => p.Title);

            case "updatedAt.asc":
                return query.OrderBy(p => p.UpdatedAt);

            case "updatedAt.desc":
                return query.OrderByDescending(p => p.UpdatedAt);

            case "createdAt.asc":
                return query.OrderBy(p => p.CreatedAt);

            default:
                return query.OrderByDescending(p => p.CreatedAt);
        }
    }

    public static IQueryable<PostModel> PostsOrder(this IQueryable<PostModel> query, string order)
    {
        switch (order)
        {
            case "title.asc":
                return query.OrderBy(p => p.Title);

            case "title.desc":
                return query.OrderByDescending(p => p.Title);

            case "updatedAt.asc":
                return query.OrderBy(p => p.UpdatedAt);

            case "updatedAt.desc":
                return query.OrderByDescending(p => p.UpdatedAt);

            case "createdAt.asc":
                return query.OrderBy(p => p.CreatedAt);

            default:
                return query.OrderByDescending(p => p.CreatedAt);
        }
    }
}
