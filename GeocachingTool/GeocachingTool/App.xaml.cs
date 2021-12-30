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
        /// <summary>
        /// Location of the database
        /// </summary>
        public static string DatabaseLocation = string.Empty;

        /// <summary>
        /// Constructor for the app
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor for the app
        /// </summary>
        /// <param name="dbLocation">The location of the database</param>
        public App(string dbLocation)
        {

            InitializeComponent();

            DatabaseLocation = dbLocation;
        }

        /// <summary>
        /// Runs on application start
        /// </summary>
        protected override void OnStart()
        {
            // Theme selection
            int preferedTheme = int.Parse(Preferences.Get("preferedTheme", "0"));

            SetTheme(preferedTheme);

            // Language selection
            string preferedLanguage = Preferences.Get("preferedLanguage", "system");

            SetLanguage(preferedLanguage);

            // Load main page
            MainPage = new AppShell();
        }

        /// <summary>
        /// Set the color theme for the application
        /// </summary>
        /// <param name="themeId">0 for system setting, 1 for light theme, 2 for dark theme</param>
        void SetTheme(int themeId)
        {
            // Check which theme
            if ((themeId == 0 && Application.Current.RequestedTheme == OSAppTheme.Dark) || themeId == 2)
            {
                // Apply dark theme
                Current.Resources["PrimaryTextColor"] = "#fffeff";
                Current.Resources["BackgroundColor"] = "#383838";
                DependencyService.Get<IChangeTheme>().EnableDarkTheme(true);
            }
            else
            {
                // Apply light theme
                DependencyService.Get<IChangeTheme>().EnableDarkTheme(false);
            }
        }

        /// <summary>
        /// Set the language for the application
        /// </summary>
        /// <param name="language">The language to be used. Use "system" for system language</param>
        void SetLanguage(string language)
        {
            if (language == "system")
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
                AppResources.Culture = CultureInfo.InstalledUICulture;
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                AppResources.Culture = new CultureInfo(language);
            }
        }
    }
}
