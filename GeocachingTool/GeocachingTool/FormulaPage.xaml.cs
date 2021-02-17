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

            string result = SolveCoordinates(formula);

            answerLabel.Text = result;
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

        private string SolveCoordinates(string coordinates)
        {
            string solvedCoordinates = String.Empty;
            int beginOfEquation = 0;
            int endOfEquation = 0;
            
            int openedBrackets = 0;
            for (int i = 0; i < coordinates.Length; i++)
            {
                if (coordinates[i] == '(')
                {
                    openedBrackets += 1;
                    if (openedBrackets == 1)
                    {
                        beginOfEquation = i;
                    }
                } 
                else if (coordinates[i] == ')')
                {
                    openedBrackets -= 1;
                    if (openedBrackets == 0)
                    {
                        endOfEquation = i;
                        solvedCoordinates += Calculate(coordinates.Substring(beginOfEquation, endOfEquation - beginOfEquation + 1));
                    }
                } else if (openedBrackets == 0)
                {
                    solvedCoordinates += coordinates[i];
                }
            }

            return solvedCoordinates;
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
                }
            }
        }

        private void helpToolbarItem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Help", "Voeg letters toe door op 'Nieuw' te tikken. Geef voor iedere letter aan wat de waarde van deze letter is. De letter zal nu in het overzicht verschijnen. Vanaf hier kun je letter ook verwijderen of aanpassen. \n\nDoor bovenin een coördinaat met formule(s) in te voeren, zal de app de coördinaten voor je uitrekenen. In de formule mogen de leters blijven staan. \n\nVoorbeeld: je hebt de letters a=3 en b=12 ingevoerd. Je hebt als formule N 51 20.(a*100) E 006 07.(b*10+17). De app geeft nu als antwoord N 51 20.300 E 006 07.137.", "Oke");
        }
    }
}