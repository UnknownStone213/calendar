using System.Net.Sockets;

namespace Client
{
    public partial class Form1 : Form
    {
        string a;

        public Form1()
        {
            InitializeComponent();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            a = textBox1.Text;
            using TcpClient tcpClient = new TcpClient();
            {
                tcpClient.ConnectAsync("127.0.0.1", 8899);
            }
        }
    }
}