using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PitStop.Helpers;
using PitStopPCL;
using System.Linq;
using System.Collections.Generic;

namespace PitStop.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profile : Page
    {
        private List<Entries> items;

        public Profile()
        {
            this.InitializeComponent();
         
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var result = Operations.checkloggedin();
            if (result.loggedin)
            {
                Status.Visibility = Windows.UI.Xaml.Visibility.Visible;
                var user = await App.MobileService.GetTable<Users>().Where(x => x.username == result.username).ToListAsync();
                var reguser = user.FirstOrDefault();    
                UserName.Text = reguser.name;
                Points.Text = reguser.points + " Points";
                items = await App.MobileService.GetTable<Entries>().Where(x => x.createdby == reguser.username).ToListAsync();
                MyEntries.ItemsSource = items;
                Status.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }
            else
            {
                ProfileLayout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                NotLogged.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.localSettings.Values["LoggedinStatus"] = null;
            Frame.Navigate(typeof(WelcomePage));
        }

        private void edit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var current = ((Entries)((Button)e.OriginalSource).DataContext);
            Frame.Navigate(typeof(SubmitData), current);

        }

        private async void delete_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var current = ((Entries)((Button)e.OriginalSource).DataContext);
            
            MyEntries.ItemsSource = null;
            items.Remove(current);
            MyEntries.ItemsSource = items;
          
            await App.MobileService.GetTable<Entries>().DeleteAsync(current);

        }
    }
}
