using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;



namespace MarketCoin;

public partial class NewTabbedPage : TabbedPage
{
    private List<Coins> Coin;
    bool Cheker = true;

    public NewTabbedPage()
    {
        InitializeComponent();

        Coin = ListCoins.APICall(currency.USD);
        LvCoins.ItemsSource = Coin;
        var CoinFormat = Coin[0].Format;
       
        //foreach (string valueName in Enum.GetNames(typeof(currency)))
        //{
        //    combobox.Items.Add(valueName);
        //}
    }

    [Obsolete]
    void LvCoins_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as Coins;
        Navigation.PushAsync(new CoinDetailPage(selectedItem));
        
    }

    void ReversIndex(System.Object sender, System.EventArgs e)
    {

        Cheker = !Cheker;
        if (Cheker == false) index.Text = "#🔻";
        else index.Text = "#🔺";
        LvCoins.ItemsSource = null;
        Coin.Reverse();
        LvCoins.ItemsSource = Coin;
    }

    void SortPrice(System.Object sender, System.EventArgs e)
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

    //void combobox_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    //{
    //    Cheker=!Cheker;
    //    LvCoins.ItemsSource = null;
    //    currency currency = (currency)combobox.SelectedIndex;
    //    Coins.count = 0;
    //    Coin = ListCoins.APICall(currency);
    //    LvCoins.ItemsSource = Coin;
    //}
  
}
