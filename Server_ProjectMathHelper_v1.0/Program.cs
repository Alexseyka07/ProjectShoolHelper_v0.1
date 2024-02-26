﻿using Repository.Models;
using Repository.Data;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using Repository;

public static class Program
{
    public static NeuralNetworkRepository neuralNetworkRepository;
    private static NetRepository netRepository;
    public static Data data;

   
    
    private static void Main(string[] args)
    {
        neuralNetworkRepository = new NeuralNetworkRepository();
        data = neuralNetworkRepository.SetSentensesForData();
        var neuralNetwork = new NeuralNetwork(new Topology(data, inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.5, layers: new int[] { 30 }));
        Console.WriteLine(neuralNetworkRepository.Learn(neuralNetwork, 50));
        var uIManager = new UIManager();
        uIManager.UIWork(neuralNetwork);

        
    }

    private static void Init()
    {
        neuralNetworkRepository = new NeuralNetworkRepository();
        data = neuralNetworkRepository.SetSentensesForData();

    }

    private static void Connect()
    {
        netRepository = new NetRepository();
        netRepository.Connect();
    }
    private static void Start()
    {
        var neuralNetworksList = new List<NeuralNetwork>()
        {
            new NeuralNetwork(new Topology(data, inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.4, layers: new int[] { 30 })),
            new NeuralNetwork(new Topology(data, inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.6, layers: new int[] { 30 })),
        };
       

        var uIManager = new UIManager();
        uIManager.UIMenu(neuralNetworksList);
    }


}