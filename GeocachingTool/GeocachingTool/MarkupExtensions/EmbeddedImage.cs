using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool.MarkupExtensions
{
    public class EmbeddedImage : IMarkupExtension
    {
        public string ResourceId { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(ResourceId))
            {
                return null;
            }
            else
            {
                return ImageSource.FromResource(ResourceId);
            }
        }
    }
}
