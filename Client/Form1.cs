using Microsoft.VisualBasic.ApplicationServices;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System;
using System.Linq;
using System.Threading;
using System.Reflection.Metadata;
using System.Reflection;

namespace Client
{
    public partial class Form1 : Form
    {
        public string remoteAddress;
        public int remotePort;
        int localPort;

        public UdpClient udpClient = new UdpClient();
        User user = new User("0", "0"); // local user 0 0
        Note currentNote;
        int currentNoteIndex;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Random rng = new Random();
                localPort = rng.Next(999, 20000);

                udpClient = new UdpClient(localPort);

                remotePort = 8889;
                remoteAddress = "127.0.0.1";

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            void ReceiveMessage()
            {
                IPEndPoint remoteEndPoint = null;
                try
                {
                    while (true)
                    {
                        Thread.Sleep(20); 
                        byte[] data = udpClient.Receive(ref remoteEndPoint);
                        string message = Encoding.Unicode.GetString(data);
                        string[] messages = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // words
                        // MessageBox.Show("Received message:\n" + message); // check server resoponse
                        switch (messages[0])
                        {
                            case "LOGIN":
                                if (user.Login != "0" && user.Password != "0")
                                {
                                    MessageBox.Show("You are already logged in");
                                } 
                                else if (messages[1] == "SUCCESS")
                                {
                                    // maybe send my local notes to user and on server  ????????????? just like user was adding notes offline but when registered notes were added
                                    user = new User(messages[2], messages[3]);
                                    labelUser.Invoke(delegate { labelUser.Text = "Logged in successfully"; });

                                    // receive notes
                                    for (int iii = 0; iii < messages.Length; iii++)
                                    {
                                        if (messages[iii] == "NOTE")
                                        {
                                            string content = "";
                                            for (int iiii = iii + 3; iiii < messages.Length; iiii++)
                                            {
                                                if (messages[iiii] == "NOTE")
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    content += messages[iiii];
                                                    content += " ";
                                                }
                                            }
                                            user.notes.Add(new Note(DateTime.Parse(messages[iii + 1]), messages[iii + 2], content));
                                        }
                                    }
                                }
                                else if (message.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1] == "FAIL")
                                {
                                    MessageBox.Show("LOGIN FAIL");
                                }
                                else
                                {
                                    MessageBox.Show("Error switch LOGIN");
                                }
                                break;
                            case "REGISTER":
                                if (messages[1] == "SUCCESS")
                                {
                                    user = new User(textBoxLogin.Text, textBoxPassword.Text);
                                    // add local notes to registered account ?????????????????? if yes i also should add notes to DB
                                    labelUser.Invoke(delegate { labelUser.Text = "Registered successfully"; });
                                }
                                else if(messages[1] == "FAIL")
                                {
                                    MessageBox.Show("Failed to register (user already exists)");
                                }
                                else
                                {
                                    MessageBox.Show("Error switch REGISTER");
                                }
                                break;
                            case "CREATE":
                                if (messages[1] == "SUCCESS")
                                {
                                    string content = "";
                                    for (int i = 5; i < messages.Length; i++)
                                    {
                                        content += messages[i] + " ";
                                    }
                                    user.notes.Add(new Note(DateTime.Parse(messages[3]), messages[4], content));
                                }
                                else if (messages[1] == "FAIL")
                                {
                                    MessageBox.Show("Failed to create new note. (check your writing)");
                                }
                                else
                                {
                                    MessageBox.Show("Error switch CREATE");
                                }
                                break;
                            case "UPDATE":
                                break;
                            case "DELETE":
                                break;
                            case "INVALID":
                                MessageBox.Show("Error incorrect request to server");
                                break;
                            default:
                                MessageBox.Show("Error switch DEFAULT");
                                break;
                        }
                        listBoxNotes.Invoke(delegate { NotesUpdate(); }); // update my notes after receiving server response
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    udpClient.Close();
                }
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;
            string message = "REGISTER " + login + " " + password;
            byte[] data = Encoding.Unicode.GetBytes(message);
            udpClient.Send(data, data.Length, remoteAddress, remotePort);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;
            string message = "LOGIN " + login + " " + password;
            byte[] data = Encoding.Unicode.GetBytes(message);
            udpClient.Send(data, data.Length, remoteAddress, remotePort);
        }

        private void NotesUpdate()
        {
            // sort by date? if yes will i have problems with crudl?
            listBoxNotes.Items.Clear();
            for (int i = 0; i < user.notes.Count; i++)
            {
                listBoxNotes.Items.Add(user.notes[i].GetNote().Substring(5));
            }
        }

        private void buttonNoteCreate_Click(object sender, EventArgs e)
        {
            try
            {
                currentNote = new Note(DateTime.Parse(textBoxNoteDate.Text), textBoxNoteCaption.Text, textBoxNoteContent.Text);
                string message = "CREATE " + user.Login + " " + user.Password + " " + currentNote.GetNote();
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpClient.Send(data, data.Length, remoteAddress, remotePort);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create new note. Check your writing \n\n" + ex.Message);
            }
        }

        private void buttonNoteUpdate_Click(object sender, EventArgs e)
        {

        }

        private void buttonNoteDelete_Click(object sender, EventArgs e)
        {

        }

        private void listBoxNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentNoteIndex = -1;
            for (int i = 0; i < user.notes.Count; i++)
            {
                if (user.notes[i].GetNote().Substring(5) == listBoxNotes.SelectedItem.ToString())
                {
                    currentNoteIndex = i;
                    break;
                }
            }
            currentNote = user.notes[currentNoteIndex];
        }
    }
}