using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPServer
{
    public partial class UDPServerForm : Form
    {
        public UDPServerForm()
        {
            InitializeComponent();
        }

        private void UDPServerForm_Load(object sender, EventArgs e)
        {
            this.UDPPacketReceivedEvent += ActionWhenEventFired;
            StartUDPServer();
        }

        private void StartUDPServer()
        {
            Task.Run(() =>
            {
                UdpClient udpServer = new UdpClient(11000);

                while (true)
                {
                    // listen on port 11000
                    var remoteEP = new IPEndPoint(IPAddress.Any, 11000);
                    var data = udpServer.Receive(ref remoteEP);

                    var receivedMessage = Encoding.ASCII.GetString(data, 0, data.Length);

                    this.UDPPacketReceivedEvent(this, new MyCustomEventArgs("Received: " + receivedMessage));

                    // Respond
                    data = Encoding.ASCII.GetBytes("Welcome to my test server." + Environment.NewLine);
                    udpServer.Send(data, data.Length, remoteEP); // reply back
                }
            });
        }

        public event EventHandler<MyCustomEventArgs> UDPPacketReceivedEvent;

        public class MyCustomEventArgs : EventArgs
        {
            public MyCustomEventArgs(string _text)
            {
                text = _text;
            }

            public string text { get; set; }
        }

        private void ActionWhenEventFired(object sender, MyCustomEventArgs e)
        {
            UpdateTextBox(e.text);
        }

        public void UpdateTextBox(String text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(UpdateTextBox), text);
                return;
            }
            richTextBox1.Text += text;
        }
    }
}
