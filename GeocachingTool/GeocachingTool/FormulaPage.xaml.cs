using GeocachingTool.Model;
using GeocachingTool.Resources;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormulaPage : ContentPage
    {
        private List<FormulaLetter> formulaLetters;

        public FormulaPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Get letters from database
            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                connection.CreateTable<FormulaLetter>();

                formulaLetters = connection.Table<FormulaLetter>().ToList();

                // Put letters in ListView
                lettersListView.ItemsSource = formulaLetters;

                // If not letters have been set, show a message saying no letters have been set yet
                if (formulaLetters.Count > 0)
                {
                    noFormulaLettersLabel.IsVisible = false;
                }
                else
                {
                    noFormulaLettersLabel.IsVisible = true;
                }
            }
        }

        private void newLetterToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new FormulaNewLetterPage());
        }

        private void calculateButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            string formula = formulaEntry.Text;

            // Check if filled in
            if (String.IsNullOrEmpty(formula))
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

        private void lettersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            FormulaLetter formulaLetter = lettersListView.SelectedItem as FormulaLetter;

            Navigation.PushModalAsync(new FormulaEditLetterPage(formulaLetter));
        }

        // Function for calculating equations containing letters. Letters are replaced by their saved values.
        private string Calculate(string formula)
        {
            // Convert to lowercase to make sure e.g. 'a' and 'A' are the same
            formula = formula.ToLower();
            
            // Replace each letter by its value
            foreach (FormulaLetter formulaLetter in formulaLetters)
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

        private async void deleteAllToolbarItem_Clicked(object sender, EventArgs e)
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
                    formulaLetters = connection.Table<FormulaLetter>().ToList();

                    lettersListView.ItemsSource = formulaLetters;
                    noFormulaLettersLabel.IsVisible = true;
                }
            }
        }

        private void helpToolbarItem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert(AppResources.help, AppResources.formulaPageHelp, AppResources.ok);
        }
    }
}