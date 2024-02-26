using System.Text.Json.Serialization;

namespace SchoolChatGPT_v1._0.NeuralNetworkClasses
{
    /// <summary>
    /// Класс, представляющий слой нейронов в нейронной сети.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Список нейронов в слое.
        /// </summary>
        public List<Neuron> Neurons { get; }

        /// <summary>
        /// Количество нейронов в слое.
        /// </summary>
        public int NeuronCount => Neurons?.Count ?? 0;

        /// <summary>
        /// Тип слоя (Input, Normal, Output).
        /// </summary>
        public NeuronType Type { get; }


        /// <summary>
        /// Инициализирует новый экземпляр класса Layer с указанными нейронами и типом слоя.
        /// </summary>
        /// <param name="neurons">Список нейронов в слое.</param>
        /// <param name="neuronType">Тип слоя (по умолчанию Normal).</param>
 
        public Layer(List<Neuron> Neurons, NeuronType Type = NeuronType.Normal)
        {
            this.Type = Type;
            this.Neurons = Neurons;
            if(Type == NeuronType.Normal)
            { }
        }

        /// <summary>
        /// Получает сигналы выходов нейронов в слое.
        /// </summary>
        /// <returns>Список сигналов выходов нейронов.</returns>
        public List<double> GetSignals()
        {
            var result = new List<double>();
            foreach (Neuron neuron in Neurons)
            {
                result.Add(neuron.Output);
            }
            return result;
        }
    }
}