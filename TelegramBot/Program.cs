using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class Program
{
    private static string token = "6965102213:AAEeF0AZP9H2GeosUSv0Dj27Lzy4qaVWvaM";
    private static TelegramBotClient client;
    private  static void Main(string[] args)
    {
        client = new TelegramBotClient(token);
        client.StartReceiving();
        client.OnMessage += Client_OnMessage;
        Console.ReadLine();
        client.StopReceiving();
    }

    private static async void Client_OnMessage(object? sender, MessageEventArgs e)
    {
        var msg = e.Message;
        if (msg.Text != null)
        {
            Console.WriteLine($"Пришло сообщение с текстом: {msg.Text}");
            await client.SendTextMessageAsync(msg.Chat.Id, "Привет", replyToMessageId: msg.MessageId);
           
        }
    }
}