using SchoolChatGPT_v1._0.NeuralNetworkClasses;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public static class Working
    {
        private static NeuralNetworkRepository neuralNetworkRepository = new NeuralNetworkRepository();
        public static double Work(string input, NeuralNetwork neuralNetwork)
        {
            return neuralNetworkRepository.Work(neuralNetwork, input);
        }
    }
}