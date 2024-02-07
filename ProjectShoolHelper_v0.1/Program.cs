using ProjectShoolHelper_v0._1;
using RepositoryDb.Models;
using RepositoryDb.Repository;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using SetWordsForNeuralNetwork;
using System.ComponentModel.Design;

namespace NNAnswerTheQuestion
{
    public class Program
    {
        private static NeuralNetwork neuralNetwork;
        private static Topology topology;
        private static DataNeuralNetwork dataNeuralNetwork;
        private static Data data;
        private static string dataName;
        private static string dataNNName;
        private static double error;
        private static List<Tuple<double, NeuralNetwork>> neuralNetworks = new List<Tuple<double, NeuralNetwork>>();
        private static void Main(string[] args)
        {
            SetSentensesForData();
            Thread[] threads = new Thread[6] 
            {
            new Thread(Init1), new Thread(Init2),
            new Thread(Init3), new Thread(Init4),
            new Thread(Init5), new Thread(Init6)
            };
            foreach (var thread in threads)
            {
                thread.Start();
                Thread.Sleep(200);
            }
            Console.WriteLine($"[DATA] InputNeuronsCount: {data.WordsData.Count}");
            Console.WriteLine($"[DATA] TrainingDataCount: {data.TrainingData.Count}");
            Console.ReadLine();
        }

        private List<Rule> GetDb()
        {
            return new TheoryRepository().AllRules();
        }

        private static void Init1()
        {
            Topology topology = new Topology(inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.6, layers: new int[] { 30 });
            //dataNeuralNetwork = new DataNeuralNetwork(dataNNName, topology);
            Learning(topology);
        }
        private static void Init2()
        {
            Topology topology = new Topology(inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.6, layers: new int[] { 50 });
            //dataNeuralNetwork = new DataNeuralNetwork(dataNNName, topology);
            Learning(topology);
        }
        private static void Init3()
        {
            Topology topology = new Topology(inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.6, layers: new int[] { 70 });
            //dataNeuralNetwork = new DataNeuralNetwork(dataNNName, topology);
            Learning(topology);
        }
        private static void Init4()
        {
            Topology topology = new Topology(inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.4, layers: new int[] { 30 });
            //dataNeuralNetwork = new DataNeuralNetwork(dataNNName, topology);
            Learning(topology);
        }
        private static void Init5()
        {
            Topology topology = new Topology(inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.4, layers: new int[] { 50 });
            //dataNeuralNetwork = new DataNeuralNetwork(dataNNName, topology);
            Learning(topology);
        }
        private static void Init6()
        {
            Topology topology = new Topology(inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.4, layers: new int[] { 70 });
            //dataNeuralNetwork = new DataNeuralNetwork(dataNNName, topology);
            Learning(topology);
        }
        private static void Learning(Topology topology)
        {
            double error = FirstLearning(topology);
            //Console.WriteLine($"Ошибка после обучения: {error}");

            /*Thread thread = new Thread(Working);
            thread.Start();*/
        }

        private static void Working(NeuralNetwork neuralNetwork)
        {
            Console.WriteLine("[WORKING]");
            while (true)
            {
                var text = Console.ReadLine();
                if (text == "l")
                {
                    Console.WriteLine("Введитме количество эпох обучения");
                    error = neuralNetwork.Learn(data.TrainingData, epoch: int.Parse(Console.ReadLine()));

                    Console.WriteLine($"Ошибка после обучения: {error}");
                }
                else if (text == "stop") break;
                else if (text == "t")
                {
                    int res = 0;

                    foreach (var item in data.TrainingData)
                    {
                        Neuron outputNeuron1 = neuralNetwork.FeedForward(item.Item2);
                        var resNeuron = outputNeuron1.Output;
                        if (item.Item1 == FindClosestNumber(resNeuron))
                            res++;
                    }
                    Console.WriteLine(res + "/" + data.TrainingData.Count);
                }
                else if (text == "s")
                {
                    dataNeuralNetwork.SetData(neuralNetwork.Layers);
                    Console.WriteLine("Save finish");
                }
                else
                {
                    Neuron outputNeuron1 = neuralNetwork.FeedForward(text, data.WordsData);
                    var res = outputNeuron1.Output;
                    Console.WriteLine(FindClosestNumber(res));
                }
            }
            Menu();
        }

        private static double FindClosestNumber( double target)
        {
            double[] numbers = { 0.13,0.16, 0.19 };

            double closestNumber = numbers[0];
            double minDifference = Math.Abs(numbers[0] - target);

            foreach (double number in numbers)
            {
                double difference = Math.Abs(number - target);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestNumber = number;
                }
            }

            return closestNumber;
        }

        private static double FirstLearning(Topology topology)
        {
            double error = 100;

           
            
              NeuralNetwork  neuralNetwork = new NeuralNetwork(topology);
            Console.WriteLine($"[LEARNING]start: LearningRate: {topology.LearningRate}, HiddenNeurons: {neuralNetwork.Layers[1].NeuronCount}");
            error = neuralNetwork.Learn(data.TrainingData, epoch: 100);
            neuralNetworks.Add(new Tuple<double, NeuralNetwork>(error,neuralNetwork));
            if(neuralNetworks.Count == 6)
            {
                foreach (var item in neuralNetworks)
                {
                    Console.WriteLine("----------------------------------------------------------------");                  
                    Console.WriteLine($"[DATA] Learning Rate: {topology.LearningRate}");
                    Console.WriteLine("[LEARNING] learning end");
                    Console.WriteLine($"[NN Learning Rate {item.Item2.Topology.LearningRate}]");
                    Console.WriteLine($"[NN Hidden Neurons {item.Item2.Layers[1].NeuronCount}]");
                    Console.WriteLine($"[LEARNING] error: {item.Item1}");
                    Console.WriteLine("-----------------------------------------------------------------");
                }
                Menu();
            }
           

            

            return error;
        }
        private static void Menu()
        {
            while(true)
            {
                try
                {
                    Console.WriteLine("Что бы начать работу напишите номер нейросети");
                    var input = int.Parse(Console.ReadLine());
                    Working(neuralNetworks[input].Item2);
                }
                catch (Exception)
                {

                    Console.WriteLine("еще раз");
                }
            }
        }
        private static bool SaveData(Data d)
        {
            try
            {
                data.SetData(d.WordsData, d.TrainingData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void SetSentensesForData()
        {
            string result = SetDataInTuple();
            data = SetSentenses.SetSentences(result);

            SaveData(data);
        }

        private static string SetDataInTuple()
        {
            string result = "";
            List<Tuple<double, string>> trainingData = SetDataQuestions.SetWords();
            foreach (var item in trainingData)
            {
                result += item.Item2 + " $" + item.Item1 + ": ";
            }
            return result;
        }

        private static string SetDataInConsole()
        {
            string result = "";
            string input = "";
            string expected = "";
            while (true)
            {
                Console.WriteLine("Введите вопрос");
                input = Console.ReadLine();
                if (input == "stop")
                    break;
                Console.WriteLine("Введите ожидаемый результат");
                string inputExpected = Console.ReadLine();
                if (inputExpected == null)
                    inputExpected = expected;
                else expected = inputExpected;
                result += input + " $" + expected + ": ";
            }
            return result;
        }
    }
}