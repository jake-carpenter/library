public static class EnumerablExtensions
{
    public static IEnumerable<T> AsNullSafe(IEnumerable<T> original)
    {
        return original != null ? original : new T[0];
    }
}