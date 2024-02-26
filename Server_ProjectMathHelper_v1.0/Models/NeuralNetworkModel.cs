using NeuralNetworkProject.NeuralNetworkClasses;
using Repository;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.Models
{
    public class NeuralNetworkModel
    {
        public NeuralNetwork NeuralNetwork { get; set; }
        public Data Data { get; set; }
        public NeuralNetworkRepository NeuralNetworkRepository { get; set; }

        public Topology Topology { get; set; }

        public string JsonName { get; set; }

        public NeuralNetworkModel()
        {
            LoadData();
            NeuralNetworkRepository = new NeuralNetworkRepository();
            
            NeuralNetwork = new NeuralNetwork(Topology);
            

        }

        private void LoadData()
        {
            Data = new Data() { Name = "FirstStage" };
            var isData = Data.GetData();
            if (isData)
            {
                NeuralNetwork = Data.NeuralNetwork;
                Topology = new Topology(Data, Data.WordsData.Count, 1, Data.LearningRate, new int[Data.Layers[1].NeuronCount]);
            }
            else { SetData(); }
        }

        public virtual void SetData()
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

            foreach (var example in dataDb.Examples)
            {
                var exampleWords = Vectorize.NormalizeText(example.Description);

                foreach (var word in exampleWords)
                {
                    if (!Data.WordsData.ContainsKey(word))
                        Data.WordsData.Add(word, Data.WordsData.Count + 1);
                }

            }

            foreach (var example in dataDb.Examples)
            {
                Data.TrainingData.Add(new Tuple<double, double[]>(example.Property.Rule.Id / 10.0, Vectorize.VectorizeText(Data.WordsData, example.Description, 0.5, favoriteWords)));
            }
        }
        public double Work(string input)
        {
            return NeuralNetworkRepository.Work(NeuralNetwork, input);
        }

        public double Learn(int epoch)
        {
            return NeuralNetworkRepository.Learn(NeuralNetwork, epoch);
        }
    }
}
