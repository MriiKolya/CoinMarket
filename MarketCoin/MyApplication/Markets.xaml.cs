using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
namespace MarketCoin;

public partial class Markets : ContentPage
{

    private List<Coins> Coin;
    bool Cheker = true;

    public Markets()
    {
        InitializeComponent();

        Coin = ListCoins.APICall(currency.USD);
        LvCoins.ItemsSource = Coin;
        Price1.Text += Coin[0].Format;
    }

    void index_Clicked(System.Object sender, System.EventArgs e)
    {
        Cheker = !Cheker;
        if (Cheker == false) index.Text = "#🔻";
        else index.Text = "#🔺";
        LvCoins.ItemsSource = null;
        Coin.Reverse();
        LvCoins.ItemsSource = Coin;
    }

    void Price1_Clicked(System.Object sender, System.EventArgs e)
    {
        Cheker = !Cheker;
        if (Cheker == false)
        {
            Price1.Text += "Price 🔻";
            LvCoins.ItemsSource = null;
            Coin.Sort(delegate (Coins x, Coins y)
            {
                return x.price.CompareTo(y.price);
            });
            LvCoins.ItemsSource = Coin;

        }
        else
        {
            Price1.Text += "Price 🔺";
            LvCoins.ItemsSource = null;
            Coin.Sort(delegate (Coins x, Coins y)
            {
                return y.price.CompareTo(x.price);
            });
            LvCoins.ItemsSource = Coin;
        }
    }

    void LvCoins_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as Coins;
        Navigation.PushAsync(new CoinDetailPage(selectedItem));
    }
}
