using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services
{
    public static class FileManager
    {
        public static string AppPath()
        {
            return @"/storage/emulated/0/Grip/"; ;
        }

        public static string AppPath(string _file)
        {
            return Path.Combine(AppPath(), _file);
        }

        public static void WriteLog(string _teg, string _value)
        {
            using (StreamWriter sw = new StreamWriter(AppPath("log.txt"), append: true))
            {
                sw.WriteLine($"{DateTime.Now}\t{_teg}\t{_value}");
            }
        }
    }
}
