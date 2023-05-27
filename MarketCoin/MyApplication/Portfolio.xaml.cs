using MarketCoin.MyApplication;
using MarketCoin.API;
using Syncfusion.Maui.Charts;
using MarketCoin.Drawables;
using MarketCoin.Data;
using Microsoft.Maui.Controls;

namespace MarketCoin;
public partial class Portfolio : ContentPage
{
    private static List<Coins> ListCoinsTransaction = new List<Coins>();
    public Portfolio()
    {
        InitializeComponent();
        App.Database.CurrentAccount.GetPriceNow();
        DrawChart();
    }
    public decimal GetPriceNow()
    {
        var TempList = ListCryptoCoins.GetAllCoins(Currency.USD);
        var templist2 = App.Database.GetCoinsList();
        if(templist2 != null)
        {
            decimal totalprice = 0;
            foreach (var NewItem in TempList)
            {
                foreach (var OldItem in templist2)
                {
                    if (NewItem.name == OldItem.name)
                    {
                        totalprice += (NewItem.price * (decimal)OldItem.amountCoins);
                    }
                }
            }
            return totalprice;
        }
        else
        {
            return 0;
        }
       
    }
    public Portfolio(Coins newCoin)
    {
        InitializeComponent();
        App.Database.AddCoins(newCoin);
        App.Database.CurrentAccount.GetPriceNow();
        LvCoins.ItemsSource = ListCoin();
        DrawChart();   
    }
    private void DrawChart()
    {
        if (ListCoin() != null)
        {
            DoughnutSeries series = new DoughnutSeries();
            series.ItemsSource = ListCoin();
            series.XBindingPath = "name";
            series.YBindingPath = "PriceWithAmount";
            ChartLegend legend = new ChartLegend();
            CryptoChart.Legend = legend;
            CryptoChart.Series.Clear();
            CryptoChart.Series.Add(series);
        }
    }
    private List<Coins> ListCoin()
    {
        var TempList = App.Database.GetCoinsList();
        return ListCoinsTransaction = TempList.GroupBy(c => c.name)
                           .Select(g => new Coins
                           {
                               name = g.Key,
                               symbol = g.FirstOrDefault().symbol,
                               price = g.FirstOrDefault()?.price ?? 0,
                               Icon = g.FirstOrDefault().Icon,
                               price_change_percentage_24h = g.FirstOrDefault().price_change_percentage_24h,
                               amountCoins = g.Sum(c => c.amountCoins),
                           }).ToList();
    }
    void AddNewTransaction(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new AddTransaction());
    }
    [Obsolete]
    void LvCoins_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as Coins;
        if (selectedItem != null)
        {
            List<Coins> TrCoin = App.Database.GetCoinsList().Where(c => c.symbol == selectedItem.symbol).ToList();
            Navigation.PushAsync(new TransactionDetailCoin(selectedItem, TrCoin));
        }
    }
    [Obsolete]
    async void RemoveCoin_Invoked(System.Object sender, System.EventArgs e)
    {
        bool result = await Device.InvokeOnMainThreadAsync<bool>(async () =>
        {
            return await DisplayAlert($"Remove", "Are you sure you want to remove" +
            " this coin? Any transactions associated with " +
            "this coin wiil also be removed", "Remove", "Cancel");
        });

        if (result)
        {
            var coinToRemove = (Coins)((BindableObject)sender).BindingContext;
            bool CoinIsDeleted = App.Database.RemoveCoins(coinToRemove);
            if(CoinIsDeleted)
            {
                App.Database.CurrentAccount.GetPriceNow();
                await Navigation.PushAsync(new Portfolio(), false);
            }
            else
            {
                await DisplayAlert($"Error", "There was an error removing the coin", "Try Again");
            }
        }
    }
    void CreateAccount(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new RegisterPage());
    }
    void LogIn(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new LoginPage());
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshData();
    }
    private void RefreshData()
    {
        if (App.Database.IsAuthenticated)
        {
            BindingContext = App.Database.CurrentAccount;
            LvCoins.ItemsSource = ListCoin();
            if (App.Database.GetCoinsList().Count == 0)
            {
                AddTransactionPage.IsVisible = true;
                StackPortfolio.IsVisible = false;
            }
            else
            {

                AddTransactionPage.IsVisible = false;
                StackPortfolio.IsVisible = true;
            }
        }
        else
        {
            AddTransactionPage.IsVisible = false;
            StackPortfolio.IsVisible = false;
        }
    }
}