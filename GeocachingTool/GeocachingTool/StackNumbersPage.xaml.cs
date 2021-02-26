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
    public partial class StackNumbersPage : ContentPage
    {
        public StackNumbersPage()
        {
            InitializeComponent();
        }

        private void inputEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string numberInput = inputEntry.Text;
            int answer = 0;

            if (!String.IsNullOrEmpty(numberInput))
            {
                bool done = false;
                while (!done)
                {
                    foreach (char c in numberInput)
                    {
                        if (c != ',' && c != '.' && c != '-')
                        {
                            answer += Int32.Parse(c.ToString());
                        }
                    
                    }

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
            answerLabel.Text = answer.ToString();
        }
    }
}