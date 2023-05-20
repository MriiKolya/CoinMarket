namespace MarketCoin;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls;
using System.Threading;



public partial class CoinDetailPage : ContentPage
{
    private Timer timer;
    Animation animation = new Animation();

    [Obsolete]
    public CoinDetailPage(Coins coins)
    {
        InitializeComponent();
        DetailPage.Title = $"{coins.symbol} #{coins.indexcoin}";
        NCoin.Text = coins.name;
        PCoin.Text = $"{coins.price} {coins.Format}";
        timer = new Timer(state => UpdatePriceCoin(coins), null, 1000, 60000);
    }

    [Obsolete]
    public void UpdatePriceCoin(Coins coins)
    {
        
        decimal updatedPrice = CSharpExample.PriceUpdate(coins.symbol, coins.Format);
        
        if (coins.price < updatedPrice)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                coins.price = updatedPrice;
                PCoin.Text = $"{coins.price} {coins.Format}";
                var animation = new Animation(v => PCoin.TextColor = Color.FromRgb((byte)(0 + v * (0 - 0)), (byte)(255 + v * (0 - 255)), (byte)(0 + v * (0 - 0))), 0, 1);
                animation.Commit(this, "ColorAnimation", length: 5000, repeat: () => false);

            });
        }
        else if (coins.price == updatedPrice)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                coins.price = updatedPrice;
                PCoin.Text = $"{coins.price} {coins.Format}";
            });
        }
        else if (coins.price > updatedPrice)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                coins.price = updatedPrice;
                PCoin.Text = $"{coins.price} {coins.Format}";
                var animation = new Animation(v => PCoin.TextColor = Color.FromRgb((byte)(255 - v * (255 - 0)), (byte)(0 + v * (0 - 0)), (byte)(0 + v * (0 - 0))), 0, 1);
                animation.Commit(this, "ColorAnimation", length: 5000, repeat: () => false);

            });
        }

    }

}
