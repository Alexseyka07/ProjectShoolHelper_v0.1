using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using System.Text.RegularExpressions;

namespace SetWordsForNeuralNetwork
{
    public class Words
    {
        private Data data;

        public Words()
        {
            data = new Data("testData")
            {
                TrainingData = new List<Tuple<double, double[]>>(),
                WordsData = new Dictionary<string, int>()
            };
        }

        public Data SetWords(string input)
        {
            string[] wordsSentence;

            wordsSentence = input.Split();
            for (int j = 0; j < wordsSentence.Length; j++)
            {
                if (!data.WordsData.ContainsKey(wordsSentence[j]))
                {
                    if (wordsSentence[j].Length > 1)
                        data.WordsData.Add(wordsSentence[j], data.WordsData.Count + 1);
                }
            }

            return data;
        }

        private string[] RemovePunctuationAndSplit(string input)
        {
            input = Regex.Replace(input, @"[\p{P}-[.]]", string.Empty);
            input = Regex.Replace(input, @"[$]", string.Empty);
            input = Regex.Replace(input, @"[\d]", " ");
            return input.Split();
        }
    }
}