using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Notification
{
    public interface IForegroundService
    {
        void StartMyForegroundService();
        void StopMyForegroundService();
        bool IsForeGroundServiceRunning();
        void SendNotification(int _id, string _title, string _message);
    }
}
