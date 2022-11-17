using Microsoft.VisualBasic.ApplicationServices;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System;
using System.Linq;
using System.Threading;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Client
{
    public partial class Form1 : Form
    {
        public string remoteAddress;
        public int remotePort;

        public UdpClient udpClient = new UdpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int localPort;

            User user = new User("0", "0"); // local user 0 0
            // List<Note> notes = new List<Note>() { };

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
                        MessageBox.Show("Received message:\n" + message); // check server resoponse
                        switch (messages[0])
                        {
                            case "LOGIN":
                                if (user.Login != "0" && user.Password != "0")
                                {
                                    MessageBox.Show("You are already logged in");
                                } 
                                else if (messages[1] == "SUCCESS")
                                {
                                    // maybe send my local notes to server ???????????
                                    user = new User(messages[2], messages[3]);

                                    labelUser.Invoke(delegate { labelUser.Text = "Logged in successfully"; });

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

                                    MessageBox.Show("LOGIN SUCCESS");
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
                            case "INVALID":
                                MessageBox.Show("Error incorrect request to server");
                                break;
                            default:
                                MessageBox.Show("Error switch DEFAULT");
                                break;
                        }
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

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;
            string message = "LOGIN " + login + " " + password;
            byte[] data = Encoding.Unicode.GetBytes(message);
            udpClient.Send(data, data.Length, remoteAddress, remotePort);
        }

        private void textBoxLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}