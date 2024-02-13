using SchoolChatGPT_v1._0.NeuralNetworkClasses;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public static class Working
    {
        public static double Work(string input, NeuralNetwork neuralNetwork)
        {
            return Program.neuralNetworkRepository.Work(neuralNetwork, input);
        }
    }
}