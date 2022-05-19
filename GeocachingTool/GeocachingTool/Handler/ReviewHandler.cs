using GeocachingTool.Resources;
using Plugin.StoreReview;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeocachingTool.Handler
{
    public static class ReviewHandler
    {
        /// <summary>
        /// The amount of times the app has been used
        /// </summary>
        public static int Usage
        {
            get => Preferences.Get(nameof(Usage), 0);
            set => Preferences.Set(nameof(Usage), value);
        }

        /// <summary>
        /// Ask a review when this method is called for the 20th time (alltime) (so after 20 interactions) 
        /// </summary>
        public static async void AskReviewAfterUsage()
        {
            Usage++;

            Console.WriteLine("App usage is now: {0}", Usage);

            // Check if app review should be asked
            if (Usage == 20)
            {
                // Ask if user wants to review
                bool answer = await Application.Current.MainPage.DisplayAlert(AppResources.review, AppResources.reviewBody, AppResources.yes, AppResources.no);

                if (answer)
                {
                    // Ask for review
                    AskDirectReview();
                }
            }
        }

        /// <summary>
        /// Ask a review directly
        /// </summary>
        public static async void AskDirectReview()
        {
            await CrossStoreReview.Current.RequestReview(false);
            Console.WriteLine("Review requested");
        }
    }
}
