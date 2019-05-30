using Android.App;
using Android.OS;
using Android.Webkit;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name")]
    public class WebViewJavaScriptEnabled : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            WebView myWebView = FindViewById<WebView>(Resource.Id.LoginText);
            // JavaScript is disable by default.
            myWebView.Settings.JavaScriptEnabled = true;

			string url;
			url = "http://www.example.com";
            myWebView.LoadUrl(url);
        }
    }
}