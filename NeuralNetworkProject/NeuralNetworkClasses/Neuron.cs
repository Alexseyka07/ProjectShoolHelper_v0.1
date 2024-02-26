using System.Text.Json.Serialization;

namespace SchoolChatGPT_v1._0.NeuralNetworkClasses
{
    public class Neuron
    {
        /// <summary>
        /// Веса связей между нейронами.
        /// </summary>
        public List<double> Weights { get; }

        /// <summary>
        /// Входные сигналы, полученные от предыдущего слоя.
        /// </summary>
        public List<double> Inputs { get; }

        /// <summary>
        /// Тип нейрона (Input, Normal, Output).
        /// </summary>
        public NeuronType NeuronType { get; }

        /// <summary>
        /// Выходное значение нейрона.
        /// </summary>
        public double Output { get; private set; }

        /// <summary>
        /// Дельта, используемая в процессе обучения (ошибка).
        /// </summary>
        public double Delta { get; private set; }

        public int InputCount { get; private set; }

        /// <summary>
        /// Конструктор класса Neuron.
        /// </summary>
        /// <param name="inputCount">Количество входных сигналов (весов).</param>
        /// <param name="neuronType">Тип нейрона (по умолчанию - Normal).</param>
        public Neuron(int inputCount, NeuronType neuronType = NeuronType.Normal)
        {
            NeuronType = neuronType;
            InputCount = inputCount;
            Weights = new List<double>();
            Inputs = new List<double>();

            // Инициализация весов случайными значениями
            InitWeightsRandomValues(InputCount);
        }
        [JsonConstructor]
        public Neuron(List<double> inputs, List<double> weights,int inputCount, NeuronType neuronType = NeuronType.Normal)
        {
            NeuronType = neuronType;
            InputCount = inputCount;
            Weights = weights;
            Inputs = inputs; 
            if(neuronType == NeuronType.Normal) 
            { }

            
        }
        /// <summary>
        /// Инициализация весов случайными значениями.
        /// </summary>
        /// <param name="inputCount">Количество входных сигналов (весов).</param>
        private void InitWeightsRandomValues(int inputCount)
        {
            var random = new Random();
            for (int i = 0; i < inputCount; i++)
            {
                // Если нейрон типа Input, устанавливаем веса равными 1 (нет обучения)
                if (NeuronType == NeuronType.Input)
                {
                    Weights.Add(1);
                }
                else
                {
                    // В противном случае, инициализируем случайными значениями
                    Weights.Add(random.NextDouble());
                }
                Inputs.Add(0);
            }
        }

        /// <summary>
        /// Прямое распространение сигнала через нейрон.
        /// </summary>
        /// <param name="inputs">Входные сигналы.</param>
        /// <returns>Выходное значение нейрона.</returns>
        public double FeedForward(List<double> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                Inputs[i] = inputs[i];
            }
            var sum = 0.0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weights[i];
            }
            Output = sum;

            // Если нейрон не является входным, применяем функцию активации (сигмоид)
            if (NeuronType != NeuronType.Input)
                Output = Sigmoid(sum);
            return Output;
        }

        /// <summary>
        /// Обучение нейрона с учетом ошибки.
        /// </summary>
        /// <param name="error">Ошибка для обучения.</param>
        /// <param name="learningRate">Скорость обучения.</param>
        public void Learn(double error, double learningRate)
        {
            if (NeuronType == NeuronType.Input)
            {
                return;
            }

            var delta = error * SigmoidDx(Output);
            for (int i = 0; i < Weights.Count; i++)
            {
                var weight = Weights[i];
                var input = Inputs[i];

                // Обновление весов с учетом ошибки и скорости обучения
                var newWeight = weight - input * delta * learningRate;
                Weights[i] = newWeight;
            }
            Delta = delta;
        }

        /// <summary>
        /// Функция активации (сигмоид).
        /// </summary>
        /// <param name="x">Входное значение.</param>
        /// <returns>Результат функции активации.</returns>
        private double Sigmoid(double x)
        {
            var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
            return result;
        }

        /// <summary>
        /// Производная функции активации (сигмоид) по входному значению.
        /// </summary>
        /// <param name="x">Входное значение.</param>
        /// <returns>Значение производной.</returns>
        private double SigmoidDx(double x)
        {
            var sigmoid = Sigmoid(x);
            var result = sigmoid * (1 - sigmoid);
            return result;
        }
    }
}