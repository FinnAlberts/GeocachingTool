﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void inputEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string word = inputEntry.Text.ToLower();

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

            foreach (char c in wordValue.ToString())
            {
                oneDigitWordValue += Int32.Parse(c.ToString());
            }

            // Return answers
            var answerString = new FormattedString();

            answerString.Spans.Add(new Span { Text = "De woordwaarde is " });
            answerString.Spans.Add(new Span { Text = wordValue.ToString(), FontAttributes = FontAttributes.Bold});
            answerString.Spans.Add(new Span { Text = ". Doorgeteld is dat " });
            answerString.Spans.Add(new Span { Text = oneDigitWordValue.ToString(), FontAttributes = FontAttributes.Bold });
            answerString.Spans.Add(new Span { Text = "." });

            answerLabel.FormattedText = answerString; 
        }
    }
}