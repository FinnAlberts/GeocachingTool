using GeocachingTool.Resources;
using System;
using System.Globalization;
using System.Threading;
using Xamarin.Essentials;
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
        }

        public App(string dbLocation)
        {
            InitializeComponent();

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

            // Language selection
            string preferedLanguage = Preferences.Get("preferedLanguage", "system");

            if (preferedLanguage == "system")
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
                AppResources.Culture = CultureInfo.InstalledUICulture;
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(preferedLanguage);
                AppResources.Culture = new CultureInfo(preferedLanguage);
            }

            MainPage = new AppShell();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
