public static class EnumerablExtensions
{
    public static class EnumerablExtensions 
    {
        public static IEnumerable<T> AsNullSafe<T>(this IEnumerable<T> original) 
        {
          return original ?? (new T[0]);
        }
    }
}
