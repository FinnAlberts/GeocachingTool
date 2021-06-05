using GeocachingTool.Model;
using GeocachingTool.Resources;
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
    public partial class NotesEditPage : ContentPage
    {
        private Note note;

        public NotesEditPage(Note note)
        {
            InitializeComponent();

            this.note = note;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set existing values in entries
            nameEntry.Text = note.Name;
            detailsEditor.Text = note.Details;
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in 
            if (String.IsNullOrEmpty(nameEntry.Text) || String.IsNullOrEmpty(detailsEditor.Text))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            }
            else
            {
                // Get input
                string name = nameEntry.Text;
                string details = detailsEditor.Text;

                // Connect to database
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<Note>();

                    // Update note
                    note.Name = name;
                    note.Details = details;

                    int rows = connection.Update(note);

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

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            // Ask for confirmation
            bool answer = await DisplayAlert(AppResources.warning, AppResources.areYouSureDelete, AppResources.yes, AppResources.no);

            if (answer)
            {
                // Connect to database
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<Note>();

                    // Delete note
                    int rows = connection.Delete(note);

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