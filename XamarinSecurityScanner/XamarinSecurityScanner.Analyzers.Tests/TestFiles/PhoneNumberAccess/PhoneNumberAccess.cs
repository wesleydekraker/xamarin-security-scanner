using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Telephony;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class UniqueIdentifiers : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            TelephonyManager telephonyManager = (TelephonyManager)GetSystemService(TelephonyService);
            string phoneNumber = telephonyManager.Line1Number;
        }
    }
}