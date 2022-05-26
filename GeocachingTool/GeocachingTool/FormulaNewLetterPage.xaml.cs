using GeocachingTool.Handler;
using GeocachingTool.Model;
using GeocachingTool.Resources;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormulaNewLetterPage : ContentPage
    {
        /// <summary>
        /// Page constructor
        /// </summary>
        public FormulaNewLetterPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs when save button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
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

                    if (formulaLetters.Count == 0) 
                    {
                        // Insert new letter
                        FormulaLetter newFormulaLetter = new FormulaLetter { Letter = letter, Value = value };
                        int rows = connection.Insert(newFormulaLetter);

                        // Check for errors
                        if (rows > 0)
                        {
                            Navigation.PopModalAsync();

                            // Review handling
                            ReviewHandler.AskReviewAfterUsage();
                        }
                        else
                        {
                            DisplayAlert(AppResources.error, AppResources.errorDefault, AppResources.ok);
                        }
                    }
                    else 
                    {
                        // Letter does already exist
                        DisplayAlert(AppResources.error, AppResources.errorLetterAlreadyExists, AppResources.ok);
                    }
                }
            }
        }

        /// <summary>
        /// Runs when text in letter entry is changed
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void LetterEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            /// Check if input is 1 letter only
            bool isValid = Regex.IsMatch(e.NewTextValue, "^[a-zA-Z]+$");

            // Update the entry accordingly
            if (e.NewTextValue.Length > 0)
            {
                ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }
        }
    }
}