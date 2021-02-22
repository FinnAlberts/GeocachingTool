using GeocachingTool.Model;
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

                lettersListView.ItemsSource = formulaLetters;

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
            string formula = formulaEntry.Text;

            if (String.IsNullOrEmpty(formula))
            {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld. Vul alle velden in en probeer het opnieuw.", "Oke");
            }
            else
            {
                string result = Calculate(formula);

                answerLabel.Text = result;
            }
        }

        private void lettersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            FormulaLetter formulaLetter = lettersListView.SelectedItem as FormulaLetter;

            Navigation.PushModalAsync(new FormulaEditLetterPage(formulaLetter));
        }

        private string Calculate(string formula)
        {
            formula = formula.ToLower();
            
            foreach (FormulaLetter formulaLetter in formulaLetters)
            {
                formula = formula.Replace(formulaLetter.Letter.ToLower(), formulaLetter.Value.ToString());
            }

            try
            {
                var result = new DataTable().Compute(formula, null);

                return result.ToString();
            }
            catch
            {
                return "n/d";
            }
        }

        private async void deleteAllToolbarItem_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Waarschuwing", "Weet je zeker dat je alle opgeslagen letters inclusief waarde wilt verwijderen? Dit kan niet ongedaan worden gemaakt.", "Ja", "Nee");

            if (answer)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<FormulaLetter>();
                    connection.DeleteAll<FormulaLetter>();

                    formulaLetters = connection.Table<FormulaLetter>().ToList();

                    lettersListView.ItemsSource = formulaLetters;
                    noFormulaLettersLabel.IsVisible = true;
                }
            }
        }

        private void helpToolbarItem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Help", "Voeg letters toe door op 'Nieuw' te tikken. Geef voor iedere letter aan wat de waarde van deze letter is. De letter zal nu in het overzicht verschijnen. Vanaf hier kun je letter ook verwijderen of aanpassen. \n\nBovenin kun je een formule invoeren om een waarde uit te rekenen. Deze kunnen simpel zijn zoals 5+5, maar deze mogen ook letters bevatten die je hebt toegevoegd. Je kunt gebruik maken van +, -, * en /.\n\nVoorbeeld: je hebt de letter 'a' toegevoegd met als waarde 4. Door 10-a in te voeren zul je als antwoord 6 krijgen.", "Oke");
        }
    }
}