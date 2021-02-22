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
