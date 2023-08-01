namespace WaveAction.Infrastructure.Extensions;

public static class QueryPaginationExtension
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, uint page, uint pageSize)
    {
        return query.Skip((int)page * (int)pageSize).Take((int)pageSize);
    }
}
