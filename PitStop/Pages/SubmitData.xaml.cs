using PitStop.Helpers;
using PitStopPCL;
using PitStopPCL.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PitStop.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubmitData : Page
    {
        public SubmitData()
        {
            this.InitializeComponent();
        }
        ObservableCollection<Resource> suggestions = new ObservableCollection<Resource>();
        private IGeolocator locator;
        private Position position;
        private Resource selectedresource;
        private UserStatus result;
        private Entries param;

        private int GetIndex(string text)
        {
            if (text.Contains("Worksh"))
                return 0;
            else if (text.Contains("Restau"))
                return 1;
            else if (text.Contains("Hosp"))
                return 2;
            else if (text.Contains("Ambula"))
                return 3;
            else if (text.Contains("Police"))
                return 4;
            else if (text.Contains("Hotels"))
                return 5;
            else
                return 0;
  }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
                param = (Entries)e.Parameter;

            if (param != null)
            {
                entitycontactname.Text = param.contact_name;
                entitytype.SelectedIndex = GetIndex(param.type);
                entityname.Text = param.entry_name;
                alternatecontact.Text = param.alternate_number;
                primarycontact.Text = param.contact_number;
                location.Text = await Helpers.Location.ReverseGeoCode(param.latitude, param.longitude);
            }
            result = Operations.checkloggedin();
            if (result.loggedin)
            {

            }
            else
            {
                SubmitDataLayout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                Loggedin.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private async void Submit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (entitytype.SelectedIndex == -1 || String.IsNullOrWhiteSpace(entityname.Text) || String.IsNullOrWhiteSpace(entityname.Text) || String.IsNullOrWhiteSpace(primarycontact.Text))
            {
                MessageDialog msg = new MessageDialog("All Fields are Must");
                await msg.ShowAsync();
            }
            else
            {
                string lat = 0.ToString(), lon = 0.ToString();
                if(selectedresource!=null)
                {
                    lat = selectedresource.point.coordinates[0].ToString();
                    lon = selectedresource.point.coordinates[1].ToString();
                }
                else
                {
                    if (position != null)
                    {
                        lat = position.Latitude.ToString();
                        lon = position.Longitude.ToString();
                    }
                  
                }

                var Submission = new Entries()
                {
                    entry_name = entityname.Text,
                    contact_name = entitycontactname.Text,
                    type = entitytype.SelectedValue.ToString(),
                    latitude = lat,
                    longitude = lon,
                    alternate_number = alternatecontact.Text,
                    contact_number = primarycontact.Text,
                    createdby = result.username
                };

                Status.Visibility = Visibility.Visible;
                if(param==null)
                { 
                await App.MobileService.GetTable<Entries>().InsertAsync(Submission);
                var temp = await App.MobileService.GetTable<Users>().Where(x => x.username == result.username).Take(1).ToListAsync();
                var obj = temp.First();
                obj.points = (int.Parse(obj.points) + 50).ToString();
                await App.MobileService.GetTable<Users>().UpdateAsync(obj);
                Status.Visibility = Visibility.Collapsed;
                var msg = new MessageDialog("Successfully Submitted");
                await msg.ShowAsync();
                }
                else
                {
                    Submission.id = param.id;
                    await App.MobileService.GetTable<Entries>().UpdateAsync(Submission);
                    Status.Visibility = Visibility.Collapsed;
                    var msg = new MessageDialog("Successfully Updated");
                    await msg.ShowAsync();

                }
            }
        }

        private async void location_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)

            {
                var result = await Location.SuggestLocation(sender.Text);
                if(result!= null)
                { 
                suggestions.Clear();
                for(int i=0;i<result.Count;i++)
                {
                    suggestions.Add(result[i]);
                }
                sender.ItemsSource = suggestions;
                }
            }
        }

        private void location_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            { 
                location.Text = ((Resource)args.ChosenSuggestion).name;
               
            }
            else

                location.Text = sender.Text;
        }

        private void location_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            location.Text = ((Resource)args.SelectedItem).name;
            selectedresource = ((Resource)args.SelectedItem);
        }

        private async void LocationFind_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try { 
            locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            position = await locator.GetPositionAsync(timeoutMilliseconds: 100000);
            var result =  await Helpers.Location.ReverseGeoCode(position.Latitude.ToString(), position.Longitude.ToString());
            if(result!=null)
            {
                location.Text = result;
            }
            }
            catch
            {

            }
        }
    }
}
