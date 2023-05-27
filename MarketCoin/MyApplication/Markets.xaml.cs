using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using MarketCoin.API;
using System.Linq;
using MarketCoin.MyApplication;
using System.Net;
using MarketCoin.Application;
using System.Collections;
using Microsoft.Maui.ApplicationModel;
using MarketCoin.Resources.Theme;
using MarketCoin.ViewModel;

namespace MarketCoin;

public partial class Markets : ContentPage
{
    
    public static List<Coins> ListCoin = ListCryptoCoins.GetAllCoins(CurrencyCoin.CurrensCoin);
    private bool Cheker = true;
    private ColorImage colorI = new ColorImage();
     
    [Obsolete]
    public Markets(bool refresh = false)
    {
        InitializeComponent();   
        Dictionary<string,double> MyDictionary = ListCryptoCoins.GlobalStatic();
        BindingContext = MyDictionary;
        //StartCarouselTimer();
        Carouse.ItemsSource = MyDictionary;
        ListCoin = ListCryptoCoins.GetAllCoins(CurrencyCoin.CurrensCoin);
        LvCoins.ItemsSource = ListCoin;
        if (refresh) RefreshCoinList();
        Price1.Text = "Price🔺";
    }

    [Obsolete]
    private async Task StartCarouselTimer()
    {
        const int CountElementMarketCap = 54;
        while (true)
        {
            await Task.Delay(5000); // ожидаем 2 секунды
            Device.BeginInvokeOnMainThread(() =>
            {
                var currentIndex = Carouse.Position;
                var nextIndex = (currentIndex + 1) % CountElementMarketCap;
                Carouse.ScrollTo(nextIndex, animate: true);
            });
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ColorImageMenu.TintColor = (Color)colorI.Convert();
        ColorImageSearch.TintColor = (Color)colorI.Convert();

    }
    private void RefreshCoinList()
    {
        var ListCoin2 = ListCryptoCoins.GetAllCoins(CurrencyCoin.CurrensCoin);
        LvCoins.ItemsSource = ListCoin2;
    }
    private void ReversList(System.Object sender, System.EventArgs e)
    {
        Cheker = !Cheker;
        if (Cheker == true)
        {
            index.Text = "#🔻";
            LvCoins.ItemsSource = null;
            ListCoin.Sort(delegate (Coins x, Coins y)
            {
                return x.indexcoin.CompareTo(y.indexcoin);
            });
            LvCoins.ItemsSource = ListCoin;
        }
        else
        {
            index.Text = "#🔺";
            LvCoins.ItemsSource = null;
            ListCoin.Sort(delegate (Coins x, Coins y)
            {
                return y.indexcoin.CompareTo(x.indexcoin);
            });
            LvCoins.ItemsSource = ListCoin;
        }      
    }
    private void SortPrice(System.Object sender, System.EventArgs e)
    {
        Cheker = !Cheker;
        if (Cheker == true)
        {
            Price1.Text = "Price 🔻";
            LvCoins.ItemsSource = null;
            ListCoin.Sort(delegate (Coins x, Coins y)
            {
                return x.price.CompareTo(y.price);
            });
            LvCoins.ItemsSource = ListCoin;
        }
        else
        {
            Price1.Text = "Price 🔺";
            LvCoins.ItemsSource = null;
            ListCoin.Sort(delegate (Coins x, Coins y) 
            {
                return y.price.CompareTo(x.price);
            });
            LvCoins.ItemsSource = ListCoin;
        }
    }
    [Obsolete]
    void LvCoins_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as Coins;
        Navigation.PushAsync(new CoinDetailPage(selectedItem));
    }
    void procent_Clicked(System.Object sender, System.EventArgs e)
    {
        Cheker = !Cheker;
        if (Cheker == false)
        {
            procent.Text = "24h #🔻";
            LvCoins.ItemsSource = null;
            ListCoin.Sort(delegate (Coins x, Coins y)
            {
                return x.price_change_percentage_24h.CompareTo(y.price_change_percentage_24h);
            });
            LvCoins.ItemsSource = ListCoin;
        }
        else
        {
            procent.Text = "24h #🔺";
            LvCoins.ItemsSource = null;
            ListCoin.Sort(delegate (Coins x, Coins y)
            {
                return y.price_change_percentage_24h.CompareTo(x.price_change_percentage_24h);
            });
            LvCoins.ItemsSource = ListCoin;
        }
    }

    void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new SearchPage(ListCoin));
    }
    void MenuTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new SettingsPage(), false);
    }
}
