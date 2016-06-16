using PitStopPCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PitStop.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
      

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageDialog msg;
            if ((RegName.Text == "") || (RegEmail.Text == "") || (RegPassword.Password == "") || (RegPasswordC.Password == "") || (RegMobile.Text == ""))
            {
                if (RegName.Text == "")
                {
                    RegName.PlaceholderText = "Name cannot be empty";
                }
                if(RegEmail.Text=="")
                {
                    RegEmail.PlaceholderText = "Email Cannot be empty";
                }
                if(RegPassword.Password=="")
                {
                    RegPassword.PlaceholderText = "Password field Cannot be empty";
                }
                if (RegPasswordC.Password == "")
                {
                    RegPasswordC.PlaceholderText = "Password field Cannot be empty";
                }
                if (RegMobile.Text== "")
                {
                    RegMobile.PlaceholderText = "Mobile Number Cannot be empty";
                }
            }
            else if (RegPasswordC.Password != RegPassword.Password)
            {
                msg = new MessageDialog("Password doesn't match, Please confirm");
                await msg.ShowAsync();
            }
            else
            {
                var User = new Users
                {
                    username = RegEmail.Text,
                    password = Helpers.Encryption.EncryptPassword(RegPassword.Password),
                    mobile = RegMobile.Text,
                    points = 0.ToString(),
                    name = RegName.Text
                };
                Status.Visibility = Visibility.Visible;
                var result = await App.MobileService.GetTable<Users>().Where(x => x.username == RegEmail.Text).ToListAsync();
                Status.Visibility = Visibility.Collapsed;
                if (result.Count==0)
                { 
                await App.MobileService.GetTable<Users>().InsertAsync(User);
                    msg = new MessageDialog("Successfully Registered");
                    await msg.ShowAsync();
                    Frame.Navigate(typeof(LoginPage));
                }
                else
                {
                    msg = new MessageDialog("Already Registered");
                    await msg.ShowAsync();
                }
            }
        }
    }
}
