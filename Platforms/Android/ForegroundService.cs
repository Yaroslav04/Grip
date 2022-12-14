using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using Grip.Platforms.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Content.ClipData;
using AndroidApp = Android.App.Application;
using Debug = System.Diagnostics.Debug;

[assembly: Dependency(typeof(ForegroundServices))]
namespace Grip.Platforms.Android
{
    [Service]
    public class ForegroundServices : Service, IForegroundService
    {
        public static bool IsForegroundServiceRunning;
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Task.Run(async () => App.BTClass.RunAsync());

            Task.Run(async () =>
            {
                while (IsForegroundServiceRunning)
                {
                    Thread.Sleep(60000);
      
                    //**********Message

                    try
                    {
                        var s = await Task.Run(() => DataAgregator.Run());

                        if (s.Count > 0)
                        {
                            foreach (var item in s)
                            {
                                SendNotification(item.N, item.TaskSoket.Type, item.TaskSoket.Name);
                            }                     
                        }
                    }
                    catch (Exception ex)
                    {
                        FileManager.WriteLog("foreground exception", ex.Message);
                    }

                    //**********Message              
                }
            });

            string channelID = "ForeGroundServiceChannel";
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);

            var notfificationChannel = new NotificationChannel(channelID, channelID, NotificationImportance.Low);
            notificationManager.CreateNotificationChannel(notfificationChannel);


            var notificationBuilder = new NotificationCompat.Builder(this, channelID)
                                         .SetContentTitle("Grip ServiceStarted")
                                         .SetSmallIcon(Resource.Mipmap.appicon_background)
                                         .SetContentText("Grip Running in Foreground")
                                         .SetPriority(1)
                                         .SetOngoing(true)
                                         .SetChannelId(channelID)
                                         .SetAutoCancel(true);


            StartForeground(1001, notificationBuilder.Build());
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnCreate()
        {
            base.OnCreate();
            IsForegroundServiceRunning = true;
            FileManager.WriteLog("foreground servise", "on create");
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            IsForegroundServiceRunning = false;
            FileManager.WriteLog("foreground servise", "on destroy");
        }

        public void StartMyForegroundService()
        {
            var intent = new Intent(AndroidApp.Context, typeof(ForegroundServices));
            AndroidApp.Context.StartForegroundService(intent);
        }

        public void StopMyForegroundService()
        {
            var intent = new Intent(AndroidApp.Context, typeof(ForegroundServices));
            AndroidApp.Context.StopService(intent);
        }

        public bool IsForeGroundServiceRunning()
        {
            return IsForegroundServiceRunning;
        }

        public void SendNotification(int _id, string _title, string _message)
        {
            string channelID = "NotificationChannel";
            var SnotificationManager = (NotificationManager)GetSystemService(NotificationService);

            var notfificationChannel = new NotificationChannel(channelID, channelID, NotificationImportance.High);
            SnotificationManager.CreateNotificationChannel(notfificationChannel);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, channelID)
            .SetContentTitle(_title)
            .SetContentText(_message)
            .SetSmallIcon(Resource.Drawable.abc_ic_arrow_drop_right_black_24dp);

            // Build the notification:
            Notification Snotification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            //const int notificationId = 0;
            notificationManager.Notify(_id, Snotification);
        }
    }
}
