
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
                dataBase = new DataBase(FileManager.AppPath(), new List<string> { "TaskDataBase.db3", "PeriodDataBase.db3", "ObjectDataBase.db3" });
            }
            return dataBase;
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
                /*3*/"день тижня",
                /*4*/"день місяця",
                /*5*/"день квартала",
                /*6*/"день в півріччі",
                /*7*/"день у році"
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
