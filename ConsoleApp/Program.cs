using System;
using System.Net.Http;
using System.Threading.Tasks;
using gRPC_v2;
using Grpc.Net.Client;
using gRPC_v2;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions()
            {
                HttpHandler = GetHttpClientHandler()
            });

            // создаем клиента
            var client = new Telnet.TelnetClient(channel);
            while (true)
            {
                Console.Write("Напишите название метода, который хотите вызвать: ");
                var message = Console.ReadLine();

                // обмениваемся сообщениями с сервером
                var reply = await client.SendRequestAsync(new Request {Message = message});
                Console.WriteLine(reply.Message);
            }
        }

        private static HttpClientHandler GetHttpClientHandler()
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            return httpHandler;
        }
    }
}