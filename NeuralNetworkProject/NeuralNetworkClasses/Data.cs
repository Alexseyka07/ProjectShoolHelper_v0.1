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
        public Dictionary<string, double> FavoriteWords { get; set; }
        public List<Tuple<double, double[]>> TrainingData { get; set; }
        public double LearningRate { get; set; }
        
        public List<Layer> Layers { get; set; }
        [JsonIgnore]
        public NeuralNetwork NeuralNetwork { get; set; }
        [JsonIgnore]
        public string Name { get; set; }

        private string json;
        private string path;
        private Topology topology;

        
       

        /// <summary>
        /// Получить данные из файла JSON.
        /// </summary>
        public bool GetData()
        {
            try
            {
                path = AppDomain.CurrentDomain.BaseDirectory + Name + ".json";
                json = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<Data>(json);
                TrainingData = data.TrainingData;
                WordsData = data.WordsData;
                FavoriteWords = data.FavoriteWords;
                Layers = data.Layers;
                LearningRate = data.LearningRate;
                topology = new Topology(data, WordsData.Count, 1, LearningRate, new int[] { 30 });
                NeuralNetwork = new NeuralNetwork(topology, Layers);
                return true;
            }
            catch
            {        
                TrainingData = new List<Tuple<double, double[]>>();
                WordsData = new Dictionary<string, int>();
                FavoriteWords = new Dictionary<string, double>();
                Layers = new List<Layer>();
                LearningRate = 0;

              return false;
            }
        }


        public bool SetData()
        {
            try
            {
                path = AppDomain.CurrentDomain.BaseDirectory + Name + ".json";
                Data data = this;
                json = JsonSerializer.Serialize(data);
                File.WriteAllText(path, json);
                return true;
            }
            catch { return false; }
        }
       
    
    }
}