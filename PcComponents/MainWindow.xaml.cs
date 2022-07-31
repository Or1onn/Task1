using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PcComponents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public PCContext pc { get; set; } = new();

        private void Window_Initialized(object sender, EventArgs e)
        {
            foreach (var item in pc.Accessories)
            {
                Accessories.Items.Add(item.Make);
            }
        }


        private void Accessories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(address, 5555);
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Server server = new(this);

            socket.Connect(endPoint);
            
            socket.Send(Encoding.ASCII.GetBytes(Accessories.SelectedItem.ToString()));

            //foreach (var item in pc.Accessories)
            //{
            //    if (Accessories.SelectedItem == item.Make)
            //    {
            //        Make.Text = item.Make;
            //        Model.Text = item.Model;
            //        Category.Text = item.Category;
            //        Category.Text = item.Year.ToString();
            //        Year.Text = item.Year.ToString();
            //        Price.Text = item.Price.ToString();
            //    }
            //}
        }
    }
}
