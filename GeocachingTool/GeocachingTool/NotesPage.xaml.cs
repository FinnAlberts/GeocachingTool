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
    public partial class NotesPage : ContentPage
    {
        public NotesPage()
        {
            InitializeComponent();
        }

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

        private void notesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Note note = notesListView.SelectedItem as Note;

            Navigation.PushModalAsync(new NotesEditPage(note));
        }

        private void newToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NotesNewPage());
        }
    }
}