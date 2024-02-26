using SchoolChatGPT_v1._0.NeuralNetworkClasses;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public static class Learning
    {

        private static List<Tuple<double, NeuralNetwork>> neuralNetworksResult = new List<Tuple<double, NeuralNetwork>>();
        private static NeuralNetworkRepository neuralNetworkRepository = new NeuralNetworkRepository();
       

        public static List<Tuple<double, NeuralNetwork>> StartLearning(List<NeuralNetwork> listNeuralNetworks, int epoch)
        {
            var neuralNetworks = new Tuple<List<NeuralNetwork>, int>(listNeuralNetworks, epoch);
            foreach (var neuralNetwork in neuralNetworks.Item1)
            {
                new Thread(Learn).Start(new Tuple<NeuralNetwork, int>(neuralNetwork, epoch));
                Thread.Sleep(100);
            }
            return neuralNetworksResult;
        }

        public static List<Tuple<double, NeuralNetwork>> StartLearning(NeuralNetwork neuralNetwork, int epoch)
        {
            Thread thread = new Thread(Learn);
            thread.Start(new Tuple<NeuralNetwork, int>(neuralNetwork, epoch));
            thread.Join();
            return neuralNetworksResult;
        }

        private static void Learn(object obj)
        {
            try
            {
                var tuple = obj as Tuple<NeuralNetwork, int>;
                neuralNetworksResult.Add(Learn(tuple.Item1, tuple.Item2));
            }
            catch
            {
                throw new Exception("Неправильно задан tuple");
            }
        }

        private static Tuple<double, NeuralNetwork> Learn(NeuralNetwork neuralNetwork, int epoch)
        {
            Console.WriteLine($"[NEURALNETWORK: LEARNINGRATE:{neuralNetwork.Topology.LearningRate}, HIDDENNEURONS: {neuralNetwork.Layers[1].NeuronCount}] Learning START");
            var error = neuralNetworkRepository.Learn(neuralNetwork, epoch);
            Info(neuralNetwork, error);
            return new Tuple<double, NeuralNetwork>(error, neuralNetwork);
        }

        private static void Info(NeuralNetwork neuralNetwork, double error)
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine($"[NEURALNETWORK: LEARNINGRATE:{neuralNetwork.Topology.LearningRate}, HIDDENNEURONS: {neuralNetwork.Layers[1].NeuronCount}] Learning END");
            Console.WriteLine($"[NEURALNETWORK: LEARNINGRATE:{neuralNetwork.Topology.LearningRate}, HIDDENNEURONS: {neuralNetwork.Layers[1].NeuronCount}] INFO:");
            Console.WriteLine($"[NEURALNETWORK] ERROR: {error}");
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine();
        }
    }
}