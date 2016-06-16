

using PitStopPCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PitStop.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            MessageDialog msg;
            if ((RegUser.Text == "") || (RegPassword.Password == ""))
            {
                if (RegUser.Text == "")
                {
                    RegUser.PlaceholderText = "UserName/Email cannot be empty";
                }
                if (RegPassword.Password == "")
                {
                    RegPassword.PlaceholderText = "Password field Cannot be empty";
                }

            }

            else
            {
                var hash = Helpers.Encryption.EncryptPassword(RegPassword.Password);
                Status.Visibility = Visibility.Visible;
                var result = await App.MobileService.GetTable<Users>().Where(x => x.username == RegUser.Text).ToListAsync();
                Status.Visibility = Visibility.Collapsed;
                if (result.Count == 0)
                {
                    msg = new MessageDialog("No User found, Make sure you are registered");
                    await msg.ShowAsync();
                }
                else if(!result.FirstOrDefault().password.Equals(hash))
                {
                    msg = new MessageDialog("Invalid Password");
                    await msg.ShowAsync();
                }
                else if(result.FirstOrDefault().password.Equals(hash))
                {
                    ApplicationDataCompositeValue composite = new Windows.Storage.ApplicationDataCompositeValue();
                    composite["username"] = result.FirstOrDefault().username;
                    composite["status"] = true;

                    App.localSettings.Values["LoggedinStatus"] = composite;
                    Frame.Navigate(typeof(Profile));
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }
    }
}
