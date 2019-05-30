using Android.App;
using Android.OS;
using Android.Webkit;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name")]
    public class HardcodedHttpUrl : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            WebView myWebView = FindViewById<WebView>(Resource.Id.LoginText);
            myWebView.LoadUrl("http://www.example.com");
        }
    }
}