using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Notification;
using NotificationServer;
using Thrift;
using Thrift.Protocols;
using Thrift.Server;
using Thrift.Transports;
using Thrift.Transports.Server;

namespace Server
{
    class Program
    {
        public static int c = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            RunNotificationServiceAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }
        static async Task RunNotificationServiceAsync()
        {
            var fabric = new LoggerFactory().AddConsole(LogLevel.Trace);
            var handler = new NotificationHandler();

            ITAsyncProcessor processor = null;
            TServerTransport serverTransport = null;
            serverTransport = new TServerSocketTransport(9090);

            ITProtocolFactory inputProtocolFactory;
            ITProtocolFactory outputProtocolFactory;
            inputProtocolFactory = new TBinaryProtocol.Factory();
            outputProtocolFactory = new TBinaryProtocol.Factory();
            processor = new NotificationService.AsyncProcessor(handler);

            var server = new AsyncBaseServer(processor, serverTransport, inputProtocolFactory, outputProtocolFactory, fabric);
            await server.ServeAsync(new CancellationToken());
        }
        class NotificationHandler : NotificationService.IAsync
        {

            public async Task<string> GetNotificationsAsync(CancellationToken cancellationToken)
            {
                var notifications = await NotificationHelper.Instance.GetNotificationsAsync(0);
                Console.WriteLine("Req#{0}, Notification:{1}", ++Program.c, notifications);
                return notifications;
            }
        }
    }
}
