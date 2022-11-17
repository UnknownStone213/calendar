using Microsoft.VisualBasic.ApplicationServices;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System;
using System.Linq;
using System.Threading;
using System.Reflection.Metadata;

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

            User user;
            List<Note> notes = new List<Note>() { };

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
                        byte[] data = udpClient.Receive(ref remoteEndPoint);
                        string message = Encoding.Unicode.GetString(data);
                        MessageBox.Show("Received message: " + message);
                        switch (message.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0])
                        {
                            case "LOGIN":
                                if (message.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1] == "SUCCESS")
                                {
                                    user = new User(message.Split(' ', StringSplitOptions.RemoveEmptyEntries)[2], message.Split(' ', StringSplitOptions.RemoveEmptyEntries)[3]);
                                    //labelUser.Text = "Logged in successfully"; !!!!!!!!!!!!!!!!!!
                                    buttonLogin.Enabled = false;
                                    buttonRegister.Enabled = false;
                                    MessageBox.Show("notes:" + user.notes.Count.ToString());
                                }
                                else if (message.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1] == "FAIL")
                                {
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Exception switch LOGIN");
                                }
                                break;
                            default:
                                MessageBox.Show("Exception switch DEFAULT");
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