using GeocachingTool.Model;
using GeocachingTool.Resources;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormulaPage : ContentPage
    {
        /// <summary>
        /// List of formula letters
        /// </summary>
        private List<FormulaLetter> _formulaLetters;

        /// <summary>
        /// Page constructor
        /// </summary>
        public FormulaPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs on page appearance
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Get letters from database
            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                connection.CreateTable<FormulaLetter>();

                _formulaLetters = connection.Table<FormulaLetter>().ToList();

                // Put letters in ListView
                lettersListView.ItemsSource = _formulaLetters;

                // If not letters have been set, show a message saying no letters have been set yet
                if (_formulaLetters.Count > 0)
                {
                    noFormulaLettersLabel.IsVisible = false;
                }
                else
                {
                    noFormulaLettersLabel.IsVisible = true;
                }
            }
        }

        /// <summary>
        /// Runs when new toolbar item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewLetterToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new FormulaNewLetterPage());
        }

        /// <summary>
        /// Runs when calculate button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            string formula = formulaEntry.Text;

            // Check if filled in
            if (string.IsNullOrEmpty(formula))
            {
                // Not filled in
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            }
            else
            {
                // Calculate result
                formula = formula.Replace(",", ".");
                
                string result = Calculate(formula);
                
                // Return result
                answerLabel.Text = result;
            }
        }

        /// <summary>
        /// Runs when a letter in the ListView is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void LettersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            FormulaLetter formulaLetter = lettersListView.SelectedItem as FormulaLetter;

            Navigation.PushModalAsync(new FormulaEditLetterPage(formulaLetter));
        }

        /// <summary>
        /// Calculate formule with letters. Letters are replaced by their values in formula letters.
        /// </summary>
        /// <param name="formula">The formula</param>
        /// <returns></returns>
        private string Calculate(string formula)
        {
            // Convert to lowercase to make sure e.g. 'a' and 'A' are the same
            formula = formula.ToLower();
            
            // Replace each letter by its value
            foreach (FormulaLetter formulaLetter in _formulaLetters)
            {
                formula = formula.Replace(formulaLetter.Letter.ToLower(), formulaLetter.Value.ToString());
            }
            
            // Try to calculate the answer
            try
            {
                var result = new DataTable().Compute(formula, null);

                // Succesfully calculated, return result
                return result.ToString();
            }
            catch
            {
                // Could not calculate answer
                return "n/d";
            }
        }

        /// <summary>
        /// Runs when delete all toolbar item is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private async void DeleteAllToolbarItem_Clicked(object sender, EventArgs e)
        {
            // Ask for confirmation
            bool answer = await DisplayAlert(AppResources.warning, AppResources.areYouSureDelete, AppResources.yes, AppResources.no);

            if (answer)
            {
                // Delete all FormulaLetters
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<FormulaLetter>();
                    connection.DeleteAll<FormulaLetter>();

                    // Update ListView and "no letters have been set yet"-label
                    _formulaLetters = connection.Table<FormulaLetter>().ToList();

                    lettersListView.ItemsSource = _formulaLetters;
                    noFormulaLettersLabel.IsVisible = true;
                }
            }
        }

        /// <summary>
        /// Runs when help toolbar item is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void HelpToolbarItem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert(AppResources.help, AppResources.formulaPageHelp, AppResources.ok);
        }
    }
}