using Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Protocols;
using Thrift.Transports;
using Thrift.Transports.Client;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfExecutedTasks = 0;
            int numberOfRequests = 50;
            for (int i = 0; i < numberOfRequests; i++)
            {
                int c = i;
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    Thread.CurrentThread.IsBackground = false;
                    var client = OpenConnectionToServer();
                    var notifications = client.GetNotificationsAsync(new CancellationToken()).GetAwaiter().GetResult();
                    if (notifications == null)
                    {
                        Console.WriteLine("couldn't get notification for client #{0}", c);
                    }
                    Console.WriteLine("notifs of client #{0}: {1}", c, notifications);
                    numberOfExecutedTasks++;
                });
            }
            while (numberOfExecutedTasks < numberOfRequests) ;
        }
        static NotificationService.Client OpenConnectionToServer()
        {
            var transport = new TSocketClientTransport(IPAddress.Loopback, 9091);
            var protocol = new TBinaryProtocol(transport);
            var client = new NotificationService.Client(protocol);

            return client;
        }
    }
}
