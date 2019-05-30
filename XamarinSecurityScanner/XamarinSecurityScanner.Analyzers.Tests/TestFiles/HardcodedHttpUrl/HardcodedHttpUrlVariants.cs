using Android.App;
using Android.OS;
using Android.Webkit;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name")]
    public class HardcodedHttpUrlVariants : Activity
    {
        private const string Url = "http://www.example.com";
        private const string IgnoreUrl = "http://xamarin.com";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // http://www.example.com
            WebView myWebView = FindViewById<WebView>(Resource.Id.LoginText);
            myWebView.LoadUrl(Url);
        }
    }
}