using NeuralNetworkProject.NeuralNetworkClasses;
using Repository;
using Repository.Models;
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
            Neuron outputNeuron = neuralNetwork.FeedForward(Vectorize.VectorizeText(neuralNetwork.Topology.WordsData, input));
            return outputNeuron.Output;
        }
        public Data SetSentensesForData()
        {
            var dataDb = new DataDb();
            var favoriteWords = new Dictionary<string, double>()
            {
                {"высота", 1.0 },
                {"высоты", 1.0 },
                {"медиана", 1.0 },
                {"высотой", 1.0 },
                {"равнобедренный", 1.0 },
                {"равнобедренном", 1.0 },
                {"биссектриса", 1.0 },
                {"периметр", 1.0 },
                {"медианы", 1.0 },
                {"гипотенуза", 1.0 },
                {"катет", 1.0 },
                {"катеты", 1.0 },
                {"катетов", 1.0 },
                {"прямоугольный", 1.0 },
                {"прямоугольного", 1.0 },
                {"гипотенузе", 1.0 },
                {"треугольник", 0.0 },
                {"параллельно", 1.0 },
                {"параллельны", 1.0 },
                {"параллельна", 1.0 },
            };
            var data = new Data()
            {
                TrainingData = new List<Tuple<double, double[]>>(),
                WordsData = new Dictionary<string, int>()
            };
           
            foreach (var example in dataDb.Examples)
            {
                
               var exampleT = Regex.Replace(example.Description, @"[\p{P}-[.]]", "");
               exampleT = Regex.Replace(exampleT, @"[\d]", "");
                exampleT = Regex.Replace(exampleT, "[A-Za-z]", "");
                var exampleWords = Regex.Replace(exampleT, @"°", "").Split();
                

                foreach (var word in exampleWords)
                {
                    if (!data.WordsData.ContainsKey(word))
                        data.WordsData.Add(word, data.WordsData.Count + 1);
                }
                
            } 
                       
            foreach (var example in dataDb.Examples)
            {

                

               data.TrainingData.Add(new Tuple<double, double[]>(example.Property.Rule.Id/10.0, Vectorize.VectorizeText(data.WordsData, example.Description, 0.5, favoriteWords)));
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
    }
}