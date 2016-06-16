using Intense.Presentation;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using PitStop.Pages;
using PitStop.Presentation;

namespace PitStop
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            this.InitializeComponent();

            var vm = new ShellViewModel();
            vm.TopItems.Add(new NavigationItem { Icon = "\uE80F",  DisplayName = "Home", PageType = typeof(WelcomePage) });
            vm.TopItems.Add(new NavigationItem { Icon = "\uE11A", DisplayName = "Search", PageType = typeof(Search) });
            vm.TopItems.Add(new NavigationItem { Icon = "\uE898", DisplayName = "Submit Data", PageType = typeof(SubmitData) });
            vm.TopItems.Add(new NavigationItem { Icon = "\uE77B", DisplayName = "Profile", PageType = typeof(Profile) });
          
            vm.BottomItems.Add(new NavigationItem { Icon = "\uE713", DisplayName = "Settings", PageType = typeof(SettingsPage) });

            // select the first top item
            vm.SelectedItem = vm.TopItems.First();

            this.ViewModel = vm;
        }

        public ShellViewModel ViewModel { get; private set; }

        public Frame RootFrame
        {
            get
            {
                return this.Frame;
            }
        }
    }
}
