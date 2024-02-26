using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        var FavoriteWords = new Dictionary<string, double>()
        {
            {"знаменателю", 1 },
            {"дроби", 1.0 },
            {"дробь", 1.0 },
            {"знаменатель", 1 },
            {"знаменателем", 1 },
            {"сократите", 1 },
            {"сократи", 1.0 },
            {"сократима", 1},
            {"приведите",  0.6},
        };
        var json = JsonSerializer.Serialize(FavoriteWords);
        Console.WriteLine(json);
    }
}