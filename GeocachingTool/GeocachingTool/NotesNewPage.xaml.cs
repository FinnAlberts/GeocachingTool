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
    public partial class NotesNewPage : ContentPage
    {
        public NotesNewPage()
        {
            InitializeComponent();
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in
            if (String.IsNullOrEmpty(nameEntry.Text) || String.IsNullOrEmpty(detailsEditor.Text)) {
                DisplayAlert("Fout", "Niet alle velden zijn ingevuld. Vul alle velden in en probeer het opnieuw.", "Oke");
            } else
            {
                // Get input
                string name = nameEntry.Text;
                string details = detailsEditor.Text;

                // Connect to database
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<Note>();

                    // Create new note
                    Note note = new Note { Name = name, Details = details };

                    int rows = connection.Insert(note);

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
    }
}