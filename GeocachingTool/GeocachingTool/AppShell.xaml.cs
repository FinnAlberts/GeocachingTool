﻿using GeocachingTool.Handler;
using System;
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

        /// <summary>
        /// Runs when the open review menu item is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void OpenReviewMenuItem_Clicked(object sender, EventArgs e)
        {
            ReviewHandler.OpenStoreReviewPage();
        }
    }
}