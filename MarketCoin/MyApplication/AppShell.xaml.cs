namespace MarketCoin;
using System.Collections.Generic;
using MarketCoin;
using Microsoft.Maui.Controls;
using MarketCoin.API;
using MarketCoin.Application;
using MarketCoin.MyApplication;
using MarketCoin.ViewModel;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        if (App.Database.CurrentAccount != null)
        {
            ShellContentPortfolio.ContentTemplate = new DataTemplate(() => new Portfolio());
        }
        else
        {
            ShellContentPortfolio.ContentTemplate = new DataTemplate(() => new RegisterOrLoginPage());
        }
        Routing.RegisterRoute(nameof(RegisterOrLoginPage), typeof(RegisterOrLoginPage));
        Routing.RegisterRoute(nameof(Markets), typeof(Markets));
        Routing.RegisterRoute(nameof(Portfolio), typeof(Portfolio));
        Routing.RegisterRoute(nameof(AppShell), typeof(AppShell));

    }
   
    void SettingsTapp(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new SettingsPage());
    }
}
