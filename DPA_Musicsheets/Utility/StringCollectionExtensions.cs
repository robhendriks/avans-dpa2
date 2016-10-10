using System.Collections.Generic;
using System.Text;

namespace DPA_Musicsheets.Utility
{
    public static class StringCollectionExtensions
    {
        public static string ConcatLines(this IEnumerable<string> strs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var str in strs)
            {
                sb.Append(str + "\n");
            }
            return sb.ToString();
        }
    }
}
