using Android;
using Android.App;
using Android.Net;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;
using Android.Widget;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CheckPermission : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            TextView myTextView = FindViewById<TextView>(Resource.Id.LoginText);
            myTextView.Text = string.Empty;

            CheckCallingOrSelfPermission(Manifest.Permission.Internet);
            CheckCallingOrSelfUriPermission(Uri.Parse("https://example.com"), ActivityFlags.NoHistory);
            CheckPermission(Manifest.Permission.Internet, 0, 0);

            EnforceCallingOrSelfPermission(Manifest.Permission.Internet, "Example");
            EnforceCallingOrSelfUriPermission(Uri.Parse("https://example.com"), ActivityFlags.NoHistory, "Example");
            EnforcePermission(Manifest.Permission.Internet, 0, 0, "Example");
        }
    }
}