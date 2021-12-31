using GeocachingTool.Model;
using GeocachingTool.Resources;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesEditPage : ContentPage
    {
        /// <summary>
        /// The note
        /// </summary>
        private readonly Note _note;

        /// <summary>
        /// Page constructor
        /// </summary>
        /// <param name="note">The note to be edited</param>
        public NotesEditPage(Note note)
        {
            InitializeComponent();

            _note = note;
        }

        /// <summary>
        /// Runs on page appearance
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set existing values in entries
            nameEntry.Text = _note.Name;
            detailsEditor.Text = _note.Details;
        }

        /// <summary>
        /// Runs when save button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Check if filled in 
            if (string.IsNullOrEmpty(nameEntry.Text) || string.IsNullOrEmpty(detailsEditor.Text))
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
                    _note.Name = name;
                    _note.Details = details;

                    int rows = connection.Update(_note);

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
        /// Runs when delete button is clicked
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
                    connection.CreateTable<Note>();

                    // Delete note
                    int rows = connection.Delete(_note);

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