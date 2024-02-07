using System.Text.Json;

namespace SchoolChatGPT_v1._0.NeuralNetworkClasses
{
    /// <summary>
    /// Класс для хранения и управления данными нейронной сети.
    /// </summary>
    public class Data
    {
        public Dictionary<string, int> WordsData { get; set; }
        public List<Tuple<double, double[]>> TrainingData { get; set; }
        public string Name { get; set; }

        private string json;
        private string path;

        /// <summary>
        /// Пустой конструктор класса Data.
        /// </summary>
        public Data(string name)
        {
            Name = name;
        }

        public Data()
        {
            Name = "data";
        }

        /// <summary>
        /// Получить данные из файла JSON.
        /// </summary>
        public void GetData()
        {
            path = Path.Combine(Settings.Settings.AppPath, $"{Name}.json");
            try
            {
                json = File.ReadAllText(path);
                Data data = JsonSerializer.Deserialize<Data>(json);
                TrainingData = data.TrainingData;
                WordsData = data.WordsData;
            }
            catch
            {
                SetData(new Dictionary<string, int>() { { "Что", 1 } }, new List<Tuple<double, double[]>>());
            }
        }

        /// <summary>
        /// Установить данные в файл JSON.
        /// </summary>
        public void SetData(Dictionary<string, int> wordsData, List<Tuple<double, double[]>> trainingData)
        {
            UpdateData(wordsData, trainingData);
            path = Path.Combine(Settings.Settings.AppPath, $"{Name}.json");
            Data data = this;
            json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
            Console.WriteLine("Data set");
        }

        private void UpdateData(Dictionary<string, int> wordsData, List<Tuple<double, double[]>> trainingData)
        {
            WordsData = wordsData;
            TrainingData = trainingData;
        }
    }
}