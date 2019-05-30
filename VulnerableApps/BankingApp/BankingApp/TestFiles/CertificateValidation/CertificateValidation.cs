using Android;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System.Net;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CertificateValidation : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
        }
    }
}