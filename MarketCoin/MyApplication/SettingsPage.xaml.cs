using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Xaml;
using MarketCoin.API;
using MarketCoin.Resources.Theme;
using Microsoft.Maui.ApplicationModel;
using MarketCoin.MyApplication;
using MarketCoin.ViewModel;
using MarketCoin.Data;

namespace MarketCoin.Application;

public partial class SettingsPage : ContentPage
{
    public ColorImage Color = new ColorImage();
    private static Account User = new Account();
    public SettingsPage()
    {
        InitializeComponent();
        //test
        //PickCurrency();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = User;
        if (App.Database.IsAuthenticated == false)
        {
            LogOut.IsVisible = false;
            RegisterOrLoginPage.IsVisible = true;
            WelcomeUser.IsVisible = false;
            WelcomeUser.Text = "";
            User = new Account();
        }
        else
        {
            LogOut.IsVisible = true;
            RegisterOrLoginPage.IsVisible = false;
            if (App.Database.CurrentAccount != null)
            {
                User = App.Database.CurrentAccount;
                WelcomeUser.Text = "Welcome " + User.EmailAddress;
            }
            WelcomeUser.IsVisible = true;
        }
    }
    //test
    //private void PickCurrency()
    //{
    //    foreach (string valueName in Enum.GetNames(typeof(Currency)))
    //    {
    //        combobox.Items.Add(valueName);
    //    }
    //    async void combobox_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    //    {
    //        CurrencyCoin.CurrensCoin = (Currency)combobox.SelectedIndex;
    //        await Navigation.PushAsync(new Markets(true));
    //    }
    //}

    [Obsolete]
    void SwitchTheme_Toggled(System.Object sender, Microsoft.Maui.Controls.ToggledEventArgs e)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Resources.MergedDictionaries;
        
        if (e.Value == true)
        {
            App.Current.Resources.MergedDictionaries.Add(new DarkTheme());
            App.Current.UserAppTheme = AppTheme.Dark;
            Preferences.Set("SwitchThemeValue", true);
            AboutUsImage.TintColor = (Color)Color.Convert();
            LogOutImage.TintColor = (Color)Color.Convert();
        }
        else
        {
            App.Current.Resources.MergedDictionaries.Add(new WhiteTheme());
            App.Current.UserAppTheme = AppTheme.Light;
            Preferences.Set("SwitchThemeValue", false);
            AboutUsImage.TintColor = (Color)Color.Convert();
            LogOutImage.TintColor = (Color)Color.Convert();
        }
    }
    void ImageButton_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new LoginPage());
    }

    void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new LoginPage());
    }
    void ClickedLogOut(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        App.Database.Logout();
        LogOut.IsVisible = true;
        App.Current.MainPage = new AppShell();
    }
    void BackToMarkets(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new Markets());
    }

    void Image_Loaded(System.Object sender, System.EventArgs e)
    {
        LogOutImage.TintColor = (Color)Color.Convert();
        AboutUsImage.TintColor = (Color)Color.Convert();
    }

    void TapAboutUs(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PushAsync(new AboutUsPage());
    }
}
