using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationServer
{
    public class NotificationHelper
    {
        private static NotificationHelper instance = null;
        private static readonly object padlock = new object();
        AutoResetEvent notifEvent = new AutoResetEvent(false);
        NotificationHelper() { }

        private static object client;
        public static NotificationHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new NotificationHelper();
                    }
                    return instance;
                }
            }
        }

        public async Task<string> GetNotificationsAsync(int Id)
        {
            bool hasNotification = HasNotification(Id);

            hasNotification = false;
            if (!hasNotification)
            {
                Action getNotifications = new Action(() =>
                {
                    int sleepTime = 10000;
                    Thread.Sleep(sleepTime);
                });
                await Task.Run(getNotifications);
            }
            string notifications = SearchForNotifications(Id);
            return notifications;
        }

        private bool HasNotification(int id)
        {
            Random random = new Random();
            int isThereAnyNotifs = random.Next(0, 2);

            return isThereAnyNotifs != 0;
        }

        private object GetClient(int id)
        {
            return client;
        }

        private string SearchForNotifications(int id)
        {
            return "foo";
        }
    }
}
