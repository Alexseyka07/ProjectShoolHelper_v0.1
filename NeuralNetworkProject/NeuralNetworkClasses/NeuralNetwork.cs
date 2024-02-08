namespace SchoolChatGPT_v1._0.NeuralNetworkClasses
{
    /// <summary>
    /// Класс, представляющий нейронную сеть.
    /// </summary>
    public class NeuralNetwork
    {
        /// <summary>
        /// Топология нейронной сети.
        /// </summary>
        public Topology Topology { get; set; }

        /// <summary>
        /// Список слоев нейронов.
        /// </summary>
        public List<Layer> Layers { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса NeuralNetwork с указанной топологией.
        /// </summary>
        /// <param name="topology">Топология нейронной сети.</param>
        public NeuralNetwork(Topology topology)
        {
            Topology = topology;
            Layers = new List<Layer>();
            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }

        public NeuralNetwork(Topology topology, List<Layer> layers)
        {
            Topology = topology;
            Layers = new List<Layer>();
            Layers = layers;
        }

        /// <summary>
        /// Выполняет прямое распространение сигнала через нейронную сеть.
        /// </summary>
        /// <param name="inputSignals">Входные сигналы.</param>
        /// <returns>Выходной нейрон (или нейроны) сети.</returns>
        public Neuron FeedForward(params double[] inputSignals)
        {
            SendSignalsToInputNeurons(inputSignals);
            FeedForwardAddLayersAfterInput();

            if (Topology.OutputCount != 1)
                return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
            return Layers.Last().Neurons[0];
        }

        /// <summary>
        /// Обучает нейронную сеть на обучающем наборе данных.
        /// </summary>
        /// <param name="dataSet">Обучающий набор данных в формате (ожидаемый результат, входные данные).</param>
        /// <param name="epoch">Количество эпох обучения.</param>
        /// <returns>Среднеквадратичная ошибка после обучения.</returns>
        public double Learn(int epoch)
        {
            var error = 0.0;
            var num = 0;

            Console.WriteLine("Эпох пройденно:");
            for (int i = 0; i < epoch; i++)
            {
                foreach (var data in Topology.TrainingData)
                {
                    error += BackPropagation(data.Item1, data.Item2);
                }
                num++;

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"[NN {Topology.LearningRate}, HiddenNeurons{Layers[1].NeuronCount}] Загружено: {CalculatePercentage(num, epoch)}%");
            }
            Console.WriteLine();
            return error / epoch;
        }

        public double Learn(int epoch, double learningRate)
        {
            var oldLearningRate = Topology.LearningRate;
            Topology.LearningRate = learningRate;
            var error = 0.0;
            var num = 0;

            for (int i = 0; i < epoch; i++)
            {
                foreach (var data in Topology.TrainingData)
                {
                    error += BackPropagation(data.Item1, data.Item2);
                }
                num++;

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"[NN {Topology.LearningRate}, HiddenNeurons{Layers[1].NeuronCount}] Загружено: {CalculatePercentage(num, epoch)}%");
            }
            Console.WriteLine();
            Topology.LearningRate = oldLearningRate;
            return error / epoch;
        }

        private static double CalculatePercentage(double number, double total)
        {
            return (number / total) * 100;
        }

        /// <summary>
        /// Выполняет обратное распространение ошибки (обучение) для заданного обучающего примера.
        /// </summary>
        /// <param name="expected">Ожидаемый результат.</param>
        /// <param name="inputs">Входные данные.</param>
        /// <returns>Квадратичная ошибка.</returns>
        private double BackPropagation(double expected, params double[] inputs)
        {
            var actual = FeedForward(inputs).Output;
            var difference = actual - expected;

            foreach (var neuron in Layers.Last().Neurons)
            {
                neuron.Learn(difference, Topology.LearningRate);
            }

            for (int j = Layers.Count - 2; j >= 0; j--)
            {
                var layer = Layers[j];
                var previousLayer = Layers[j + 1];

                for (int i = 0; i < layer.NeuronCount; i++)
                {
                    var neuron = layer.Neurons[i];

                    for (int k = 0; k < previousLayer.NeuronCount; k++)
                    {
                        var previousNeuron = previousLayer.Neurons[k];
                        var error = previousNeuron.Weights[i] * previousNeuron.Delta;
                        neuron.Learn(error, Topology.LearningRate);
                    }
                }
            }

            return difference * difference;
        }

        /// <summary>
        /// Выполняет прямое распространение сигнала через слои после входного слоя.
        /// </summary>
        private void FeedForwardAddLayersAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                var previousLayerSignals = Layers[i - 1].GetSignals();

                foreach (var neuron in layer.Neurons)
                {
                    neuron.FeedForward(previousLayerSignals);
                }
            }
        }

        /// <summary>
        /// Отправляет входные сигналы на входные нейроны сети.
        /// </summary>
        /// <param name="inputSignals">Входные сигналы.</param>
        private void SendSignalsToInputNeurons(params double[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                var signal = new List<double>() { inputSignals[i] };
                var neuron = Layers[0].Neurons[i];

                neuron.FeedForward(signal);
            }
        }

        /// <summary>
        /// Создает выходной слой нейронов.
        /// </summary>
        private void CreateOutputLayer()
        {
            var outputNeurons = new List<Neuron>();
            var lastLayer = Layers.Last();
            for (int i = 0; i < Topology.InputCount; i++)
            {
                var neuron = new Neuron(lastLayer.NeuronCount, NeuronType.Output);
                outputNeurons.Add(neuron);
            }
            var outputLayer = new Layer(outputNeurons, NeuronType.Output);
            Layers.Add(outputLayer);
        }

        /// <summary>
        /// Создает скрытые слои нейронов.
        /// </summary>
        private void CreateHiddenLayers()
        {
            for (int j = 0; j < Topology.HiddenLayers.Count; j++)
            {
                var hiddenNeurons = new List<Neuron>();
                var lastLayer = Layers.Last();
                for (int i = 0; i < Topology.HiddenLayers[j]; i++)
                {
                    var neuron = new Neuron(lastLayer.NeuronCount);
                    hiddenNeurons.Add(neuron);
                }
                var hiddenLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddenLayer);
            }
        }

        /// <summary>
        /// Создает входной слой нейронов.
        /// </summary>
        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();
            for (int i = 0; i < Topology.InputCount; i++)
            {
                var neuron = new Neuron(1, NeuronType.Input);
                inputNeurons.Add(neuron);
            }
            var inputLayer = new Layer(inputNeurons, NeuronType.Input);
            Layers.Add(inputLayer);
        }
    }
}