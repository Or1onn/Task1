using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PcComponents
{
    internal class Server
    {
        public PCContext pc { get; set; } = new();

        public MainWindow Window { get; set; }

        public Server(MainWindow window)
        {
            Window = window;
        }

        public void Connect()
        {
            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint localIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            listeningSocket.Bind(localIP);

            while (true)
            {
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                byte[] data = new byte[1024];

                EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                if (listeningSocket.Available > 0)
                {
                    bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));


                    foreach (var item in pc.Accessories)
                    {
                        if (builder.ToString() == item.Make)
                        {
                            Window.Make.Text = item.Make;
                            Window.Model.Text = item.Model;
                            Window.Category.Text = item.Category;
                            Window.Category.Text = item.Year.ToString();
                            Window.Year.Text = item.Year.ToString();
                            Window.Price.Text = item.Price.ToString();
                        }
                    }

                }
            }
        }
    }
}
