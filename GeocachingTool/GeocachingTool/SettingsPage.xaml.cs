using GeocachingTool.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public Dictionary<string, string> languages;

        public SettingsPage()
        {
            InitializeComponent();

            // Fill picker with languages
            languages = new Dictionary<string, string>
            {
                {"SYSTEM LANGUAGE PLACEHOLDER", "system" },
                {"English", "en"},
                {"Nederlands", "nl"}
            };

            languagePicker.ItemsSource = languages.Keys.ToList();

            // Set currently selected language
            string selectedLanguage = Preferences.Get("preferedLanguage", "system");
            languagePicker.SelectedItem = languages.FirstOrDefault(x => x.Value == selectedLanguage).Key;
        }

        private void languagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected language
            string selectedLanguage = languages[languagePicker.SelectedItem.ToString()];

            if (selectedLanguage != Preferences.Get("preferedLanguage", "null"))
            {
                // Save selection
                Preferences.Set("preferedLanguage", selectedLanguage);

                // Notify user
                DisplayAlert(AppResources.succes, "RESTART PLACEHOLDER", AppResources.ok);
            }
        }
    }
}