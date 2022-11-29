namespace BioLib
{
    public class Reader
    {
        /// <summary>
        /// Get first half letters from a word.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetHalfChars(string word)
        {
            var outDictionary = new Dictionary<string, string>();
            if (word.Length == 0)
                return new Dictionary<string, string>();

            double split = (double)word.Count() / 2;
            string letterLengt = Math.Round(split, 2).ToString();
            if (letterLengt.Contains("."))
            {
                if (word.Length > 3)
                    split = Math.Round(split, MidpointRounding.AwayFromZero);
                else
                    split = Math.Round(split, MidpointRounding.ToZero);
            }

            if (word.Length == 1)
                split = 1;
            int splitChars = Int32.Parse(split.ToString());
            var value = word.Substring(0, splitChars);
            if (!outDictionary.ContainsKey(word))
                outDictionary.Add(word, value);
            return outDictionary;
        }
    }
}