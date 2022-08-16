using Microsoft.AspNetCore.Routing;
using System.Net;
using System.Text;
using System;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Xml;

namespace SoapService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            TcpListener listener = new(IPAddress.Parse("127.0.0.1"), 5555);

            listener.Start();

            var client = await listener.AcceptTcpClientAsync();

            var clientStream = client.GetStream();

            if (client.ReceiveBufferSize > 0)
            {
                var buffer = new byte[client.ReceiveBufferSize];
                await clientStream.ReadAsync(buffer, 0, client.ReceiveBufferSize);

                var serializer = new XmlSerializer(typeof(UserModel));

                UserModel? result = new();

                using (TextReader reader = new StringReader(Encoding.Default.GetString(buffer)))
                {
                    result = serializer.Deserialize(reader) as UserModel;
                    UsersDBContext db = new();

                    foreach (var item in db!.Users)
                    {
                        if (result?.Name == item.Name)
                        {
                            using (var sww = new StringWriter())
                            {
                                using (XmlWriter writer = XmlWriter.Create(sww))
                                {
                                    serializer.Serialize(writer, item);
                                    var xml = sww.ToString();
                                    await clientStream.WriteAsync(Encoding.ASCII.GetBytes(xml));

                                    app.Run();
                                }
                            }
                        }
                    }

                    using (var sww = new StringWriter())
                    {
                        using (XmlWriter writer = XmlWriter.Create(sww))
                        {
                            result!.Age = 0;
                            result!.Password = "";

                            await db!.Users.AddAsync(result!);
                            await db!.SaveChangesAsync();

                            serializer.Serialize(writer, result);
                            var xml = sww.ToString();
                            await clientStream.WriteAsync(Encoding.ASCII.GetBytes(xml));
                        }
                    }
                }
            }

            app.Run();
        }
    }
}