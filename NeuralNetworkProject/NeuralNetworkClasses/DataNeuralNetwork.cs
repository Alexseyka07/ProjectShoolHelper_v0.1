using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using System.Text.Json;

namespace SchoolChatGPT_v1._0.NeuralNetworkClasses
{
    /// <summary>
    /// Класс для хранения и управления данными нейронной сети.
    /// </summary>
    public class DataNeuralNetwork
    {
        private Topology topology;
        private string path;
        private string name;
        private string json;
        public List<Layer> Layers { get; set; }



        /// <summary>
        /// Пустой конструктор класса Data.
        /// </summary>
        public DataNeuralNetwork(string name, Topology topology)
        {
            this.topology = topology;
            this.name = name;
        
        }

        /// <summary>
        /// Получить данные из файла JSON.
        /// </summary>
        public NeuralNetwork GetData()
        {
            try
            {
                json = File.ReadAllText(path);
                DataNeuralNetwork data = JsonSerializer.Deserialize<DataNeuralNetwork>(json);
                List<Layer> layers = data.Layers;
                NeuralNetwork neuralNetwork = new NeuralNetwork(topology, layers);
                return neuralNetwork;
            }
            catch
            {
                NeuralNetwork neuralNetwork = new NeuralNetwork(topology);
                SetData(neuralNetwork.Layers);
                return neuralNetwork;
            }
        }

        /// <summary>
        /// Установить данные в файл JSON.
        /// </summary>
        public void SetData(List<Layer> layers)
        {
            UpdateData(layers);

            DataNeuralNetwork data = this;
            json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }

        private void UpdateData(List<Layer> layers)
        {
            Layers = layers;
        }
    }
}