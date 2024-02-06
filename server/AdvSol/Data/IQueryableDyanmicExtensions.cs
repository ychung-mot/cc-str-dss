using System.Linq.Dynamic.Core;

namespace AdvSol.Data
{
    public static class IQueryableDynamicExtensions
    {
        public static IOrderedQueryable<TSource> DynamicOrderBy<TSource>(this IQueryable<TSource> source, string ordering, params object[] args)
        {
            return source.OrderBy(ordering, args);
        }
    }
}
