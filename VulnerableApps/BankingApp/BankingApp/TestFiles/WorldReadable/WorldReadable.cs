using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class WorldReadable : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            GetPreferences(FileCreationMode.WorldReadable);
        }
    }
}