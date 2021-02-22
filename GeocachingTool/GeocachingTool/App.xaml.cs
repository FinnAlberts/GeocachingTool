using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    public partial class App : Application
    {
        public static string DatabaseLocation = string.Empty;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        public App(string dbLocation)
        {
            InitializeComponent();

            MainPage = new AppShell();

            DatabaseLocation = dbLocation;
        }

        protected override void OnStart()
        {
            // Check for dark theme
            OSAppTheme currentTheme = Application.Current.RequestedTheme;

            if (currentTheme == OSAppTheme.Dark)
            {
                App.Current.Resources["PrimaryTextColor"] = "#fffeff";
                App.Current.Resources["BackgroundColor"] = "#383838";
                

                /*
                 <Color x:Key="PrimaryColor">#02874d</Color>
            <Color x:Key="SecondaryColor">#CCE7DB</Color>
            <Color x:Key="PrimaryTextColor">#4a4a4a</Color>
            <Color x:Key="SecondaryTextColor">#ffffff</Color>
            <Color x:Key="PlaceholderTextColor">#9c9a9c</Color>
            <Color x:Key="BackgroundColor">#fffeff</Color>
                 */
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
