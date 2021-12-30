using GeocachingTool.Model;
using GeocachingTool.Resources;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormulaNewLetterPage : ContentPage
    {
        public FormulaNewLetterPage()
        {
            InitializeComponent();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in
            if (string.IsNullOrEmpty(letterEntry.Text) || string.IsNullOrEmpty(valueEntry.Text))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            }
            else
            {
                // Get input
                string letter = letterEntry.Text;
                float value = float.Parse(valueEntry.Text);

                // Connect to database
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<FormulaLetter>();

                    // Check if letter already exists
                    List<FormulaLetter> formulaLetters = connection.Table<FormulaLetter>().ToList();

                    formulaLetters = (from formulaLetter in formulaLetters where formulaLetter.Letter == letter select formulaLetter).ToList();

                    if (formulaLetters.Count == 0) // Letter does not yet exist
                    {
                        // Insert new letter
                        FormulaLetter newFormulaLetter = new FormulaLetter { Letter = letter, Value = value };
                        int rows = connection.Insert(newFormulaLetter);

                        // Check for errors
                        if (rows > 0)
                        {
                            DisplayAlert(AppResources.succes, AppResources.succesLetterAdded, AppResources.ok);
                            Navigation.PopModalAsync();
                        }
                        else
                        {
                            DisplayAlert(AppResources.error, AppResources.errorDefault, AppResources.ok);
                        }
                    }
                    else // Letter does already exist
                    {
                        DisplayAlert(AppResources.error, AppResources.errorLetterAlreadyExists, AppResources.ok);
                    }
                }
            }
        }

        private void LetterEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isValid = Regex.IsMatch(e.NewTextValue, "^[a-zA-Z]+$");

            if (e.NewTextValue.Length > 0)
            {
                ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }
        }
    }
}