using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Method635.App.BL.Interfaces;
using Method635.App.Forms.Droid.ToastMessenger;
using Xamarin.Forms;

[assembly:Dependency(typeof(ToastMessenger))]
namespace Method635.App.Forms.Droid.ToastMessenger
{
    public class ToastMessenger : IToastMessageService
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}