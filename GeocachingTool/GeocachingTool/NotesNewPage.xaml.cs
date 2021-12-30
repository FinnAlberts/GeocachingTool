using GeocachingTool.Model;
using GeocachingTool.Resources;
using SQLite;
using System;
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

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in
            if (string.IsNullOrEmpty(nameEntry.Text) || string.IsNullOrEmpty(detailsEditor.Text)) {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
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
                        DisplayAlert(AppResources.error, AppResources.errorDefault, AppResources.ok);
                    }
                }
            }
        }
    }
}