using GeocachingTool.Handler;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StackNumbersPage : ContentPage
    {
        /// <summary>
        /// Page constructor
        /// </summary>
        public StackNumbersPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs when input is changed
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void InputEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get input
            string numberInput = inputEntry.Text;

            // Initialize answer variable
            int answer = 0;

            // Check if filled in
            if (!string.IsNullOrEmpty(numberInput))
            {
                // While number contains more than 1 digit, done will be false
                bool done = false;
                while (!done)
                {
                    // For each digit of the number, add it to answer
                    foreach (char c in numberInput)
                    {
                        // Check for non-numberical input
                        if (c != ',' && c != '.' && c != '-')
                        {
                            answer += Int32.Parse(c.ToString());
                        }
                    
                    }

                    // Check if answer is 1 digit long, if not: start again, with the found answer, else return the answer
                    if (answer.ToString().Length == 1)
                    {
                        done = true;
                    } else
                    {
                        numberInput = answer.ToString();
                        answer = 0;
                    }
                }
            }

            // Return result
            answerLabel.Text = answer.ToString();
        }

        /// <summary>
        /// Runs on page dissappearance
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Review handling
            ReviewHandler reviewHandler = new ReviewHandler();
            reviewHandler.AskReviewAfterUsage();
        }
    }
}