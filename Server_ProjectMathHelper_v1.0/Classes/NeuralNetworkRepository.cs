using Repository;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using SetWordsForNeuralNetwork;
using System.Text.RegularExpressions;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public class NeuralNetworkRepository
    {
        /// <summary>
        /// Метод создаёт экземпляр нейронной сети
        /// </summary>
        /// <param name="topology">
        /// Топология содержит данные о обучении и слоях нейронной сети
        /// </param>
        /// <returns>
        /// Возвращает новый экземпляр NeuralNetwork(Topology)
        /// </returns>
        public NeuralNetwork InitNeuralNetwork(Topology topology)
        {
            return new NeuralNetwork(topology);
        }

        /// <summary>
        /// Метод обучает нейронную сеть.
        /// </summary>
        /// <param name="neuralNetwork">
        /// Экземпляр, который метод будет обучать
        /// </param>
        /// <param name="epoch">
        /// Количество итераций обучения на фиктивных данных
        /// </param>
        /// <returns>
        /// Возвращает ошибку после обучения, учитывая количество итераций
        /// </returns>
        public double Learn(NeuralNetwork neuralNetwork, int epoch)
        {
            return neuralNetwork.Learn(epoch);
        }

        /// <summary>
        /// Метод выполняет работу нейронной сети
        /// </summary>
        /// <param name="neuralNetwork">
        /// Экземпляр нейронной сети через который проходит запрос
        /// </param>
        /// <param name="input">
        /// Ввод пользователя, поступающий в нейронную сеть
        /// </param>
        /// <returns></returns>
        public double Work(NeuralNetwork neuralNetwork, string input)
        {
            Neuron outputNeuron = neuralNetwork.FeedForward(VectorizeText(neuralNetwork.Topology.WordsData, input));
            return outputNeuron.Output;
        }

        public Data SetSentensesForData()
        {
            var dataDb = new DataDb();
            var data = new Data()
            {
                TrainingData = new List<Tuple<double, double[]>>(),
                WordsData = new Dictionary<string, int>()
            };

            foreach (var example in dataDb.Examples)
            {
                var exampleWords = example.Description.Split();
                foreach (var word in exampleWords)
                {
                    if (!data.WordsData.ContainsKey(word))
                        data.WordsData.Add(word, data.WordsData.Count + 1);
                }
                
            }          
            foreach (var example in dataDb.Examples)
            {
                data.TrainingData.Add(new Tuple<double, double[]>(example.Property.Id, VectorizeText(data.WordsData, example.Description)));
            }
            return data;
           
        }

        public double FindClosestOutput(double output, List<Tuple<double, double[]>> trainingData)
        {
            var outputs = SetOutputs(trainingData);
            var closestOutput = outputs[0];
            var minDifference = Math.Abs(outputs[0] - output);

            foreach (var number in outputs)
            {
                var difference = Math.Abs(number - output);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestOutput = number;
                }
            }

            return closestOutput;
        }

        private double[] SetOutputs(List<Tuple<double, double[]>> trainingData)
        {
            var result = new List<double>();
            foreach (var item in trainingData)
            {
                if (!result.Contains(item.Item1))
                    result.Add(item.Item1);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Векторизует текст, преобразуя его в числовой вектор на основе словаря.
        /// </summary>
        /// <param name="text">Текст для векторизации.</param>
        /// <param name="vocabulary">Словарь слов, хранящихся в памяти.</param>
        /// <returns>Числовой вектор, представляющий текст.</returns>
        private double[] VectorizeText(Dictionary<string, int> wordsData, string input)
        {
            input = Regex.Replace(input, @"[\p{P}-[.]]", string.Empty); //удаление знаков препинания
            var words = input.Split(' '); // превращение строки в массив слов
            // создание пустого вектора, где все элементы равны 0
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

        

       
    }
}