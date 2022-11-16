using Server;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Linq;
using System.Threading;
using System.Reflection.Metadata;

List<User> users = new List<User>();

// read all users and their notes
string path = @"C:\Work\vs\calendar\Server\db.txt";
string[] file = File.ReadAllLines(path);
for (int i = 0; i < file.Length; i++)
{
    if (file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0] == "USER")
    {
        users.Add(new User(file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1], file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[2]));
    }
    else if (file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0] == "NOTE")
    {
        string line = file[i];
        string content = "";
        for (int ii = 3; ii < line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length; ii++)
        {
            content += line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[ii];
        }
        Note bufferNote = new Note(Convert.ToDateTime(file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]), file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[2], content);
        users[users.Count - 1].notes.Add(bufferNote);
    }
    else
    {
        Console.WriteLine("Exception DB");
    }
}

// check users and their notes
for (int i = 0; i < users.Count; i++)
{
    Console.WriteLine(users[i].GetUser());
    for (int ii = 0; ii < users[i].notes.Count; ii++)
    {
        Console.WriteLine(users[i].notes[ii].GetNote());
    }
}

string remoteAddress = "127.0.0.1";
int remotePort = -1;
int localPort;
byte[] buffer = new byte[65000];

try
{
    Console.Write("\nSet local port: ");
    localPort = Int32.Parse(Console.ReadLine());
    Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
    receiveThread.Start();
    SendMessage();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

void SendMessage()
{
    UdpClient sender = new UdpClient();

    try
    {
        while (true)
        {
            if (remoteAddress != "" && remotePort != -1 && buffer != new byte[65000]) // if buffer = default > returns exception
            {
                Thread.Sleep(1); // without this sleep message does not appear
                string message = Encoding.Unicode.GetString(buffer);
                string response = "response";
                switch (message.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]) // first word
                {
                    case "LOGIN":
                        if (message.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length == 3) // LOGIN USERNAME PASSWORD
                        {
                            for (int i = 0; i < users.Count; i++)
                            {
                                if (message.Split(' ')[1] == users[i].Login && message.Split(' ')[2] == users[i].Password)
                                {
                                    response = "LOGIN SUCCESS. USER " + users[i].Login + " PASSWORD " + users[i].Password;
                                    Console.WriteLine("Client {0}:{1} LOGGED IN successfully", remoteAddress, remotePort);
                                    // give info about notes !!!!!!!!!!!!!!!!!!!!!
                                    break;
                                }
                                else
                                {
                                    response = "LOGIN FAIL";
                                }
                            }
                        }
                        else
                        {
                            response = "LOGIN FAIL. Not 3 words. Write LOGIN USERNAME PASSWORD";
                        }
                        break;
                    default:
                        response = "INVALID";
                        break;
                }
                byte[] data = Encoding.Unicode.GetBytes(response);
                sender.Send(data, data.Length, remoteAddress, remotePort);

                // reset 
                response = "";
                remoteAddress = "";
                remotePort = -1;
                buffer = new byte[65000];
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message + "EXCEPTION void SendMessage()");
    }
    finally
    {
        sender.Close();
    }
}

void ReceiveMessage()
{
    UdpClient receiver = new UdpClient(localPort);
    IPEndPoint remoteEndPoint = null;

    try
    {
        Console.WriteLine("Udp server started...");
        while (true)
        {
            byte[] data = receiver.Receive(ref remoteEndPoint);
            remoteAddress = remoteEndPoint.Address.ToString();
            remotePort = remoteEndPoint.Port;
            string message = Encoding.Unicode.GetString(data);
            Console.WriteLine("Received message from {0}:{1} {2}", remoteAddress, remotePort, message);
            buffer = data;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message + "EXCEPTION void void ReceiveMessage()");
    }
    finally
    {
        receiver.Close();
    }
}


// void CreateUserNote() { }
// void UpdateUserNote() { }
// void DeleteUserNote() { }
// void UpdateDB() { }