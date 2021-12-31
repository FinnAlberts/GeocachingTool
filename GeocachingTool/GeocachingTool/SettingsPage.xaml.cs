using GeocachingTool.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        /// <summary>
        /// Dictionary containing language label and code 
        /// </summary>
        public Dictionary<string, string> languages;

        /// <summary>
        /// Page constructor
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            // Fill picker with theme
            string[] themes = { AppResources.systemTheme, AppResources.lightTheme, AppResources.darkTheme };

            themePicker.ItemsSource = themes;

            // Fill picker with languages
            languages = new Dictionary<string, string>
            {
                {AppResources.systemLanguage, "system" },
                {"Deutsch", "de"},
                {"English", "en"},
                {"Nederlands", "nl"}
            };

            languagePicker.ItemsSource = languages.Keys.ToList();

            // Set currently selected language
            string selectedLanguage = Preferences.Get("preferedLanguage", "system");
            languagePicker.SelectedItem = languages.FirstOrDefault(x => x.Value == selectedLanguage).Key;

            // Set currently selected theme
            themePicker.SelectedIndex = int.Parse(Preferences.Get("preferedTheme", "0"));
        }

        /// <summary>
        /// Runs when a language is selected
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected language
            string selectedLanguage = languages[languagePicker.SelectedItem.ToString()];

            if (selectedLanguage != Preferences.Get("preferedLanguage", "system"))
            {
                // Save selection
                Preferences.Set("preferedLanguage", selectedLanguage);

                // Notify user
                DisplayAlert(AppResources.succes, AppResources.restart, AppResources.ok);
            }
        }

        /// <summary>
        /// Runs when a theme is selected
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected theme
            int selectedTheme = themePicker.SelectedIndex;

            if (selectedTheme.ToString() != Preferences.Get("preferedTheme", "0"))
            {
                // Save selection
                Preferences.Set("preferedTheme", selectedTheme.ToString());

                // Notify user
                DisplayAlert(AppResources.succes, AppResources.restart, AppResources.ok);
            }
        }
    }
}