namespace JokesGenerator
{
    public static class ArrayExtensions
    {
        public static void ReplaceAllOccurrences(this string[] items, string oldValue, string newValue)
        {
            for (int index = 0; index < items.Length; index++)
                if (items[index] != null && items[index].Contains(oldValue))
                    items[index] = items[index].Replace(oldValue, newValue);
        }
    }
}
