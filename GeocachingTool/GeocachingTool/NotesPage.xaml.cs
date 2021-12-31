using GeocachingTool.Model;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPage : ContentPage
    {
        /// <summary>
        /// Page constructor
        /// </summary>
        public NotesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs on page appearance
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Get all notes and put them into the ListView
            using (SQLiteConnection connenction = new SQLiteConnection(App.DatabaseLocation))
            {
                connenction.CreateTable<Note>();

                List<Note> notes = connenction.Table<Note>().ToList();

                notesListView.ItemsSource = notes;

                // Show a label saying no notes have been created yet if no notes have been created
                if (notes.Count > 0)
                {
                    noNotesLabel.IsVisible = false;
                }
                else
                {
                    noNotesLabel.IsVisible = true;
                }
            }
        }

        /// <summary>
        /// Runs when an item in notes ListView is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void NotesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Note note = notesListView.SelectedItem as Note;

            Navigation.PushModalAsync(new NotesEditPage(note));
        }

        /// <summary>
        /// Runs when new toolbar item is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void NewToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NotesNewPage());
        }
    }
}