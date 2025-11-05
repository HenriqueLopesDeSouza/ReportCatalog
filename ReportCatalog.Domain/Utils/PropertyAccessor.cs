using System.Linq.Expressions;
using System.Reflection;

namespace ReportCatalog.Domain.Utils;

public static class PropertyAccessor
{
    private static readonly Dictionary<(Type, string), Delegate> _cache = new();

    public static Func<T, object?> Compile<T>(string propertyPath)
    {
        var key = (typeof(T), propertyPath);
        if (_cache.TryGetValue(key, out var del)) return (Func<T, object?>)del;

        Func<T, object?> lambda;

        if (typeof(T) == typeof(object))
        {
            var runtimeCache = new Dictionary<(Type, string), Func<object, object?>>();
            object? AccessRuntime(object? x)
            {
                if (x is null) return null;
                var rt = x.GetType();
                var rkey = (rt, propertyPath);

                if (!runtimeCache.TryGetValue(rkey, out var getter))
                {
                    var paramObj = Expression.Parameter(typeof(object), "o");
                    var casted = Expression.Convert(paramObj, rt);
                    Expression body = casted;

                    foreach (var member in propertyPath.Split('.'))
                    {
                        var prop = body.Type.GetProperty(member,
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                            ?? throw new InvalidOperationException(
                                $"Propriedade '{member}' não encontrada em {body.Type.Name}");

                        body = Expression.Property(body, prop);
                    }

                    var convert = Expression.Convert(body, typeof(object));
                    var lam = Expression.Lambda<Func<object, object?>>(convert, paramObj).Compile();
                    runtimeCache[rkey] = lam;
                    getter = lam;
                }

                return getter(x);
            }

            lambda = (T x) => AccessRuntime(x);
        }
        else
        {
            var param = Expression.Parameter(typeof(T), "x");
            Expression body = param;

            foreach (var member in propertyPath.Split('.'))
            {
                var prop = body.Type.GetProperty(member,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                    ?? throw new InvalidOperationException(
                        $"Propriedade '{member}' não encontrada em {body.Type.Name}");

                body = Expression.Property(body, prop);
            }

            var convert = Expression.Convert(body, typeof(object));
            lambda = Expression.Lambda<Func<T, object?>>(convert, param).Compile();
        }

        _cache[key] = lambda;
        return lambda;
    }
}
