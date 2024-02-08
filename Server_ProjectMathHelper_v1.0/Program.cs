using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;

public class Program
{
    private static NeuralNetworkRepository neuralNetworkRepository;
    private static Learning learning;
    private static Data data;

    private static void Main(string[] args)
    {
        Init();
        Start();
    }
    private static void Init()
    {
        neuralNetworkRepository = new NeuralNetworkRepository();
        learning = new Learning(neuralNetworkRepository);
        data = neuralNetworkRepository.SetSentensesForData();
    }
    public static void Start()
    {
        var neuralNetworksList = new List<NeuralNetwork>()
        {
            new NeuralNetwork(new Topology(data, inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.4, layers: new int[] { 30 })),
            new NeuralNetwork(new Topology(data, inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.6, layers: new int[] { 30 })),
        };
        var neuralNetworks = learning.StartLearning(neuralNetworksList, 50);

        var uIManager = new UIManager(neuralNetworkRepository);
        uIManager.UIMenu(neuralNetworksList);
    }
}