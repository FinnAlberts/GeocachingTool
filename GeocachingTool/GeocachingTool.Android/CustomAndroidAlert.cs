using System.Threading.Tasks;
using Android.App;
using GeocachingTool.Droid;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(CustomAndroidAlert))]
namespace GeocachingTool.Droid
{
    public class CustomAndroidAlert : IAlert
    {
        /// <summary>
        /// Display a non-cancelable alert with three buttons
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message of the alert</param>
        /// <param name="firstButton">First option text</param>
        /// <param name="secondthirdButtoncancel">Third option text</param>
        /// <returns>The clicked button</returns>
        public Task<string> Display(string title, string message, string firstButton, string secondButton, string thirdButton)
        {
            TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(Platform.CurrentActivity);

            alertBuilder.SetTitle(title);
            alertBuilder.SetMessage(message);
            alertBuilder.SetCancelable(false);

            alertBuilder.SetPositiveButton(firstButton, (senderAlert, args) =>
            {
                taskCompletionSource.SetResult(firstButton);
            });

            alertBuilder.SetNegativeButton(secondButton, (senderAlert, args) =>
            {
                taskCompletionSource.SetResult(secondButton);
            });

            alertBuilder.SetNeutralButton(thirdButton, (senderAlery, args) =>
            {
                taskCompletionSource.SetResult(thirdButton);
            });

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();

            return taskCompletionSource.Task;
        }
    }
}
