using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ExternalStorage : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            GetExternalFilesDir(Environment.DirectoryPictures);
            GetExternalFilesDirs(Environment.DirectoryPictures);
            GetExternalMediaDirs();
            GetExternalCacheDirs();
        }
    }
}