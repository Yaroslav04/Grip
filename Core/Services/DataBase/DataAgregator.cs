using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.DataBase
{
    public static class DataAgregator
    {
        public static async Task<List<ObjectSoketClass>> Run()
        {
            List<ObjectSoketClass> result = new List<ObjectSoketClass>();

            foreach (var task in await App.DataBase.GetTasksAsync())
            {
                var periods = await App.DataBase.GetPeriodsAsync(task.N);
                if (periods != null)
                {
                    foreach (var period in periods)
                    {
                        //TODO проверяем попадает ли сегодняшний день под правила period

                        if (DateTime.Now.TimeOfDay > period.ControlTime)
                        {
                            //проверяем есть ли такой обьект
                            if (await App.DataBase.IsObjectExistAsync(task.N, period.N, DateTime.Now.DayOfYear))
                            {
                                ObjectClass obj = await App.DataBase.GetObjectAsync(task.N, period.N, DateTime.Now.DayOfYear);
                                //проверяем статус обджекта
                                if (obj.Status == 0)
                                {
                                    // проверяем не вылезло ли время сейчас за стоп тайм                                 
                                    if (DateTime.Now.TimeOfDay > period.StopTime)
                                    {
                                        obj.Status = 2;
                                        await App.DataBase.UpdateObjectAsync(obj);
                                    }
                                    else
                                    {
                                        //проверяем больше ли времени на счетчике со временем сейчас
                                        if (DateTime.Now.TimeOfDay > obj.NotificationTime)
                                        {
                                            var t = DateTime.Now.TimeOfDay + TimeSpan.FromMinutes(30);
                                            if (t > period.StopTime)
                                            {
                                                obj.Status = 2;
                                                await App.DataBase.UpdateObjectAsync(obj);
                                            }
                                            else
                                            {
                                                obj.NotificationTime = t;
                                                await App.DataBase.UpdateObjectAsync(obj);
                                                result.Add(new ObjectSoketClass(obj, task, period));
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
                                objectClass.NotificationTime = period.ControlTime;
                                objectClass.Status = 0;
                                objectClass.Day = DateTime.Now.DayOfYear;
                                await App.DataBase.SaveObjectAsync(objectClass);

                                ObjectClass obj = await App.DataBase.GetObjectAsync(task.N, period.N, DateTime.Now.DayOfYear);

                                if (DateTime.Now.TimeOfDay > period.StopTime)
                                {
                                    obj.Status = 2;
                                    await App.DataBase.UpdateObjectAsync(obj);
                                }
                                else
                                {
                                    //проверяем больше ли времени на счетчике со временем сейчас
                                    if (DateTime.Now.TimeOfDay > obj.NotificationTime)
                                    {
                                        var t = DateTime.Now.TimeOfDay + TimeSpan.FromMinutes(30);
                                        if (t > period.StopTime)
                                        {
                                            obj.Status = 2;
                                            await App.DataBase.UpdateObjectAsync(obj);
                                        }
                                        else
                                        {
                                            obj.NotificationTime = t;
                                            await App.DataBase.UpdateObjectAsync(obj);
                                            result.Add(new ObjectSoketClass(obj, task, period));
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
