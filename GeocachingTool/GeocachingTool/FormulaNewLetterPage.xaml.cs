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
    public partial class FormulaNewLetterPage : ContentPage
    {
        public FormulaNewLetterPage()
        {
            InitializeComponent();
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in
            if (String.IsNullOrEmpty(letterEntry.Text) || String.IsNullOrEmpty(valueEntry.Text))
            {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld. Vul alle velden in en probeer het opnieuw.", "Oke");
            }
            else
            {
                // Get input
                string letter = letterEntry.Text;
                int value = Int32.Parse(valueEntry.Text);

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
                            DisplayAlert("Succes", "De letter is succesvol toegevoegd.", "Oke");
                            Navigation.PopModalAsync();
                        }
                        else
                        {
                            DisplayAlert("Fout", "Er is iets misgegaan. Probeer het opnieuw.", "Oke");
                        }
                    }
                    else // Letter does already exist
                    {
                        DisplayAlert("Fout", "Deze letter bestaat al. Gebruik een andere letter.", "Oke");
                    }
                }
            }
        }
    }
}