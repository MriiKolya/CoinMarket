using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls;
using System.Threading;
using MarketCoin.API;
using SkiaSharp;
using System.Drawing;
using SkiaSharp.Views.Maui.Controls;
using Color = Microsoft.Maui.Graphics.Color;
using SkiaSharp.Views.Maui;
using Syncfusion.Maui.Charts;
using System.Globalization;

namespace MarketCoin.MyApplication;

public partial class CoinDetailPage : ContentPage
{
    private Timer timer;
    Animation animation = new Animation();
    protected ValueToColorConverter ValueToColorConverter = new ValueToColorConverter();

    [Obsolete]
    public CoinDetailPage(Coins coins)
    {
        InitializeComponent();
        BindingContext = coins;
        GetLineCharts(coins);
        SetPropertyOnPage(coins);
        timer = new Timer(state => UpdatePriceCoin(coins), null, 10000, 30000);
    }

    [Obsolete]
    private void SetPropertyOnPage(Coins coins)
    {
        Price_Change.TextColor = (Color)ValueToColorConverter.Convert(coins.price_change_24h, typeof(Color), null, CultureInfo.CurrentCulture);
        FormatMarketCapChange.Text = (coins.market_cap_change_24h / 1000000000.0).ToString("0.##") + " Bn";
        FormatMarketCapChange.TextColor = (Color)ValueToColorConverter.Convert(coins.market_cap_change_24h, typeof(Color), null, CultureInfo.CurrentCulture);
        Procent.Text = Math.Round(coins.price_change_percentage_24h, 2).ToString() + " %";
        DetailPage.Title = $"{coins.symbol} #{coins.indexcoin}";
        NCoin.Text = coins.name;
        PCoin.Text = coins.price + " " + coins.formatCoin;
    }
    private LineSeries GetLineSeries(Color color)
    {
        return new LineSeries()
        {
            XBindingPath = "data",
            YBindingPath = "price",
            EnableAnimation = true,
            Fill = new SolidColorBrush(color),
            EnableTooltip = true,
            StrokeWidth = 2,
        };
    }

    [Obsolete]
    private void GetLineCharts(Coins coins)
    {
        ChartLine.BindingContext = new DataChart(coins);
        ChartLegend legend = new ChartLegend();
        CategoryAxis primaryAxis = new CategoryAxis();
        NumericalAxis secondaryAxis = new NumericalAxis();
        primaryAxis.MajorGridLineStyle = null;
        ChartLine.XAxes.Add(primaryAxis);
        var binding = new Binding() { Path = "Data" };
        if (coins.sparkline_in_7d[0] >= coins.sparkline_in_7d[^1])
        {
            Color color = Color.FromHex("ef476f");
            var lineSeries = GetLineSeries(color); 
            lineSeries.SetBinding(ChartSeries.ItemsSourceProperty, binding);
            ChartLine.Series.Add(lineSeries);
            ChartLine.YAxes.Add(secondaryAxis);
        }
        else
        {
            Color color = Color.FromHex("06d6a0");
            var lineSeries = GetLineSeries(color);
            lineSeries.SetBinding(ChartSeries.ItemsSourceProperty, binding);
            ChartLine.Series.Add(lineSeries);
            ChartLine.YAxes.Add(secondaryAxis);
        }
    }
    [Obsolete]
    public void UpdatePriceCoin(Coins coins)
    {
        decimal updatedPrice = ListCryptoCoins.GetNewPriceCoin(CurrencyCoin.CurrensCoin,coins.name);
        SetPropertyOnPage(coins);
        if (coins.price < updatedPrice)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                coins.price = updatedPrice;
                PCoin.Text = $"{coins.price} {coins.formatCoin}";
                var animation = new Animation(v => PCoin.TextColor = Color.FromRgb((byte)(0 + v * (0 - 0)), (byte)(255 + v * (0 - 255)), (byte)(0 + v * (0 - 0))), 0, 1);
                animation.Commit(this, "ColorAnimation", length: 5000, repeat: () => false);
            });
        }
        else if (coins.price == updatedPrice)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                coins.price = updatedPrice;
                PCoin.Text = $"{coins.price} {coins.formatCoin}";
            });
        }
        else if (coins.price > updatedPrice)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                coins.price = updatedPrice;
                PCoin.Text = $"{coins.price} {coins.formatCoin}";
                var animation = new Animation(v => PCoin.TextColor = Color.FromRgb((byte)(255 - v * (255 - 0)), (byte)(0 + v * (0 - 0)), (byte)(0 + v * (0 - 0))), 0, 1);
                animation.Commit(this, "ColorAnimation", length: 5000, repeat: () => false);
            });
        }

    }
}
