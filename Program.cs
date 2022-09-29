namespace CoincidenceIndex;

internal class Program
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

    private static readonly Dictionary<double, string> LanguageFrequency = new()
    {
        { 0.078, "german" },
        { 0.074, "spanish, italian or portuguese" },
        { 0.077, "french" },
        { 0.066, "english" },
        { 0.0385, "random" }
    };

    public static void Main(string[] args)
    {
        string[] fileList = Directory.GetFiles("data", "*.txt");
        var charFrequencies = new Dictionary<char, int>[fileList.Length];
        for (var i = 0; i < fileList.Length; i++)
        {
            charFrequencies[i] = new Dictionary<char, int>();
            string text = File.ReadAllText(fileList[i]);
            foreach (char c in text)
            {
                charFrequencies[i][c] = charFrequencies[i].TryGetValue(c, out int charFreq) ? charFreq + 1 : 1;
            }

            double coincidenceIndex = 0;
            foreach (char c in Alphabet)
            {
                if (charFrequencies[i].ContainsKey(c))
                {
                    coincidenceIndex +=
                        charFrequencies[i][c] * (charFrequencies[i][c] - 1.0) /
                        (text.Length * (text.Length - 1.0));
                }
            }

            double closest = LanguageFrequency.Keys.Aggregate((x, y) =>
                Math.Abs(x - coincidenceIndex) < Math.Abs(y - coincidenceIndex) ? x : y);

            Console.WriteLine($"Closest language for {Path.GetFileName(fileList[i])}({coincidenceIndex}) is {LanguageFrequency[closest]}");
        }

        for (int i = 0; i < fileList.Length - 1; i++)
        {
            for (int j = i + 1; j < fileList.Length; j++)
            {
                if (isEqual(charFrequencies[i], charFrequencies[j]))
                {
                    Console.WriteLine(
                        $"{Path.GetFileName(fileList[i])} and {Path.GetFileName(fileList[j])} have the same character frequency");
                }
            }
        }
    }

    public static bool isEqual(Dictionary<char, int> dict1, Dictionary<char, int> dict2)
    {
        if (dict1.Count != dict2.Count)
        {
            return false;
        }

        foreach (var pair in dict1)
        {
            if (!dict2.ContainsKey(pair.Key) || dict2[pair.Key] != pair.Value)
            {
                return false;
            }
        }

        return true;
    }
}