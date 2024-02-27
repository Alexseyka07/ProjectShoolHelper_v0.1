using Repository.Models;
using Repository.Data;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using Repository;
using Server_ProjectMathHelper_v1._0.Models;

public static class Program
{
    private static List<SecondNeuralNetwork> secondNeuralNetworks = new List<SecondNeuralNetwork>();
    private static void Main(string[] args)
    {
        NetRepository netRepository = new NetRepository();
        netRepository.Connect();

    }
}