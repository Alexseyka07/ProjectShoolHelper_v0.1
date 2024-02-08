using SchoolChatGPT_v1._0.NeuralNetworkClasses;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public class Working
    {
        private NeuralNetworkRepository neuralNetworkRepository;

        public Working(NeuralNetworkRepository neuralNetworkRepository)
        {
            this.neuralNetworkRepository = neuralNetworkRepository;
        }

        public double Work(string input, NeuralNetwork neuralNetwork)
        {
            return neuralNetworkRepository.Work(neuralNetwork, input);
        }
    }
}