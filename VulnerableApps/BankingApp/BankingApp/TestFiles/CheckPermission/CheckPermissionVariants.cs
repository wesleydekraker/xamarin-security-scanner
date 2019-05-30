using Android;
using Android.App;
using Android.Net;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CheckPermissionVariants : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            this.CheckCallingOrSelfPermission(Manifest.Permission.Internet);
            base.CheckCallingOrSelfUriPermission(Uri.Parse("https://example.com"), ActivityFlags.NoHistory);
        }
    }
}