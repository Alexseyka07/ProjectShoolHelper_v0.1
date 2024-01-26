using System;
using System.Collections.Generic;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;

namespace SetWordsForNeuralNetwork
{
    public static class SetSentenses
    {
        private static Data SetWords(string input)
        {
            Console.WriteLine("Введите слова:");
            Words words = new Words();
            return words.SetWords(input);
        }

        public static Data SetSentences(string input)
        {
            Data data = SetWords(input);
            Sentences sentences = new Sentences(data);
            return sentences.SetTrainingData(input);
        }
        
        public static void SetDataINDb()
        {

        }
    }
}