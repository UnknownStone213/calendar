using Server;
using System.Net;
using System.Net.Sockets;
using System.Text;

IPAddress localAddr = IPAddress.Parse("127.0.0.1");
var tcpListener = new TcpListener(localAddr, 8888);

string path = @"mytxt.txt";
User user = new User("Vlad", "123");
string s = File.ReadAllText(path);
Console.WriteLine(s);


try
{

}
catch (Exception e)
{

}
finally 
{

}