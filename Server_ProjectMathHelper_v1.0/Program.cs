using Repository.Models;
using Repository.Data;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using Repository;
using Server_ProjectMathHelper_v1._0.Models;

public static class Program
{
    private static void Main(string[] args)
    {
        FirstNeuralNetwork firstNeuralNetwork = new FirstNeuralNetwork("TestFirstEtap");
        new UIManager().UIWork(firstNeuralNetwork.NeuralNetwork);
    }

    


}