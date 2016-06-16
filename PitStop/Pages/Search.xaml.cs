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
using Windows.System;
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
    public sealed partial class Search : Page
    {
        private IGeolocator locator;
        private Position position;
        private Resource selectedresource;
        bool order;
        ObservableCollection<Resource> suggestions = new ObservableCollection<Resource>();

        public Search()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Type.Visibility == Visibility.Visible)
                Type.Visibility = Visibility.Collapsed;
            else
                Type.Visibility = Visibility.Visible;
        }



        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                position = await locator.GetPositionAsync(timeoutMilliseconds: 100000);
                var result = await Helpers.Location.ReverseGeoCode(position.Latitude.ToString(), position.Longitude.ToString());
                if (result != null)
                {
                    location.Text = result;
                    order = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void location_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)

            {
                var result = await Location.SuggestLocation(sender.Text);
                if (result != null)
                {
                    suggestions.Clear();
                    for (int i = 0; i < result.Count; i++)
                    {
                        suggestions.Add(result[i]);
                    }
                    sender.ItemsSource = suggestions;
                }
            }
        }

        private async void location_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                location.Text = ((Resource)args.ChosenSuggestion).name;
                if (order)
                {
                    SearchListView.ItemsSource = await Helpers.Operations.GetEntries(position.Latitude.ToString(), position.Longitude.ToString(), 4000);
                }
                else if (!order)
                {
                    SearchListView.ItemsSource = await Helpers.Operations.GetEntries(selectedresource.point.coordinates[0].ToString(), selectedresource.point.coordinates[1].ToString(), 4000);

                }
            }
            else
            {
                if (position != null)
                {
                    if (order)
                    {
                        SearchListView.ItemsSource = await Helpers.Operations.GetEntries(position.Latitude.ToString(), position.Longitude.ToString(), 3000);
                    }
                    else if (!order)
                    {
                        SearchListView.ItemsSource = await Helpers.Operations.GetEntries(selectedresource.point.coordinates[0].ToString(), selectedresource.point.coordinates[1].ToString(), 3000);

                    }
                }

            }


        }

        private async void location_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            location.Text = ((Resource)args.SelectedItem).name;
            selectedresource = ((Resource)args.SelectedItem);
            order = false;
        }

        private async void loc_Click(object sender, RoutedEventArgs e)
        {
            var current = ((Entries)((Button)e.OriginalSource).DataContext);

            var uri = new Uri(@"ms-drive-to:?destination.latitude=" + current.latitude + "&destination.longitude=" + current.longitude + "&destination.name=" + current.entry_name);

            // Set the option to show a warning
            var promptOptions = new Windows.System.LauncherOptions();
            promptOptions.TreatAsUntrusted = true;

            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(uri, promptOptions);

            if (success)
            {
                // URI launched
            }
            else
            {
                MessageDialog msg = new MessageDialog("Unable to Show Directions");
                await msg.ShowAsync();
            }

        }

        private async void call_Click(object sender, RoutedEventArgs e)
        {
            var current = ((Entries)((Button)e.OriginalSource).DataContext);

            if (App.IsMobile)
            {

                Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(current.contact_number, current.contact_name);
            }
            else
            {
                var uriSkype = new Uri(@"Skype:" + current.contact_number + ")?call");

                // Set the option to show a warning
                var promptOptions = new Windows.System.LauncherOptions();
                promptOptions.TreatAsUntrusted = true;

                // Launch the URI
                var success = await Windows.System.Launcher.LaunchUriAsync(uriSkype, promptOptions);

                if (success)
                {
                    // URI launched
                }
                else
                {
                    MessageDialog msg = new MessageDialog("Unable to Make Call");
                    await msg.ShowAsync();
                }
            }

        }
    }
}
