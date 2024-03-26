using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Models;
using System.Linq.Expressions;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public class UIManager
    {      
        private  List<NeuralNetwork> neuralNetworks;
        private List<SecondNeuralNetwork> neuralNetworkModels;
        private NeuralNetworkRepository neuralNetworkRepository = new NeuralNetworkRepository();
    
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
        public void UIMenu(List<SecondNeuralNetwork> neuralNetworkModels)
        {
            this.neuralNetworkModels = neuralNetworkModels;
            while (true)
            {
                Console.WriteLine("Что бы начать работу напишите номер нейросети");
                Console.WriteLine("Info");
                foreach (var model in neuralNetworkModels)
                {
                    Console.WriteLine("Info");
                    Console.WriteLine($"[NN {model.Data.LearningRate} Name:{model.JsonName}]");

                }
                int input;
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    try { UIWork(neuralNetworkModels[input - 1]); }                   
                    catch{ Console.WriteLine("Попробуйте ещё раз"); }
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
                    Data data = new Data()
                    {
                        WordsData = neuralNetwork.Topology.WordsData,
                        TrainingData = neuralNetwork.Topology.TrainingData,
                        LearningRate = neuralNetwork.Topology.LearningRate,
                        FavoriteWords = neuralNetwork.Topology.FavoriteWords,
                        Name = neuralNetwork.Topology.JsonName,
                        Layers = neuralNetwork.Layers
                    };
                   
                    data.SetData();
                }
                else
                {
                    Console.WriteLine(Work(neuralNetwork, input));
                }
            }
            UIMenu();
        }
        public void UIWork(NeuralNetworkModel modelneuralNetwork)
        {
            Console.WriteLine("[WORKING]");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "l")
                {
                    Console.WriteLine($"Ошибка после обучения: {modelneuralNetwork.Learn(int.Parse(Console.ReadLine()))}");
                }
                else if (input == "stop") break;
                else if (input == "t")
                {
                    Console.WriteLine(Testing(modelneuralNetwork.NeuralNetwork) + "/" + modelneuralNetwork.NeuralNetwork.Topology.TrainingData.Count);
                }
                else if (input == "save")
                {
                    modelneuralNetwork.SaveData();
                }
                else
                {
                    Console.WriteLine(modelneuralNetwork.Work(input));
                }
            }
        }
        private double Work(NeuralNetwork neuralNetwork, string input)
        {
            return 0.1;
        }

        private double Learn(NeuralNetwork neuralNetwork, int epoch)
        {
            Console.WriteLine("Введитме количество эпох обучения");
            return 0.2;
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