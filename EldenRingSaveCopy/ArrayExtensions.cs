using System;
using System.Collections.Generic;

namespace EldenRingSaveCopy
{
    static class ArrayExtensions
    {
        public static IEnumerable<int> FindSubArrayIndices(this byte[] buffer, byte[] pattern)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (pattern.Length == 0 || pattern.Length > buffer.Length)
            {
                yield break;
            }

            for (int i = 0; i <= buffer.Length - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (buffer[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    yield return i;
                }
            }
        }
    }
}
