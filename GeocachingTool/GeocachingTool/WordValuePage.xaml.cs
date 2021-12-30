using GeocachingTool.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WordValuePage : ContentPage
    {
        public WordValuePage()
        {
            InitializeComponent();
        }

        private void InputEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get input
            string word = inputEntry.Text.ToLower();

            // Initialize wordValue variable
            int wordValue = 0;

            // For each character in the input, get the ASCII value and subtract 96 ('a' = 97 in ASCII). Add this to wordValue.
            foreach (char c in word)
            {
                int value = (int)c;

                if (value >= 97 && value <= 122)
                {
                    wordValue += value - 96;
                }
            }

            // Add individual integers together to get a single digit answer
            int oneDigitWordValue = 0;
            int oneDigitWordValueTemp = wordValue;
            
            // While number will contain more than 1 digit, done will be false
            bool done = false;

            while (!done)
            {
                // Add every digit to the sum
                foreach (char c in oneDigitWordValueTemp.ToString())
                {
                    oneDigitWordValue += Int32.Parse(c.ToString());
                }

                // Check if answer is 1 digit long, if not: start again, with the found answer, else return the answer
                if (oneDigitWordValue.ToString().Length == 1)
                {
                    done = true;
                } 
                else
                {
                    oneDigitWordValueTemp = oneDigitWordValue;
                    oneDigitWordValue = 0;
                }
            }

            // Return answers
            var answerString = new FormattedString();

            answerString.Spans.Add(new Span { Text = AppResources.wordValuePageWordValue });
            answerString.Spans.Add(new Span { Text = wordValue.ToString(), FontAttributes = FontAttributes.Bold});
            answerString.Spans.Add(new Span { Text = AppResources.wordValuePageCountedThrough });
            answerString.Spans.Add(new Span { Text = oneDigitWordValue.ToString(), FontAttributes = FontAttributes.Bold });
            answerString.Spans.Add(new Span { Text = "." });

            answerLabel.FormattedText = answerString; 
        }
    }
}