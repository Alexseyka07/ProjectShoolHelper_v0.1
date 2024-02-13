using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchoolChatGPT_v1._0.NeuralNetworkClasses
{
    /// <summary>
    /// Класс для хранения и управления данными нейронной сети.
    /// </summary>
    public class Data
    {
        public Dictionary<string, int> WordsData { get; set; }
        public List<Tuple<double, double[]>> TrainingData { get; set; }
        public List<Layer> Layers { get; set; }
        public double LearningRate { get; set; }
        public string Name { get; set; }

        private string json;
        private string path;
        private Topology topology;

        /// <summary>
        /// Пустой конструктор класса Data.
        /// </summary>
        public Data(string name)
        {
            Name = name;
            path = Path.Combine(Settings.Settings.AppPath, $"{Name}.json");
        }

        public Data()
        {
            Name = "testData";
            path = Path.Combine(Settings.Settings.AppPath, $"{Name}.json");
        }

        /// <summary>
        /// Получить данные из файла JSON.
        /// </summary>
        public void GetData()
        {

          //  try
            {
                json = File.ReadAllText(path);
                Data data = JsonSerializer.Deserialize<Data>(json);
                TrainingData = data.TrainingData;
                WordsData = data.WordsData;
            }
           // catch
            {
              //  SetData(new Dictionary<string, int>() { { "Что", 1 } }, new List<Tuple<double, double[]>>());
            }
        }
        public NeuralNetwork GetNeuralNetwork()
        {
           // try
            {
                
                json = File.ReadAllText(path);
                Data data = JsonSerializer.Deserialize<Data>(json);
                List<Layer> layers = data.Layers;
                topology = new Topology(this, Layers[0].NeuronCount, 1, LearningRate, new int[] { 30 });
                NeuralNetwork neuralNetwork = new NeuralNetwork(topology, layers);
                return neuralNetwork;
            }
            /*catch
            {
                NeuralNetwork neuralNetwork = new NeuralNetwork(topology);
                SetDataNeuralNetwork(neuralNetwork);
                return neuralNetwork;
            }*/
        }

        /// <summary>
        /// Установить данные в файл JSON.
        /// </summary>
        public void SetData(Dictionary<string, int> wordsData, List<Tuple<double, double[]>> trainingData)
        {
            UpdateData(wordsData, trainingData);
            Data data = this;
            json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
            Console.WriteLine("Data set");
        }

        public void SetData()
        {            
            Data data = this;
            json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
            Console.WriteLine("Data set");
        }
        public void SetDataNeuralNetwork(NeuralNetwork neuralNetwork)
        {
            UpdateData(neuralNetwork.Layers, neuralNetwork.Topology.LearningRate);

            Data data = this;
            json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }

        private void UpdateData(List<Layer> layers, double learningRate)
        {
            Layers = layers;
            LearningRate = learningRate;
        }

        private void UpdateData(Dictionary<string, int> wordsData, List<Tuple<double, double[]>> trainingData)
        {
            WordsData = wordsData;
            TrainingData = trainingData;
        }
    }
}