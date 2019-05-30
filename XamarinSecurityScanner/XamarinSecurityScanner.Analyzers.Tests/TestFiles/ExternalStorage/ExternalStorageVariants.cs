using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ExternalStorageVariants : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            this.GetExternalFilesDir(Environment.DirectoryPictures);
            base.GetExternalFilesDirs(Environment.DirectoryPictures);
        }
    }
}