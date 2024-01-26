using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SetWordsForNeuralNetwork
{
    public class Sentences
    {
        Data data;

        public Sentences(Data data)
        {
            this.data = data;
        }

        public List<Tuple<double, string>> SetSentences(string sentences)
        {
            List<Tuple<double, string>> result = new List<Tuple<double, string>>();

            // Разбиваем входные предложения на части, разделенные символом ":"
            string[] arraySentensesAndValue = sentences.Split(':');

            for (int i = 0; i < arraySentensesAndValue.Length; i++)
            {
                try
                {
                    // Разбиваем каждую часть на две части, разделенные символом "$"
                    string[] array = arraySentensesAndValue[i].Split('$');
                    double value = double.Parse(array[1]); // Преобразуем вторую часть в число
                    string sentense = RemovePunctuation(array[0]); // Удаляем пунктуацию из первой части
                    
                    result.Add(new Tuple<double, string>(value, sentense)); // Добавляем результат в список
                    
                }
                catch
                {
                    // Обработка исключения, если произошла ошибка при парсинге или других операциях
                }
            }

            return result; // Возвращаем список результатов
        }
        
        public Data SetTrainingData(string input)
        {
            List<Tuple<double, string>> sentenses = SetSentences(input);
           
            
            for (int i = 0; i < sentenses.Count; i++)
            {
                var vector = new double[data.WordsData.Count];
                for (int x = 0; x < vector.Length; x++)
                {
                    vector[x] = 0;
                }
                var wordsOfQuetion = sentenses[i].Item2.Split(); // Читаем входные слова из консоли и разделяем их на части

                for (int j = 0; j < wordsOfQuetion.Length; j++)
                {
                    if (data.WordsData.ContainsKey(wordsOfQuetion[j]))
                    {
                        var num = data.WordsData[wordsOfQuetion[j]]; // Получаем номер слова
                        vector[num - 1] = 1.0;
                    }
                }

                bool a = false;

                for (int j = 0; j < data.TrainingData.Count; j++)
                {
                    if (vector == data.TrainingData[j].Item2) a = true; // Проверяем, существует ли вектор в обучающем наборе
                }

                if (!a)
                {
                    data.TrainingData.Add(new Tuple<double, double[]>(sentenses[i].Item1, vector)); // Добавляем в обучающий набор
                }
            }
            return data;
        }

        private string RemovePunctuation(string input)
        {
            input = Regex.Replace(input, @"[\p{P}-[.]]", string.Empty);
            return input;
        }
    }
}