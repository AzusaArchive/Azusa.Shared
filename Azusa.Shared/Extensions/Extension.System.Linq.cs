namespace Azusa.Shared.Extensions;

public static class LinqExtensions
{
    public static void Foreach<T>(this IEnumerable<T> @this, Action<T> action)
    {
        foreach (var item in @this)
        {
            action(item);
        }
    }
}