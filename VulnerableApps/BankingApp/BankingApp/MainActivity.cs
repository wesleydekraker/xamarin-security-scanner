using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace BankingApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            EditText pincodeText = FindViewById<EditText>(Resource.Id.PincodeText);
            Button loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            TextView loginError = FindViewById<TextView>(Resource.Id.LoginError);

            loginButton.Click += (sender, e) =>
            {
                if (pincodeText.Text == "1234")
                {
                    Intent intent = new Intent(this, typeof(OverviewActivity));
                    StartActivity(intent);
                }
                else
                {
                    loginError.Text = "Verkeerde pincode!";
                }
            };
        }
	}
}

