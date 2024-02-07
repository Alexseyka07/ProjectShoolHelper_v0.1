using System;
using System.Collections.Generic;
using System.Xml;

namespace SchoolChatGPT_v1._0.NeuralNetworkClasses
{
    // <summary>
    /// Класс, представляющий топологию нейронной сети.
    /// </summary>
    public class Topology
    {

        /// <summary>
        /// Количество входных нейронов.
        /// </summary>
        public int InputCount { get; }

        /// <summary>
        /// Количество выходных нейронов.
        /// </summary>
        public int OutputCount { get; }

        /// <summary>
        /// Коэффициент обучения нейронной сети.
        /// </summary>
        public double LearningRate { get; }

        /// <summary>
        /// Список, содержащий количество нейронов в каждом скрытом слое.
        /// </summary>
        public List<int> HiddenLayers { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Topology с указанными параметрами.
        /// </summary>
        /// <param name="inputCount">Количество входных нейронов.</param>
        /// <param name="outputCount">Количество выходных нейронов.</param>
        /// <param name="learningRate">Коэффициент обучения нейронной сети.</param>
        /// <param name="layers">Список, содержащий количество нейронов в каждом скрытом слое.</param>
        public Topology(int inputCount, int outputCount, double learningRate, params int[] layers)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            LearningRate = learningRate;
            HiddenLayers = new List<int>();
            HiddenLayers.AddRange(layers);
           
        }
    }
}