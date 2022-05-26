using GeocachingTool.Resources;
using Plugin.StoreReview;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeocachingTool.Handler
{
    public static class ReviewHandler
    {
        private const int ActionsBeforeReviewRequest = 20;

        /// <summary>
        /// The amount of times the app has been used
        /// </summary>
        public static int AppUsage
        {
            get => Preferences.Get(nameof(AppUsage), 0);
            set => Preferences.Set(nameof(AppUsage), value);
        }

        /// <summary>
        /// Ask a review after the user has done some actions in the app (defined in ActionsBeforeReviewRequest constant)
        /// </summary>
        public static async void AskReviewAfterUsage()
        {
            // Increase usage counter
            AppUsage++;
            Console.WriteLine("App usage is now: {0}", AppUsage);

            // Check if app review should be asked
            if (AppUsage == ActionsBeforeReviewRequest)
            {
                // Create a (custom) alert
                IAlert alert = DependencyService.Get<IAlert>();
                string answer = await alert.Display(AppResources.review, AppResources.reviewBody, AppResources.yes, AppResources.later, AppResources.no);

                if (answer == AppResources.yes)
                {
                    // Ask for review
                    AskDirectReview();
                } else if (answer == AppResources.later)
                {
                    // Reset usage counter
                    AppUsage = 0;
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

        /// <summary>
        /// Open the review page in the Google Play Store
        /// </summary>
        public static void OpenStoreReviewPage()
        {
            CrossStoreReview.Current.OpenStoreReviewPage("com.finnalberts.geocachingbackpack");
        }
    }
}
