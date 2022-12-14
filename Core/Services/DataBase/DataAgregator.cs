using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grip.Core.Services.Parser;

namespace Grip.Core.Services.DataBase
{
    public static class DataAgregator
    {
        public static async Task<List<ObjectSoketClass>> Run()
        {
            List<ObjectSoketClass> result = new List<ObjectSoketClass>();

            try
            {
                foreach (var obj in await App.DataBase.ObjectDB.GetObjectsAsync())
                {
                    var per = await App.DataBase.PeriodDB.GetPeriodAsync(obj.TaskId);
                    if (per.IsAutoDayEnd)
                    {
                        if (obj.Day < DateTime.Now.DayOfYear)
                        {
                            if (obj.Status == 0)
                            {
                                obj.Status = 2;
                                await App.DataBase.ObjectDB.UpdateAsync(obj);
                            }
                        }
                    }
                    else
                    {
                        if (obj.Status == 0)
                        {
                            obj.Day = DateTime.Now.DayOfYear;
                            await App.DataBase.ObjectDB.UpdateAsync(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileManager.WriteLog("agregator:last day update", ex.Message);
            }

            foreach (var task in await App.DataBase.TaskDB.GetTasksAsync())
            {
                var periods = await App.DataBase.PeriodDB.GetPeriodsAsync(task.N);
                if (periods != null)
                {
                    foreach (var period in periods)
                    {
                        if (period.Period != 0)
                        {
                            if (PeriodParser.IsPeriod(period))
                            {
                                if (DateTime.Now.TimeOfDay > period.StartTime)
                                {
                                    if (await App.DataBase.ObjectDB.IsObjectExistAsync(task.N, period.N, DateTime.Now.DayOfYear))
                                    {
                                        ObjectClass obj = await App.DataBase.ObjectDB.GetObjectAsync(task.N, period.N, DateTime.Now.DayOfYear);
                                        if (obj.Status == 0)
                                        {
                                            if (DateTime.Now.TimeOfDay > period.StopTime)
                                            {
                                                try
                                                {
                                                    obj.Status = 2;
                                                    await App.DataBase.ObjectDB.UpdateAsync(obj);
                                                }
                                                catch (Exception ex)
                                                {
                                                    FileManager.WriteLog("agregator:execute time lost", ex.Message);
                                                }

                                            }
                                            else
                                            {
                                                if (DateTime.Now.TimeOfDay > obj.NotificationTime)
                                                {
                                                    var t = DateTime.Now.TimeOfDay + TimeSpan.FromMinutes(period.Pause);
                                                    if (t > period.StopTime)
                                                    {
                                                        try
                                                        {
                                                            obj.Status = 2;
                                                            await App.DataBase.ObjectDB.UpdateAsync(obj);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            FileManager.WriteLog("agregator:execute time lost", ex.Message);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            obj.NotificationTime = t;
                                                            await App.DataBase.ObjectDB.UpdateAsync(obj);
                                                            result.Add(new ObjectSoketClass(obj, task, period));
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            FileManager.WriteLog("agregator:update notification time", ex.Message);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ObjectClass objectClass = new ObjectClass();
                                        objectClass.TaskId = task.N;
                                        objectClass.PeriodId = period.N;
                                        objectClass.NotificationTime = period.StartTime;
                                        objectClass.Status = 0;
                                        objectClass.Day = DateTime.Now.DayOfYear;
                                        objectClass.SaveDate = DateTime.Now;
                                        try
                                        {

                                        }
                                        catch (Exception s)
                                        {
                                            Debug.WriteLine(s.Message);
                                        }
                                        await App.DataBase.ObjectDB.SaveAsync(objectClass);

                                        ObjectClass obj = await App.DataBase.ObjectDB.GetObjectAsync(task.N, period.N, DateTime.Now.DayOfYear);

                                        if (DateTime.Now.TimeOfDay > period.StopTime)
                                        {
                                            try
                                            {
                                                obj.Status = 2;
                                                await App.DataBase.ObjectDB.UpdateAsync(obj);
                                            }
                                            catch (Exception ex)
                                            {
                                                FileManager.WriteLog("agregator:execute time lost when create object", ex.Message);
                                            }
                                        }
                                        else
                                        {
                                            //проверяем больше ли времени на счетчике со временем сейчас
                                            if (DateTime.Now.TimeOfDay > obj.NotificationTime)
                                            {
                                                var t = DateTime.Now.TimeOfDay + TimeSpan.FromMinutes(period.Pause);
                                                if (t > period.StopTime)
                                                {
                                                    try
                                                    {
                                                        obj.Status = 2;
                                                        await App.DataBase.ObjectDB.UpdateAsync(obj);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        FileManager.WriteLog("agregator:execute time lost when create object", ex.Message);
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        obj.NotificationTime = t;
                                                        await App.DataBase.ObjectDB.UpdateAsync(obj);
                                                        result.Add(new ObjectSoketClass(obj, task, period));
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        FileManager.WriteLog("agregator:create object", ex.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
