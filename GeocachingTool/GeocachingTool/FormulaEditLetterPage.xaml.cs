using GeocachingTool.Model;
using GeocachingTool.Resources;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormulaEditLetterPage : ContentPage
    {
        /// <summary>
        /// The formula letter
        /// </summary>
        private readonly FormulaLetter _formulaLetter;

        /// <summary>
        /// Page constructor
        /// </summary>
        /// <param name="formulaLetter">The letter to be edited</param>
        public FormulaEditLetterPage(FormulaLetter formulaLetter)
        {
            InitializeComponent();

            _formulaLetter = formulaLetter;
        }

        /// <summary>
        /// Runs on page appearance
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set the set data into label and entry
            letterLabel.Text = _formulaLetter.Letter;
            valueEntry.Text = _formulaLetter.Value.ToString();
        }

        /// <summary>
        /// Runs when save button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event argumets</param>
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in 
            if (string.IsNullOrEmpty(valueEntry.Text))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            }
            else
            {
                // Get input
                float value = float.Parse(valueEntry.Text);

                // Connect to database
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<FormulaLetter>();

                    // Update value of letter
                    _formulaLetter.Value = value;

                    int rows = connection.Update(_formulaLetter);

                    // Check for errors
                    if (rows > 0)
                    {
                        Navigation.PopModalAsync();
                    }
                    else
                    {
                        DisplayAlert(AppResources.error, AppResources.errorDefault, AppResources.ok);
                    }
                }
            }
        }

        /// <summary>
        /// Runs when the delete button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            // Ask for confirmation
            bool answer = await DisplayAlert(AppResources.warning, AppResources.areYouSureDelete, AppResources.yes, AppResources.no);

            if (answer)
            {
                // Connect to database
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<FormulaLetter>();

                    // Delete letter
                    int rows = connection.Delete(_formulaLetter);

                    // Check for errors
                    if (rows > 0)
                    {
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                       await DisplayAlert(AppResources.error, AppResources.errorDefault, AppResources.ok);
                    }
                }
            }
        }
    }
}