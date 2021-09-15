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
            // Theme selection
            int preferedTheme = int.Parse(Preferences.Get("preferedTheme", "0"));

            if ((preferedTheme == 0 && Application.Current.RequestedTheme == OSAppTheme.Dark) || preferedTheme == 2)
            {
                // Apply dark theme
                App.Current.Resources["PrimaryTextColor"] = "#fffeff";
                App.Current.Resources["BackgroundColor"] = "#383838";
                DependencyService.Get<IChangeTheme>().EnableDarkTheme(true);
            } else
            {
                // Apply light theme
                DependencyService.Get<IChangeTheme>().EnableDarkTheme(false);
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
