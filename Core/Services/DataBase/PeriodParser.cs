using Java.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.DataBase
{
    public static class PeriodParser
    {
        public static bool IsPeriod(PeriodClass _period)
        {
            var period = _period.Period;

            if (!IsBetweenStartEndDay(_period))
            {
                return false;
            }       

            /*0*/
            if (period == 0)
            {
                if (_period.StartDate.DayOfYear == DateTime.Now.DayOfYear)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /*1*/
            if (period == 1)
            {
                return true;
            }

            /*2*/
            if (period == 2)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    return false;
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            /*3*/
            if (period == 3)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /*4*/
            if (period == 4)
            {
                if (DateTime.Now.DayOfWeek == _period.StartDate.DayOfWeek)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /*5*/
            if (period == 5)
            {
                if (DateTime.Now.Day == _period.StartDate.Day)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /*6*/
            if (period == 6)
            {
                if (DateTime.Now.DayOfYear == _period.StartDate.DayOfYear)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        static bool IsBetweenStartEndDay(PeriodClass _period)
        {
            if (DateTime.Now.DayOfYear >= _period.StartDate.DayOfYear & DateTime.Now.DayOfYear <= _period.EndDate.DayOfYear)
            {
                return true;
            }
            return false;
        }

        public static int GetIntFromPeriodName(string _value)
        {
            int index = App.PeriodTypes.IndexOf(_value);
            return index;
        }
    }
}
