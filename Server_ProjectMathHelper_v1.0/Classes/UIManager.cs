using SchoolChatGPT_v1._0.NeuralNetworkClasses;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public class UIManager
    {      
        private  List<NeuralNetwork> neuralNetworks;
    
        public  void UIMenu(List<NeuralNetwork> neuralNetworksList)
        {
            neuralNetworks = neuralNetworksList;
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

        public void UIWork(NeuralNetwork neuralNetwork)
        {
            Console.WriteLine("[WORKING]");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "l")
                {
                    Console.WriteLine($"Ошибка после обучения: {Learn(neuralNetwork, int.Parse(Console.ReadLine()))}");
                }
                else if (input == "stop") break;
                else if (input == "t")
                {
                    Console.WriteLine(Testing(neuralNetwork) + "/" + neuralNetwork.Topology.TrainingData.Count);
                }
                else if(input == "save")
                {
                    //Program.data.SetDataNeuralNetwork(neuralNetwork);
                }
                else
                {
                    Console.WriteLine(Work(neuralNetwork, input));
                }
            }
            UIMenu();
        }

        private double Work(NeuralNetwork neuralNetwork, string input)
        {
            return Program.neuralNetworkRepository.FindClosestOutput(Working.Work(input, neuralNetwork), neuralNetwork.Topology.TrainingData);
        }

        private double Learn(NeuralNetwork neuralNetwork, int epoch)
        {
            Console.WriteLine("Введитме количество эпох обучения");
            return Learning.StartLearning(neuralNetwork, epoch)[0].Item1;
        }

        private int Testing(NeuralNetwork neuralNetwork)
        {
            var res = 0;
            foreach (var item in neuralNetwork.Topology.TrainingData)
            {
                Neuron outputNeuron1 = neuralNetwork.FeedForward(item.Item2);
                var resNeuron = outputNeuron1.Output;
                if (item.Item1 == Program.neuralNetworkRepository.FindClosestOutput(resNeuron, neuralNetwork.Topology.TrainingData))
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