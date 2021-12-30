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
        private readonly FormulaLetter formulaLetter;

        public FormulaEditLetterPage(FormulaLetter formulaLetter)
        {
            InitializeComponent();

            this.formulaLetter = formulaLetter;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set the set data into label and entry
            letterLabel.Text = formulaLetter.Letter;
            ValueEntry.Text = formulaLetter.Value.ToString();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in 
            if (string.IsNullOrEmpty(ValueEntry.Text))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            }
            else
            {
                // Get input
                float value = float.Parse(ValueEntry.Text);

                // Connect to database
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<FormulaLetter>();

                    // Update value of letter
                    formulaLetter.Value = value;

                    int rows = connection.Update(formulaLetter);

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
                    int rows = connection.Delete(formulaLetter);

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