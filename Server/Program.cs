using Server;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Linq;
using System.Threading;
using System.Reflection.Metadata;
using Microsoft.Win32;

List<User> users = new List<User>();

// read all users and their notes
string path = @"C:\Work\vs\calendar\Server\db.txt";
string[] file = File.ReadAllLines(path);

User currentUser;
Note currentNote;

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
            content += " ";
        }
        Note bufferNote = new Note(Convert.ToDateTime(file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]), file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[2], content);
        users[users.Count - 1].notes.Add(bufferNote);
    }
    else
    {
        Console.WriteLine("Error DB");
    }
}

// write on console users and their notes
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
    localPort = 8889;
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
                Thread.Sleep(20);
                string message = Encoding.Unicode.GetString(buffer);
                string[] messages = message.Split(' ', StringSplitOptions.RemoveEmptyEntries); // words
                string response = "response";
                switch (messages[0]) // first word
                {
                    case "LOGIN": // LOGIN USERNAME PASSWORD
                        if (messages.Length == 3)
                        {
                            for (int i = 0; i < users.Count; i++)
                            {
                                if (messages[1] == users[i].Login && messages[2] == users[i].Password)
                                {
                                    response = "LOGIN SUCCESS " + users[i].Login + " " + users[i].Password;
                                    for (int ii = 0; ii < users[i].notes.Count; ii++)
                                    {
                                        response += " " + users[i].notes[ii].GetNote();
                                    }
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
                            response = "LOGIN FAIL NOT 3 WORDS";
                        }
                        break;
                    case "REGISTER": // REGISTER LOGIN PASSWORD
                        if (messages.Length == 3)
                        {
                            for (int i = 0; i < users.Count; i++)
                            {
                                if (users[i].Login == messages[1] && users[i].Password == messages[2])
                                {
                                    response = "REGISTER FAIL";
                                    break;
                                }
                            }
                            if (response != "REGISTER FAIL")
                            {
                                File.AppendAllText(path, "\nUSER " + messages[1] + " " + messages[2]);
                                response = "REGISTER SUCCESS";
                            }
                        }
                        else
                        {
                            response = "REGISTER FAIL INCORRECT REQUEST";
                        }
                        break;
                    case "CREATE": // CREATE LOGIN PASSWORD NOTE date caption content
                        if (messages[3] != "NOTE")
                        {
                            response = "CREATE FAIL";
                        }
                        else 
                        {
                            DBUpdate(message);
                            response = "CREATE SUCCESS " + currentNote.GetNote();
                        }
                        break;
                    case "UPDATE":
                        // send client currentNote and make client rewrite user.notes[index]
                        break;
                    case "DELETE":
                        // send client currentNote to delete
                        break;
                    default:
                        response = "INVALID";
                        break;
                }
                byte[] data = Encoding.Unicode.GetBytes(response);
                sender.Send(data, data.Length, remoteAddress, remotePort);
                Console.WriteLine("{0} Send response {1}:{2}\n{3}", DateTime.Now.ToLongTimeString(), remoteAddress, remotePort, response);

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
        Console.WriteLine(ex.Message);
    }
    finally
    {
        sender.Close();
        Console.WriteLine("\nsender closed");
    }
}

void ReceiveMessage()
{
    UdpClient receiver = new UdpClient(localPort);
    IPEndPoint remoteEndPoint = null;

    try
    {
        Console.WriteLine("\nUdp server " + localPort.ToString() + " started...");
        while (true)
        {
            byte[] data = receiver.Receive(ref remoteEndPoint);
            remoteAddress = remoteEndPoint.Address.ToString();
            remotePort = remoteEndPoint.Port;
            string message = Encoding.Unicode.GetString(data);
            Console.WriteLine("{0} Received message {1}:{2}\n{3}", DateTime.Now.ToLongTimeString(), remoteAddress, remotePort, message);
            buffer = data;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        receiver.Close();
        Console.WriteLine("\nreceiver closed");
    }
}

void DBUpdate(string message) // read file, create/update/delete note, rewrite file (local and then in db), show on console, send to user outside this method!!!
{
    string[] messages = message.Split(' ', StringSplitOptions.RemoveEmptyEntries); // words
    currentUser = new User(messages[1], messages[2]);

    currentNote = new Note(DateTime.Parse("10/10/2010"), "0", "0");
    for (int i = 0; i < messages.Length; i++)
    {
        string content = "";
        for (int ii = 6; ii < messages.Length; ii++)
        {
            content += messages[ii];
            content += " ";
        }
        currentNote = new Note(Convert.ToDateTime(messages[4]), messages[5], content);
    }
    // Console.WriteLine("\ncurrentUser=" + currentUser.GetUser() + "\ncurrentNote=" + currentNote.GetNote());

    // rewtire local variable file 
    file = File.ReadAllLines(path);
    switch (messages[0])
    {
        case "CREATE":
            int createPosition = -1;
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i].Substring(5, currentUser.Login.Length) == currentUser.Login && file[i].Substring(6 + currentUser.Login.Length) == currentUser.Password) 
                {
                    file[i] += "\n" + currentNote.GetNote();
                    break;
                }
            }
            break;
        case "DELETE":
            break;
        case "UPDATE":
            break;
        default:
            Console.WriteLine("Erroro void DBUpdate(string message) switch default");
            break;
    }

    // rewrite file db
    File.WriteAllText(path, string.Empty);
    File.AppendAllLines(path, file);

    // write on console users and their notes
    //Console.WriteLine("\nDB was edited:");
    //for (int i = 0; i < users.Count; i++)
    //{
    //    Console.WriteLine(users[i].GetUser());
    //    for (int ii = 0; ii < users[i].notes.Count; ii++)
    //    {
    //        Console.WriteLine(users[i].notes[ii].GetNote());
    //    }
    //}
    //Console.WriteLine("\n");
}

// void CreateUserNote() { }
// void UpdateUserNote() { }
// void DeleteUserNote() { }