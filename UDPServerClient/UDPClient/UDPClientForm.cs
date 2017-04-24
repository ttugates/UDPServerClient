using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace UDPClient
{
    public partial class UDPClientForm : Form
    {
        public UDPClientForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PingServer();
        }

        private void PingServer()
        {
            try
            {
                var client = new UdpClient();
                // endpoint where server is listening
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
                client.Connect(ep);

                // send data
                var data = Encoding.ASCII.GetBytes("Hello from client" + Environment.NewLine);
                client.Send(data, data.Length);

                // then receive data
                var receivedData = client.Receive(ref ep);

                // MessageBox.Show(ep.ToString());
                if (receivedData != null)
                {
                    var receivedMessage = Encoding.ASCII.GetString(receivedData, 0, receivedData.Length);
                    richTextBox1.Text += "Client Received " + receivedMessage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
