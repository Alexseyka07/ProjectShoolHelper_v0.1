using SchoolChatGPT_v1._0.NeuralNetworkClasses;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public class UIManager
    {
        private NeuralNetworkRepository neuralNetworkRepository;

        private List<NeuralNetwork> neuralNetworks;

        public UIManager(NeuralNetworkRepository neuralNetworkRepository)
        {
            this.neuralNetworkRepository = neuralNetworkRepository;
        }

        public void UIMenu(List<NeuralNetwork> neuralNetworks)
        {
            this.neuralNetworks = neuralNetworks;
            while (true)
            {
                Console.WriteLine("Что бы начать работу напишите номер нейросети");
                int input;
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    UIWork(neuralNetworks[input]);
                }
                else Console.WriteLine("Попробуйте ещё раз");
            }
        }

        private void UIWork(NeuralNetwork neuralNetwork)
        {
            Console.WriteLine("[WORKING]");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "l")
                {
                    Console.WriteLine($"Ошибка после обучения: {Learning(neuralNetwork, int.Parse(Console.ReadLine()))}");
                }
                else if (input == "stop") break;
                else if (input == "t")
                {
                    Console.WriteLine(Testing(neuralNetwork) + "/" + neuralNetwork.Topology.TrainingData.Count);
                }
                else
                {
                    Console.WriteLine(Working(neuralNetwork, input));
                }
            }
            UIMenu();
        }

        private double Working(NeuralNetwork neuralNetwork, string input)
        {
            return neuralNetworkRepository.FindClosestOutput(new Working(neuralNetworkRepository).Work(input, neuralNetwork), neuralNetwork.Topology.TrainingData);
        }

        private double Learning(NeuralNetwork neuralNetwork, int epoch)
        {
            Console.WriteLine("Введитме количество эпох обучения");
            return new Learning(neuralNetworkRepository).StartLearning(neuralNetwork, epoch)[0].Item1;
        }

        private int Testing(NeuralNetwork neuralNetwork)
        {
            var res = 0;
            foreach (var item in neuralNetwork.Topology.TrainingData)
            {
                Neuron outputNeuron1 = neuralNetwork.FeedForward(item.Item2);
                var resNeuron = outputNeuron1.Output;
                if (item.Item1 == neuralNetworkRepository.FindClosestOutput(resNeuron, neuralNetwork.Topology.TrainingData))
                    res++;
            }
            return res;
        }

        private void UIMenu()
        {
            while (true)
            {
                Console.WriteLine("Что бы начать работу напишите номер нейросети");
                int input;
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    UIWork(neuralNetworks[input]);
                }
                else Console.WriteLine("Попробуйте ещё раз");
            }
        }
    }
}