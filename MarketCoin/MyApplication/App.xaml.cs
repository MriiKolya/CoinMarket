using MarketCoin.Data;
using MarketCoin.MyApplication;
using MarketCoin.ViewModel;

namespace MarketCoin;
public partial class App : IApplication
{   
    private static DatabaseContext database;
    public static DatabaseContext Database
    {
        get
        {
            if(database == null)
            {
                database = new DatabaseContext();
            }
            return database;
        }
    }
    public App()
    {   
        InitializeComponent();
        MainPage = new AppShell();

    }
}

