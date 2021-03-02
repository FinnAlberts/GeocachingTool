using GeocachingTool.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormulaEditLetterPage : ContentPage
    {
        private FormulaLetter formulaLetter;

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

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in 
            if (String.IsNullOrEmpty(ValueEntry.Text))
            {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld. Vul alle velden in en probeer het opnieuw.", "Oke");
            }
            else
            {
                // Get input
                int value = Int32.Parse(ValueEntry.Text);

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
                        DisplayAlert("Fout", "Er is iets misgegaan. Probeer het opnieuw.", "Oke");
                    }
                }
            }
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            // Ask for confirmation
            bool answer = await DisplayAlert("Waarschuwing", "Weet je zeker dat je deze letter inclusief waarde wilt verwijderen? Dit kan niet ongedaan worden gemaakt.", "Ja", "Nee");

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
                       await DisplayAlert("Fout", "Er is iets misgegaan. Probeer het opnieuw.", "Oke");
                    }
                }
            }
        }
    }
}