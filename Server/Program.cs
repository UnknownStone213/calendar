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
Note currentNote = new Note(DateTime.Parse("10/10/2010"), "0", "0");

for (int i = 0; i < file.Length; i++)
{
    if (file[i] == string.Empty)
    {

    } 
    else if (file[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0] == "USER")
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
Console.WriteLine("\n--- DB ---");
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
                // reread all users and their notes
                string message = Encoding.Unicode.GetString(buffer); // client request
                string[] messages = message.Split(' ', StringSplitOptions.RemoveEmptyEntries); // client request split in words
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
                    case "UPDATE": // UPDATE LOGIN PASSWORD FIRST NOTE date caption content SECOND NOTE ... d c c !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        if (messages[3] != "FIRST" || message.IndexOf("SECOND") == -1)
                        {
                            response = "UPDATE FAIL";
                        }
                        else
                        {
                            DBUpdate(message);
                            response = "UPDATE SUCCESS " + currentNote.GetNote();
                        }
                        break;
                    case "DELETE": // DELETE LOGIN PASSWORD NOTE date caption content
                        if (messages[3] != "NOTE")
                        {
                            response = "DELETE FAIL";
                        }
                        else
                        {
                            DBUpdate(message);
                            response = "DELETE SUCCESS " + currentNote.GetNote();
                        }
                        break;
                    default:
                        response = "INVALID";
                        break;
                }
                byte[] data = Encoding.Unicode.GetBytes(response);
                sender.Send(data, data.Length, remoteAddress, remotePort);
                Console.WriteLine("{0} Send response {1}:{2}\n{3}", DateTime.Now.ToLongTimeString(), remoteAddress, remotePort, response);

                // reset and wait for the next client-request
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

void DBUpdate(string message) // read db, rewrite variable, rewrite db
{
    string[] messages = message.Split(' ', StringSplitOptions.RemoveEmptyEntries); // client's request split in words
    currentUser = new User(messages[1], messages[2]);

    // rewtire variable string[] file 
    file = File.ReadAllLines(path);
    switch (messages[0])
    {
        case "CREATE":
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

            // change file
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i].Substring(5, currentUser.Login.Length) == currentUser.Login && file[i].Substring(6 + currentUser.Login.Length) == currentUser.Password) 
                {
                    file[i] += "\n" + currentNote.GetNote();
                    break;
                }
            }
            break;
        case "UPDATE": // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Note secondNote = new Note(DateTime.Parse("10/10/2010"), "0", "0");
            for (int i = 0; i < messages.Length; i++)
            {
                int secondNoteIndex = -1;
                string content = "";
                string content2 = "";
                for (int ii = 7; ii < messages.Length; ii++)
                {
                    if (messages[ii] == "SECOND")
                    {
                        secondNoteIndex = ii;
                        // find second note
                        for (int iii = ii + 4; iii < messages.Length; iii++)
                        {
                            content2 += messages[iii];
                            content2 += " ";
                        }
                        secondNote = new Note(DateTime.Parse(messages[secondNoteIndex + 2]), messages[secondNoteIndex + 3], content2);
                        break;
                    }
                    else
                    {
                        content += messages[ii];
                        content += " ";

                    }
                }
                currentNote = new Note(Convert.ToDateTime(messages[5]), messages[6], content);
                break;
            }

            // change file
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] != string.Empty && file[i].Substring(0, 4) == "USER")
                {
                    if (file[i].Substring(5, currentUser.Login.Length) == currentUser.Login && file[i].Substring(6 + currentUser.Login.Length) == currentUser.Password)
                    {
                        for (int ii = i + 1; ii < file.Length; ii++)
                        {
                            try
                            {
                                // last && ( _ || _ ) is me solving problem with extra space at the end of currentNote.Content
                                if (currentNote.Date.ToString("MM/dd/yyyy") == file[ii].Substring(5, 10) && currentNote.Caption == file[ii].Substring(16, currentNote.Caption.Length) && (currentNote.Content == file[ii].Substring(17 + currentNote.Caption.Length) || currentNote.Content.Substring(0, currentNote.Content.Length - 1) == file[ii].Substring(17 + currentNote.Caption.Length)))
                                {
                                    file[ii] = secondNote.GetNote();
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                                // line might be bigger and will throw exception OutOfIndex
                            }
                        }
                    }
                }
            }
            break;
        case "DELETE":
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

            // change file
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] != string.Empty && file[i].Substring(0, 4) == "USER")
                {
                    if (file[i].Substring(5, currentUser.Login.Length) == currentUser.Login && file[i].Substring(6 + currentUser.Login.Length) == currentUser.Password)
                    {
                        for (int ii = i + 1; ii < file.Length; ii++)
                        {
                            try
                            {
                                // last && ( _ || _ ) is me solving problem with extra space at the end of currentNote.Content
                                if (currentNote.Date.ToString("MM/dd/yyyy") == file[ii].Substring(5, 10) && currentNote.Caption == file[ii].Substring(16, currentNote.Caption.Length) && (currentNote.Content == file[ii].Substring(17 + currentNote.Caption.Length) || currentNote.Content.Substring(0, currentNote.Content.Length - 1) == file[ii].Substring(17 + currentNote.Caption.Length)))
                                {
                                    file[ii] = string.Empty;
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                                // line might be bigger and will throw exception OutOfIndex
                            }
                        }
                        break;
                    }
                }
            }
            break;
        default:
            Console.WriteLine("Erroro void DBUpdate(string message) switch default");
            break;
    }

    // rewrite db (+ delete empty lines)
    int amountNotEmpty = 0;
    for (int i = 0; i < file.Length; i++)
    {
        if (file[i] != string.Empty)
        {
            amountNotEmpty++;
        }
    }
    string[] fileBuffer = new string[amountNotEmpty];
    int index = 0;
    for (int i = 0; i < file.Length; i++) 
    {
        if (file[i] != string.Empty)
        {
            fileBuffer[index] = file[i];
            index++;
        }
    }
    File.WriteAllText(path, string.Empty);
    File.AppendAllLines(path, fileBuffer);
}

/*
void GetUsers() 
{
    List<User> users = new List<User>();


}
*/