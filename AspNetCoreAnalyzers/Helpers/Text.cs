namespace AspNetCoreAnalyzers
{
    public static class Text
    {
        public static void SkipWhiteSpace(string text, ref int pos)
        {
            while (pos < text.Length &&
                   text[pos] == ' ')
            {
                pos++;
            }
        }

        public static bool TrySkipPast(Span text, ref int pos, string substring)
        {
            var before = pos;
            while (pos + substring.Length <= text.Length)
            {
                if (IsAt(text, pos, substring))
                {
                    pos += substring.Length;
                    return true;
                }

                pos++;
            }

            pos = before;
            return false;
        }

        public static bool TrySkipPast(string text, ref int pos, string substring)
        {
            var before = pos;
            while (pos + substring.Length <= text.Length)
            {
                if (IsAt(text, pos, substring))
                {
                    pos += substring.Length;
                    return true;
                }

                pos++;
            }

            pos = before;
            return false;
        }

        private static bool IsAt(Span text, int pos, string substring)
        {
            for (var i = 0; i < substring.Length; i++)
            {
                if (text[pos + i] != substring[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsAt(string text, int pos, string substring)
        {
            for (var i = 0; i < substring.Length; i++)
            {
                if (text[pos + i] != substring[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
