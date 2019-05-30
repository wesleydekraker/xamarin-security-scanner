using Android.App;
using Android.OS;
using Android.Webkit;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name")]
    public class WebViewJavaScriptEnabledAsVariableName : Activity
    {
        private bool _javascriptEnabled;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            WebView myWebView = FindViewById<WebView>(Resource.Id.LoginText);
            var JavaScriptEnabled = true;
            _javascriptEnabled = JavaScriptEnabled;

            myWebView.LoadUrl("http://www.example.com");
        }
    }
}