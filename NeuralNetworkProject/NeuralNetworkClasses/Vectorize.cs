using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NeuralNetworkProject.NeuralNetworkClasses
{
    public static class Vectorize
    {
        public static double[] VectorizeText(Dictionary<string, int> wordsData, string input)
        {
            var words = NormalizeText(input);

            var vector = new double[wordsData.Count].Select(x => x = 0).ToArray();
            for (int j = 0; j < words.Length; j++)
            {
                if (wordsData.ContainsKey(words[j]))
                {
                    var num = wordsData[words[j]]; // заполнение вектора единицами, там где это нужно
                    vector[num - 1] = 1.0;
                }
            }

            return vector; // метод возвращает готовый вектор
        }

        public static double[] VectorizeText(Dictionary<string, int> wordsData, string input, double defoultValue, Dictionary<string, double> fafouritWords)
        {

            var words = NormalizeText(input);
            // создание пустого вектора, где все элементы равны 0
            var vector = new double[wordsData.Count].Select(x => x = 0).ToArray();
           foreach ( var word in words) 
           {
                if (wordsData.ContainsKey(word))
                {
                    var num = wordsData[word]; // заполнение вектора единицами, там где это нужно
                    vector[num - 1] = defoultValue;
                    if (fafouritWords.ContainsKey(word))
                    {
                        vector[num - 1] = fafouritWords[word];
                    }
                    
                }
            }

            return vector; // метод возвращает готовый вектор
        }

        public static string[] NormalizeText(string input)
        {
            var exampleT = Regex.Replace(input, @"[\p{P}-[.]]", "");
            exampleT = Regex.Replace(exampleT, @"[\d]", "");
            exampleT = Regex.Replace(exampleT, "[A-Za-z]", "");
            return Regex.Replace(exampleT, @"°", "").Split();

        }
    }
}
