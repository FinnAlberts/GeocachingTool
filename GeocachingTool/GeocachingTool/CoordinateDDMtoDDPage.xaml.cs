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
    public partial class CoordinateDDMtoDDPage : ContentPage
    {
        public CoordinateDDMtoDDPage()
        {
            InitializeComponent();
        }

        private void convertButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            float northDegrees = float.Parse(northCoordinateEntry.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
            float northMinutes = float.Parse(northMinuteEntry.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);

            float eastDegrees = float.Parse(eastCoordinateEntry.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
            float eastMinutes = float.Parse(eastMinuteEntry.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);

            // Convert
            float north = northDegrees + northMinutes / 60;
            float east = eastDegrees + eastMinutes / 60;

            answerLabel.Text = String.Format("N{0} E{1}", north, east);
        }
    }
}