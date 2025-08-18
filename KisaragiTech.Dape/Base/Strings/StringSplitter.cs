using System;
using System.Collections.Generic;

namespace KisaragiTech.Dape.Base.Strings;

public static class StringSplitter
{
    public static IEnumerable<ReadOnlyMemory<char>> ToMemories(string haystack, char needle)
    {
        var start = 0;
        var index = haystack.IndexOf(needle, start);
        while (index != -1)
        {
            yield return haystack.AsMemory(start, index - start);
            start = index + 1;
            index = haystack.IndexOf(needle, start);
        }

        // 最後のピース
        if (start <= haystack.Length)
            yield return haystack.AsMemory(start);
    }
}