using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class LoggingInfoFQ : AppCompatActivity
    {
        private const string Tag = "MainActivity";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Util.Log.Info(Tag, "OnCreate");
        }
    }
}