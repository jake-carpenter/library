public static class ObjectExtensions
{
    public static IEnumerable<T> IntoEnumerable<T>(this T value)
    {
        return new List<T> { value };
    }
}