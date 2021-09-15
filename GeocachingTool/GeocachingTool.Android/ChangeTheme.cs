using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using GeocachingTool.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(ChangeTheme))]
namespace GeocachingTool.Droid
{
    class ChangeTheme : IChangeTheme
    {
        public void EnableDarkTheme(bool _)
        {
            AppCompatDelegate.DefaultNightMode = _ ? AppCompatDelegate.ModeNightYes : AppCompatDelegate.ModeNightNo;
        }
    }
}