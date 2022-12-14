
namespace Grip;

public partial class App : Application
{
    static DataBase dataBase;
    public static DataBase DataBase
    {
        get
        {
            if (dataBase == null)
            {
                dataBase = new DataBase(FileManager.AppPath(), new List<string> {
                    "TaskDataBase.db3", "PeriodDataBase.db3", "ObjectDataBase.db3", "SensorDataBase.db3" });
            }
            return dataBase;
        }
    }

    static BTClass btClass;

    public static BTClass BTClass
    {
        get
        {
            if (btClass == null)
            {
                btClass = new BTClass();
            }
            return btClass;
        }
    }

    public static List<string> SensorTypes
    {
        get
        {
            return new List<string>
            {
                "temperature",
                "humidity",
                "co",
                "pm1",
                "pm2.5",
                "pm10",
            };
        }
    }
    public static List<string> TaskTypes
    {
        get
        {
            return new List<string>
            {
                /*1*/"Здоровье",
                /*2*/"Медикаменты",
                /*3*/"Еда",
                /*4*/"Спорт",
                /*5*/"Работа",
                /*6*/"Финансы",
                /*7*/"Ментальность",
            };
        }
    }

    public static bool IsServiseRunning = false;

    public static List<string> PeriodTypes
    {
        get
        {
            return new List<string>
            {
                /*0*/"одноразово",
                /*1*/"кожен день",
                /*2*/"кожен будній день",
                /*3*/"вихідні дні",
                /*4*/"день тижня",
                /*5*/"день місяця",
                /*6*/"день у році"
            };
        }
    }

    public static List<string> ObjectStatuses
    {
        get
        {
            return new List<string>
            {
                /*0*/"на виконанні",
                /*1*/"виконано",
                /*2*/"пропущено",
            };
        }
    }

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
