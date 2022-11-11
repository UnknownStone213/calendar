using Server;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Transactions;

IPAddress localAddr = IPAddress.Parse("127.0.0.1");
var tcpListener = new TcpListener(localAddr, 8899);

//string path = @"mytxt.txt";
User user = new User("Vlad", "123");
//string s = File.ReadAllText(path);
//Console.WriteLine(s);

try
{
    tcpListener.Start();
    Console.WriteLine("Server started.");

    while (true) 
    {
        using var tcpClient = await tcpListener.AcceptTcpClientAsync();
        Console.WriteLine("tcp client accepted.");
        var stream = tcpClient.GetStream();
        Console.WriteLine("stream opened.");
        var enter = new List<byte>();

        while (stream.ReadByte() != '\n') 
        {
            enter.Add((byte)stream.ReadByte());
        }
        string request = Encoding.UTF8.GetString(enter.ToArray());

        if (request == "END") // END
        {
            break;
        }

        switch (request.Substring(0, request.IndexOf(' ')))
        {
            case "CREATE":
                break;
            case "READ":
                string exit = "a";
                await stream.WriteAsync(Encoding.UTF8.GetBytes(exit));
                break;
            case "UPDATE":
                break;
            case "DELETE":
                break;
            case "LIST":
                break;
            default:
                break;
        }

    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
finally 
{
    tcpListener.Stop();
    Console.WriteLine("Client _________ disconnected.");
}