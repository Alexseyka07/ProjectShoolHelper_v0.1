using Repository.Models;
using Repository.Data;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using Repository;
using Server_ProjectMathHelper_v1._0.Models;
using Server_ProjectMathHelper_v1._0.Controllers;

public static class Program
{
    private static List<SecondNeuralNetwork> secondNeuralNetworks = new List<SecondNeuralNetwork>();
    private static void Main(string[] args)
    {
        TelegramBotController telegramBotController = new TelegramBotController();
      
    }
    
    private static void LNN2()
    {
        DataDb dataDb = new DataDb();
        for (int i = 1; i < dataDb.Rules.Where(x => x.Id <= 4).ToList().Count + 1; i++)
        {
            secondNeuralNetworks.Add(new SecondNeuralNetwork($"secondNN{i}", i));

        }

        foreach (var neuralNetwork in secondNeuralNetworks)
        {
            Console.WriteLine($"Start {neuralNetwork.JsonName}");
            new Thread(() => { 
                neuralNetwork.Learn(100);
               neuralNetwork.SaveData(); }).Start();
            Thread.Sleep(50);
        }

        
        Console.WriteLine("learning end");
        UIManager uiManager = new UIManager();
        uiManager.UIMenu(secondNeuralNetworks);
    }
}