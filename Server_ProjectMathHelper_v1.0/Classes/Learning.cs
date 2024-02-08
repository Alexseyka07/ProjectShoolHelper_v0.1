using SchoolChatGPT_v1._0.NeuralNetworkClasses;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public class Learning
    {
        public NeuralNetworkRepository NeuralNetworkRepository { get; }

        private List<Tuple<double, NeuralNetwork>> neuralNetworksResult;

        public Learning(NeuralNetworkRepository neuralNetworkRepository)
        {
            NeuralNetworkRepository = neuralNetworkRepository;
            neuralNetworksResult = new List<Tuple<double, NeuralNetwork>>();
        }

        public List<Tuple<double, NeuralNetwork>> StartLearning(List<NeuralNetwork> listNeuralNetworks, int epoch)
        {
            var neuralNetworks = new Tuple<List<NeuralNetwork>, int>(listNeuralNetworks, epoch);
            foreach (var neuralNetwork in neuralNetworks.Item1)
            {
                new Thread(Learn).Start(new Tuple<NeuralNetwork, int>(neuralNetwork, epoch));
                Thread.Sleep(100);
            }
            return neuralNetworksResult;
        }

        public List<Tuple<double, NeuralNetwork>> StartLearning(NeuralNetwork neuralNetwork, int epoch)
        {
            Thread thread = new Thread(Learn);
            thread.Start(new Tuple<NeuralNetwork, int>(neuralNetwork, epoch));
            thread.Join();
            return neuralNetworksResult;
        }

        private void Learn(object obj)
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

        private Tuple<double, NeuralNetwork> Learn(NeuralNetwork neuralNetwork, int epoch)
        {
            Console.WriteLine($"[NEURALNETWORK: LEARNINGRATE:{neuralNetwork.Topology.LearningRate}, HIDDENNEURONS: {neuralNetwork.Layers[1].NeuronCount}] Learning START");
            var error = NeuralNetworkRepository.Learn(neuralNetwork, epoch);
            Info(neuralNetwork, error);
            return new Tuple<double, NeuralNetwork>(error, neuralNetwork);
        }

        private void Info(NeuralNetwork neuralNetwork, double error)
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