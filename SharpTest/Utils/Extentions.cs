using System.Runtime.CompilerServices;
using System.Text;

namespace WebSharp
{
    public static class Extentions
    {
        public static char[] ToCharArray(this StringBuilder stringBuilder)
        {
            int offset = 0;
            char[] array = new char[stringBuilder.Length];
            foreach (var chunk in stringBuilder.GetChunks())
            {
                chunk.Span.CopyTo(new Span<char>(array, offset, chunk.Span.Length));
                offset += chunk.Span.Length;
            }
            return array;
        }

        public static object[] ToArray<T>(this IEnumerable<T> source)
        {
            int count = source.Count();
            object[] array = new object[count];

            int index = 0;
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                array[index] = enumerator.Current;
                index++;
            }
            return array;
        }
    }
}
