using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Page constructor
        /// </summary>
        public AppShell()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs when the website button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private async void OpenWebsiteMenuItem_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.geocaching.com/", BrowserLaunchMode.SystemPreferred);
        }
    }
}