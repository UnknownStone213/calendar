using System.Net.Sockets;
using System.Text;

namespace Client
{
    public partial class Form1 : Form
    {
        string enter;

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
            enter = textBox1.Text;
            using TcpClient tcpClient = new TcpClient();
            {
                tcpClient.ConnectAsync("127.0.0.1", 8899);
                var stream = tcpClient.GetStream();
                byte[] buffer = new byte[4096];

                stream.WriteAsync(Encoding.UTF8.GetBytes(enter + '\n'));
                stream.ReadAsync(buffer, 0, buffer.ToArray().Length);
                string exit = buffer.ToString();
                textBox1.Text = exit;
            }
        }
    }
}