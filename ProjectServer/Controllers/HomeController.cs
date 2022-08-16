using Microsoft.AspNetCore.Mvc;
using ProjectServer.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProjectServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        [HttpPost]
        [ActionName("SendData")]
        public async Task<IActionResult> SendData(string Name)
        {
            TcpClient client = new TcpClient();
            UserModel user = new UserModel();
            XmlSerializer xmlSubmit = new XmlSerializer(typeof(UserModel));

            user.Name = Name;

            await client.ConnectAsync("127.0.0.1", 5555);
            var clientStream = client.GetStream();

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xmlSubmit.Serialize(writer, user);
                    var xml = sww.ToString();

                    var buffer = Encoding.Default.GetBytes(xml);
                    await clientStream.WriteAsync(buffer, 0, xml.Length);

                }
            }
            if (client.ReceiveBufferSize > 0)
            {
                var buffer = new byte[client.ReceiveBufferSize];

                await clientStream.ReadAsync(buffer, 0, client.ReceiveBufferSize);

                var clientName = Encoding.ASCII.GetString(buffer);

                var serializer = new XmlSerializer(typeof(UserModel));

                using (TextReader reader = new StringReader(Encoding.Default.GetString(buffer)))
                {
                    user = serializer.Deserialize(reader) as UserModel;
                    ViewData["Name"] = user.Name;
                    ViewData["Password"] = user.Password;
                    ViewData["Age"] = user.Age;

                    return View("ResponceView");
                }
            }

            return View();
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ResponceView()
        {
            return View("ResponceView");
        }
        public IActionResult HomeView()
        {
            return View("HomeView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}